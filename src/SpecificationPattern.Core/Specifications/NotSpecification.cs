using System.Collections.Generic;
using System.Text;

namespace SpecificationPattern.Core.Specifications
{
    public sealed class OrSpecification<T> : ISpecification<T>
    {
        private readonly ISpecification<T> _left;
        private readonly ISpecification<T> _right;

        public OrSpecification(ISpecification<T> left, ISpecification<T> right)
        {
            _left = left;
            _right = right;
        }

        public bool IsSatisfiedBy(T entity)
        {
            return _left.IsSatisfiedBy(entity) || _right.IsSatisfiedBy(entity);
        }

        public void IsSatisfiedBy(StringBuilder builder, IDictionary<string, object> parameters)
        {
            builder.Append("(");
            _left.IsSatisfiedBy(builder, parameters);
            builder.Append(") OR (");
            _right.IsSatisfiedBy(builder, parameters);
            builder.Append(")");
        }
    }
}
