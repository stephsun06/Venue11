using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Venue11.Domain.Repositories;

namespace Venue11.Domain.Dapper
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        public IDbConnection GetOpenConnection()
        {
            var cs = GetConnectionString();
            var connection = new SqlConnection(cs);
            connection.Open();
            return connection;
        }

        private static string GetConnectionString()
        {
            var key = string.Format("Venue_{0}", Environment.MachineName);
            try
            {
                return ConfigurationManager.ConnectionStrings[key].ConnectionString;
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to get connectionstring for key: " + key, ex);
            }
        }
    }
}