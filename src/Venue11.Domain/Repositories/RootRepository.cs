using System;
using System.Data;

namespace Venue11.Domain.Repositories
{
    public class RootRepository : IDisposable
    {
        protected readonly IDbConnection Connection;
        protected readonly IDbTransaction ActiveTransaction;
        protected RootRepository(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null) throw new ArgumentNullException("unitOfWork");
            ActiveTransaction = unitOfWork.GetActiveTransaction();
            Connection = unitOfWork.GetActiveConnection();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    Connection.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
