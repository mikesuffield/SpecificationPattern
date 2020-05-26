using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpecificationPattern.Shared.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        void Complete();

        Task<int> Execute(string statement, IDictionary<string, object> parameters = null);

        Task<T> ExecuteScalar<T>(string statement, IDictionary<string, object> parameters = null);

        Task<IEnumerable<T>> Query<T>(string statement, IDictionary<string, object> parameters = null);
    }
}
