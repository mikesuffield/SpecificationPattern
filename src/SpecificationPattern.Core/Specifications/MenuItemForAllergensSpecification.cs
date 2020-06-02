using SpecificationPattern.Core.Models;
using SpecificationPattern.Shared.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpecificationPattern.Core.Specifications
{
    public sealed class MenuItemForAllergensSpecification : ISpecification<MenuItem>
    {
        private readonly IEnumerable<AllergenType> _allergens;

        public MenuItemForAllergensSpecification(IEnumerable<AllergenType> allergens)
        {
            _allergens = allergens;
        }

        public bool IsSatisfiedBy(MenuItem entity)
        {
            return entity.Allergens.Select(x => x.AllergenType).Intersect(_allergens).Any();
        }

        public void IsSatisfiedBy(StringBuilder builder, IDictionary<string, object> parameters)
        {
            if (_allergens != null & _allergens.Any())
            {
                builder.Append("[Id] IN (SELECT MenuItemId FROM Allergens WHERE AllergenType IN @allergens)");
                parameters.Add("allergens", _allergens.Select(x => x.ToString()));
            }
        }
    }
}
