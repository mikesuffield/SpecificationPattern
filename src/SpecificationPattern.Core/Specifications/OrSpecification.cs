using System.Collections.Generic;
using System.Text;

namespace SpecificationPattern.Core.Specifications
{
    public sealed class NotSpecification<T> : ISpecification<T>
    {
        private readonly ISpecification<T> _specification;

        public NotSpecification(ISpecification<T> specification)
        {
            _specification = specification;
        }

        public bool IsSatisfiedBy(T entity)
        {
            return !_specification.IsSatisfiedBy(entity);
        }

        public void IsSatisfiedBy(StringBuilder builder, IDictionary<string, object> parameters)
        {
            builder.Append("NOT ");
            _specification.IsSatisfiedBy(builder, parameters);
        }
    }
}
