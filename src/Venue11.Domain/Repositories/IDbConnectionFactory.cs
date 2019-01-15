using System.Data;

namespace Venue11.Domain.Repositories
{
    public interface IDbConnectionFactory
    {
        IDbConnection GetOpenConnection();
    }
}

