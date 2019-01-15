using System;
using System.Data;

namespace Venue11.Domain.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        Guid Id { get; }
        void Begin(IsolationLevel isolation = IsolationLevel.ReadUncommitted);
        void Commit();
        void Rollback();
        IDbConnection GetActiveConnection();
        IDbTransaction GetActiveTransaction();
    }
}
