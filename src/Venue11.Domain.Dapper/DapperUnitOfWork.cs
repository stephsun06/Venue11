
using System;
using System.Data;
using Venue11.Domain.Repositories;

namespace Venue11.Domain.Dapper
{
    public class DapperUnitOfWork : IUnitOfWork
    {
        private IDbConnection connection;
        private IDbTransaction transaction;
        public Guid Id { get; private set; }

        public DapperUnitOfWork(IDbConnectionFactory connectionFactory)
        {
            connection = connectionFactory.GetOpenConnection();
            Id = Guid.NewGuid();
        }

        public void Begin(IsolationLevel isolation = IsolationLevel.ReadUncommitted)
        {
            transaction = connection.BeginTransaction(isolation);
        }

        public void Commit()
        {
            transaction.Commit();
            transaction = null;
        }

        public void Rollback()
        {
            transaction.Rollback();
            transaction = null;
        }

        public IDbConnection GetActiveConnection()
        {
            return connection;
        }

        public IDbTransaction GetActiveTransaction()
        {
            return transaction;
        }

        public void Dispose()
        {
            if (connection.State != ConnectionState.Closed)
            {
                if (transaction != null)
                    transaction.Rollback();
                connection.Close();
                connection = null;
            }
            GC.SuppressFinalize(this);
        }
    }
}

