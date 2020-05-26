using System;

namespace SpecificationPattern.Core.Models
{
    public abstract class EntityBase
    {
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public byte[] RowRevision { get; set; }
    }
}
