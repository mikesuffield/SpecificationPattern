using System.Collections.Generic;
using System.Text;

namespace SpecificationPattern.Core.Specifications
{
    public interface ISpecification<T>
    {
        bool IsSatisfiedBy(T entity);

        void IsSatisfiedBy(StringBuilder builder, IDictionary<string, object> parameters);
    }
}
