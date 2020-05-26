using System;

namespace SpecificationPattern.Core.Models
{
    public class Allergen : EntityBase
    {        
        public Guid MenuItemId { get; set; }

        public string Name { get; set; } // TODO: Should this be an enum?
    }
}
