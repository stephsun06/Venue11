using System;
using System.Collections.Generic;
using Venue11.Domain.Entities;

namespace Venue11.Domain.Repositories
{
    public interface ISystemVariableRepository
    {
        IEnumerable<SystemVariable> GetSystemVariable();
       
    }
}
