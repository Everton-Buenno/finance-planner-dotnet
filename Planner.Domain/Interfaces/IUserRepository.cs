using Planner.Domain.Entities;
using Planner.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planner.Domain.Interfaces
{
    public interface IUserRepository:IBaseRepository<User>
    {
        Task<User> GetByEmailAsync(Email email);
        Task<bool> ExistsByEmailAsync(Email email);
    }
}
