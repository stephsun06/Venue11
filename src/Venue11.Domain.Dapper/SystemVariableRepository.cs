
using System.Collections.Generic;
using System.Data;
using Dapper;
using Venue11.Domain.Entities;
using Venue11.Domain.Repositories;

namespace Venue11.Domain.Dapper
{
    public class SystemVariableRepository : RootRepository , ISystemVariableRepository
    {
        public SystemVariableRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public IEnumerable<SystemVariable> GetSystemVariable()
        {
                return Connection.Query<SystemVariable>("GetSystemvariable", commandType: CommandType.StoredProcedure);
        }
    }
}
