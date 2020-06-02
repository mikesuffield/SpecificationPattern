using SpecificationPattern.Shared.Enums;
using System;

namespace SpecificationPattern.Application.DTOs
{
    public class AllergenDto
    {
        public Guid Id { get; set; }

        public AllergenType AllergenType { get; set; }
    }
}
