using SpecificationPattern.Core.Models;
using SpecificationPattern.Shared.Enums;
using System.Collections.Generic;
using System.Text;

namespace SpecificationPattern.Core.Specifications
{
    public sealed class MenuItemForMealTypeSpecification : ISpecification<MenuItem>
    {
        private readonly MealType _mealType;

        public MenuItemForMealTypeSpecification(MealType mealType)
        {
            _mealType = mealType;
        }

        public bool IsSatisfiedBy(MenuItem entity)
        {
            return entity.MealType == _mealType;
        }

        public void IsSatisfiedBy(StringBuilder builder, IDictionary<string, object> parameters)
        {
            builder.Append("[MealType] = @mealType");
            parameters.Add("mealType", _mealType.ToString());
        }
    }
}
