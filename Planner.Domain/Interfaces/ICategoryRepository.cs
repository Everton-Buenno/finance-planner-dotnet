using Planner.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planner.Domain.Interfaces
{
    public interface ICategoryRepository:IBaseRepository<Category>
    {
        Task<IEnumerable<Category>> GetByUserId(Guid userId);

        Task AddDefaultCategories(Guid userId);
    }
}
