using Planner.Application.DTOs.CategoryDTOs;
using Planner.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Planner.Application.interfaces
{
    public interface ICategoryService
    {
        Task<ResultViewModel<CategoryViewModel>> GetByIdAsync(Guid id);
        Task<ResultViewModel<List<CategoryViewModel>>> GetAllByUserIdAsync(Guid userId);
        Task<ResultViewModel> AddAsync(CategoryInputModel input);
        Task<ResultViewModel> UpdateAsync(Guid id, CategoryInputModel input);
        Task<ResultViewModel> DeleteAsync(Guid id);
    }
} 