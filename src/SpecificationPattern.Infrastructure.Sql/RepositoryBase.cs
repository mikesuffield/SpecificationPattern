using SpecificationPattern.Shared.Interfaces;

namespace SpecificationPattern.Infrastructure.Sql
{
    public abstract class RepositoryBase
    {
        protected IUnitOfWork UnitOfWork;

        public RepositoryBase(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }
    }
}
