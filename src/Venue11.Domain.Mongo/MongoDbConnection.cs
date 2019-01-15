using System;
using System.Configuration;

namespace Venue11.Domain.Mongo
{
    public class MongoDbConnection : IMongoDbConnection
    {
        public string GetConnectionString()
        {
            var key = string.Format("MongoDB_{0}", Environment.MachineName);

            return ConfigurationManager.AppSettings[key];
        }
    }
}
