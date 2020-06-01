using System.ComponentModel.DataAnnotations;

namespace SpecificationPattern.Shared.Enums
{
    public enum AllergenType
    {
        Unknown,
        Celery,
        [Display(Name = "Cereals containing gluten")]
        CerealsContainingGluten,
        Crustaceans,
        Eggs,
        Fish,
        Lupin,
        Milk,
        Molluscs,
        Mustard,
        Nuts,
        Peanuts,
        Sesame,
        Soya,
        Sulphites,
    }
}
