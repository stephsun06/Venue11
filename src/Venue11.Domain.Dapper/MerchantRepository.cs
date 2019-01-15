using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using System.Threading.Tasks;
using Venue11.Domain.Entities;
using Venue11.Domain.Repositories;

namespace Venue11.Domain.Dapper
{
    public class MerchantRepository : DapperRepository, IMerchantRepository
    {
        public MerchantRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public  IEnumerable<Merchant> GetMerchants()
        {
            return Connection.Query<Merchant>("GetMerchants", commandType: CommandType.StoredProcedure);
        }

        public void UpdateMerchant(Merchant entity)
        {
            var p = new DynamicParameters(new
            {
                Id = entity.id,
                LastUpdated = entity.requested
            });

            SqlExecute("UpdateMerchant", p, true);
        }
    }
}
