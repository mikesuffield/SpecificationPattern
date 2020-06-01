using SpecificationPattern.Shared.Enums;
using System;

namespace SpecificationPattern.Core.Models
{
    public class Allergen : EntityBase
    {        
        public Guid MenuItemId { get; set; }

        public AllergenType Name { get; set; }
    }
}
