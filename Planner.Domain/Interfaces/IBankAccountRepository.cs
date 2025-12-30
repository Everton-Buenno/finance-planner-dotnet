using Planner.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planner.Domain.Interfaces
{
    public interface IBankAccountRepository:IBaseRepository<BankAccount>
    {
        Task<IEnumerable<BankAccount>> GetAccountsByUserId(Guid userId);

    }
}
