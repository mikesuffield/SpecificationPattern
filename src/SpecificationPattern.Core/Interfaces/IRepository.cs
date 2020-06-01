using SpecificationPattern.Core.Models;
using SpecificationPattern.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpecificationPattern.Core.Interfaces
{
    public interface IRepository<T> where T : EntityBase
    {   
        Task<T> Add(T entity);

        Task<T> Find(Guid id);

        Task<IEnumerable<T>> All();

        Task<IEnumerable<T>> All(ISpecification<T> specification);

        Task Remove(T entity);
    }
}
