using Microsoft.EntityFrameworkCore;
using Planner.Domain.Entities;
using Planner.Domain.Enums;
using Planner.Domain.Interfaces;
using Planner.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planner.Infrastructure.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(PlannerDbContext context) : base(context)
        {
        }
        public async Task AddDefaultCategories(Guid userId)
        {
            var categories = new List<Category>
                    {
                        new Category("Casa", "#FFB6B6", "pi pi-home", userId,TypeCategory.Expense),
                        new Category("Educação", "#FFD700", "pi pi-book", userId, TypeCategory.Expense),
                        new Category("Eletrônicos", "#87CEEB", "pi pi-desktop", userId, TypeCategory.Expense),
                        new Category("Lazer", "#FF69B4", "pi pi-star", userId,TypeCategory.Expense),
                        new Category("Outros", "#D3D3D3", "pi pi-ellipsis-h", userId,TypeCategory.Expense),
                        new Category("Petshop", "#FF8C00", "pi pi-paw", userId, TypeCategory.Expense),
                        new Category("Restaurante", "#FF6347", "pi pi-utensils", userId, TypeCategory.Expense),
                        new Category("Saúde", "#98FB98", "pi pi-heart", userId, TypeCategory.Expense),
                        new Category("Serviços", "#40E0D0", "pi pi-briefcase", userId, TypeCategory.Expense),
                        new Category("Supermercado", "#F08080", "pi pi-shopping-cart", userId, TypeCategory.Expense),
                        new Category("Transporte", "#00CED1", "pi pi-car", userId, TypeCategory.Expense),
                        new Category("Vestuário", "#9370DB", "pi pi-tags", userId, TypeCategory.Expense),
                        new Category("Viagem", "#4682B4", "pi pi-send", userId, TypeCategory.Expense),

                        new Category("Investimento", "#32CD32", "pi pi-chart-line", userId, TypeCategory.Income),
                        new Category("Outros", "#D3D3D3", "pi pi-ellipsis-h", userId, TypeCategory.Income),
                        new Category("Prêmio", "#DAA520", "pi pi-gift", userId, TypeCategory.Income),
                        new Category("Presente", "#FFB6C1", "pi pi-heart", userId, TypeCategory.Income),
                        new Category("Salário", "#3CB371", "pi pi-wallet", userId, TypeCategory.Income),
                    };

            await _dbSet.AddRangeAsync(categories);
            await _dbContext.SaveChangesAsync();
        }
        public async Task<IEnumerable<Category>> GetByUserId(Guid userId)
        {
            return await _dbSet
                    .AsNoTracking()
                    .Where(c => c.UserId == userId)
                    .ToListAsync();
        }
    }
}
