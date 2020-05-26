using SpecificationPattern.Core.Models;
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

        Task Remove(T entity);
    }
}
