using Microsoft.Extensions.Logging;
using SpecificationPattern.Core.Interfaces;
using SpecificationPattern.Core.Models;
using SpecificationPattern.Core.Specifications;
using SpecificationPattern.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecificationPattern.Infrastructure.Sql
{
    public class MenuItemRepository : RepositoryBase, IMenuItemRepository
    {
        private readonly ILogger<MenuItemRepository> _logger;

        public MenuItemRepository(
            IUnitOfWork unitOfWork,
             ILogger<MenuItemRepository> logger)
            : base(unitOfWork)
        {
            _logger = logger;
        }

        public async Task<MenuItem> Add(MenuItem entity)
        {
            entity.Id = Guid.NewGuid();
            await AddMenuItem(entity);
            await AddAllergens(entity);

            return entity;
        }

        public async Task<IEnumerable<MenuItem>> All()
        {
            var queries = "SELECT * FROM MenuItems; SELECT * FROM Allergens";

            var result = await UnitOfWork.QueryMultiple(queries);
            var menuItems = result.Read<MenuItem>(true);

            if (menuItems.Any())
            {
                var allergens = result.Read<Allergen>(true);

                foreach (var menuItem in menuItems)
                {
                    menuItem.Allergens = allergens.Where(x => x.MenuItemId == menuItem.Id).AsEnumerable();
                }
            }

            return menuItems;
        }


        public async Task<IEnumerable<MenuItem>> All(ISpecification<MenuItem> specification)
        {
            var getAllMenuItemsQueryBuilder = new StringBuilder("SELECT * FROM MenuItems WHERE ");
            var parameters = new Dictionary<string, object>();

            specification.IsSatisfiedBy(getAllMenuItemsQueryBuilder, parameters);

            var menuItems = await UnitOfWork.Query<MenuItem>(getAllMenuItemsQueryBuilder.ToString(), parameters);

            if (menuItems.Any())
            {
                var menuItemIds = menuItems.Select(x => x.Id);

                var getAllAllergensQuery = $"SELECT * FROM Allergens WHERE MenuItemId IN @menuItemIds";
                var getAllAllergensParameters = new Dictionary<string, object>()
                {
                    { "menuItemIds", menuItemIds },
                };
                var allergens = await UnitOfWork.Query<Allergen>(getAllAllergensQuery, getAllAllergensParameters);

                foreach (var menuItem in menuItems)
                {
                    menuItem.Allergens = allergens.Where(x => x.MenuItemId == menuItem.Id).AsEnumerable();
                }
            }

            return menuItems;
        }

        public async Task<MenuItem> Find(Guid id)
        {
            var idParameter = new Dictionary<string, object>()
            {
                { "id", id },
            };

            string queries = "SELECT * FROM MenuItems WHERE Id = @id; SELECT * FROM Allergens WHERE MenuItemId = @id";

            var result = await UnitOfWork.QueryMultiple(queries, idParameter);

            var menuItem = result.ReadSingleOrDefault<MenuItem>();

            if (menuItem == null)
            {
                _logger.LogInformation($"MenuItem with id {id} could not be found");
                return null;
            }

            menuItem.Allergens = result.Read<Allergen>();

            return menuItem;
        }

        public async Task Remove(MenuItem entity)
        {
            await RemoveAllergens(entity);
            await RemoveMenuItem(entity);
        }

        private async Task RemoveAllergens(MenuItem entity)
        {
            foreach (var allergen in entity.Allergens)
            {
                var deleteAllergenQuery = "DELETE FROM Allergens WHERE Id = @id AND RowRevision = @rowRevision";
                var deleteAllergenParameters = new Dictionary<string, object>()
                {
                    { "id", allergen.Id },
                    { "rowRevision", allergen.RowRevision },
                };

                var allergenRecordsAffected = await UnitOfWork.Execute(deleteAllergenQuery, deleteAllergenParameters);
                if (allergenRecordsAffected == 0)
                {
                    LogAndThrow($"MenuItem with id {entity.Id} could not be deleted as one or more allergens do not exist or concurrency check failed");

                }
            }
        }

        private async Task RemoveMenuItem(MenuItem entity)
        {
            var deleteMenuItemQuery = "DELETE FROM MenuItems WHERE Id = @id AND RowRevision = @rowRevision";
            var deleteMenuItemParameters = new Dictionary<string, object>()
            {
                { "id", entity.Id },
                { "rowRevision", entity.RowRevision },
            };

            var recordsAffected = await UnitOfWork.Execute(deleteMenuItemQuery, deleteMenuItemParameters);
            if (recordsAffected == 0)
            {
                LogAndThrow($"MenuItem with id {entity.Id} could not be deleted as it does not exist or concurrency check failed");
            }
        }

        private async Task AddMenuItem(MenuItem entity)
        {
            var now = DateTime.UtcNow;

            var insertMenuItemQuery = "INSERT INTO MenuItems ([Id], [Name], [Price], [MealType], [CreatedAt]) OUTPUT Inserted.RowRevision VALUES (@id, @name, @price, @mealType, @createdAt)";
            var insertMenuItemParameters = new Dictionary<string, object>()
            {
                { "id", entity.Id },
                { "name", entity.Name },
                { "price", entity.Price },
                { "mealType", entity.MealType.ToString() },
                { "createdAt", now },
            };

            var rowRevision = await UnitOfWork.ExecuteScalar<byte[]>(insertMenuItemQuery, insertMenuItemParameters);
            if (rowRevision == null)
            {
                LogAndThrow($"MenuItem with id {entity.Id} could not be added to the database");
            }

            entity.RowRevision = rowRevision;
            entity.CreatedAt = now;
        }

        private async Task AddAllergens(MenuItem entity)
        {
            var now = DateTime.UtcNow;

            foreach (var allergen in entity.Allergens)
            {
                var insertAllergensQuery = "INSERT INTO Allergens ([Id], [MenuItemId], [AllergenType], [CreatedAt]) OUTPUT Inserted.RowRevision VALUES (@id, @menuItemId, @allergenType, @createdAt)";
                var insertAllergensParameters = new Dictionary<string, object>()
                {
                    { "id", Guid.NewGuid() },
                    { "menuItemId", entity.Id },
                    { "allergenType", allergen.AllergenType.ToString() },
                    { "createdAt", now },
                };

                var allergenRowRevision = await UnitOfWork.ExecuteScalar<byte[]>(insertAllergensQuery, insertAllergensParameters);
                if (allergenRowRevision == null)
                {
                    LogAndThrow($"MenuItem with id {entity.Id} could not be added to the database");
                }

                allergen.RowRevision = allergenRowRevision;
                allergen.CreatedAt = now;
            }
        }

        private void LogAndThrow(string message)
        {
            _logger.LogError(message);
            throw new Exception(message);
        }
    }
}
