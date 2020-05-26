using Dapper;
using Microsoft.Extensions.Configuration;
using SpecificationPattern.Shared.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SpecificationPattern.Infrastructure.Sql
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool _complete = false;
        private IDbConnection _sqlConnection;
        private IDbTransaction _sqlTransaction;

        public UnitOfWork(IConfiguration configuration)
        {
            var connString = configuration.GetConnectionString("SpecificationPattern");

            if (_sqlConnection == null)
            {
                _sqlConnection = new SqlConnection(connString);
            }

            if (_sqlConnection.State != ConnectionState.Open)
            {
                _sqlConnection.Open();
            }
        }

        public void Complete()
        {
            _complete = true;

            if(_sqlTransaction != null)
            {
                try
                {
                    _sqlTransaction.Commit();
                }
                catch
                {
                    _sqlTransaction.Rollback();
                    throw;
                }
                finally
                {
                    _sqlTransaction.Dispose();
                    _sqlTransaction = null;
                }
            }
        }

        public void Dispose()
        {
            if (!_complete)
            {
                _sqlTransaction?.Rollback();
                _sqlTransaction?.Dispose();
                _sqlConnection?.Close();
            }

            _sqlConnection?.Dispose();
            _sqlTransaction = null;
            _sqlConnection = null;
        }

        public async Task<int> Execute(string statement, IDictionary<string, object> parameters = null)
        {
            BeginTransaction();
            return await _sqlConnection.ExecuteAsync(statement, parameters, _sqlTransaction);
        }

        public async Task<T> ExecuteScalar<T>(string statement, IDictionary<string, object> parameters = null)
        {
            BeginTransaction();
            return (T)await _sqlConnection.ExecuteScalarAsync<T>(statement, parameters, _sqlTransaction);
        }

        public async Task<IEnumerable<T>> Query<T>(string statement, IDictionary<string, object> parameters = null)
        {
            return await _sqlConnection.QueryAsync<T>(statement, parameters, _sqlTransaction);
        }

        private void BeginTransaction()
        {
            if (_sqlTransaction == null)
            {
                _sqlTransaction = _sqlConnection.BeginTransaction();
            }
        }
    }
}
