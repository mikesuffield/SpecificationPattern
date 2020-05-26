using Microsoft.Extensions.Logging;
using SpecificationPattern.Core.Interfaces;
using SpecificationPattern.Core.Models;
using SpecificationPattern.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
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
            await AddMenuItem(entity);
            await AddAllergens(entity);

            return entity;
        }

        public async Task<IEnumerable<MenuItem>> All()
        {
            var getAllMenuItemsQuery = "SELECT * FROM MenuItems";
            var menuItems = await UnitOfWork.Query<MenuItem>(getAllMenuItemsQuery);

            if (menuItems.Any())
            {
                var getAllAllergensQuery = "SELECT * FROM Allergens";
                var allergens = await UnitOfWork.Query<Allergen>(getAllAllergensQuery);

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

            var getMenuItemsByIdQuery = "SELECT * FROM MenuItems WHERE Id = @id";
            var menuItems = await UnitOfWork.Query<MenuItem>(getMenuItemsByIdQuery, idParameter);
           
            var menuItem = menuItems.SingleOrDefault();
            if (menuItem == null)
            {
                LogAndThrow($"MenuItem with id {id} could not be found");

            }

            var getAllergensByMenuItemIdQuery = "SELECT * FROM Allergens WHERE MenuItemId = @id";
            var allergens = await UnitOfWork.Query<Allergen>(getAllergensByMenuItemIdQuery, idParameter);
            menuItem.Allergens = allergens;

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
                var insertAllergensQuery = "INSERT INTO Allergens ([Id], [MenuItemId], [Name], [CreatedAt]) OUTPUT Inserted.RowRevision VALUES (@id, @menuItemId, @name, @createdAt)";
                var insertAllergensParameters = new Dictionary<string, object>()
                {
                    { "id", allergen.Id },
                    { "menuItemId", entity.Id },
                    { "name", allergen.Name },
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
