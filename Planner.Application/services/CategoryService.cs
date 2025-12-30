using Planner.Application.DTOs.CategoryDTOs;
using Planner.Application.DTOs;
using Planner.Application.interfaces;
using Planner.Domain.Entities;
using Planner.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Planner.Application.services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<ResultViewModel<CategoryViewModel>> GetByIdAsync(Guid id)
        {
            var result = await _categoryRepository.GetByIdAsync(id);
            if (result == null)
            {
                return ResultViewModel<CategoryViewModel>.Error("Categoria não encontrada.");
            }
            return ResultViewModel<CategoryViewModel>.Success(new CategoryViewModel
            {
                Id = result.Id,
                Name = result.Name,
                Color = result.Color,
                Icon = result.Icon,
                UserId = result.UserId,
                Type = result.Type
            });
        }

        public async Task<ResultViewModel<List<CategoryViewModel>>> GetAllByUserIdAsync(Guid userId)
        {
            var response = await _categoryRepository.GetByUserId(userId);
            if (response == null || !response.Any())
            {
                await _categoryRepository.AddDefaultCategories(userId);
                
                response = await _categoryRepository.GetByUserId(userId);
            }
            
            var categories = response.Select(c => new CategoryViewModel
            {
                Id = c.Id,
                Name = c.Name,
                Color = c.Color,
                Icon = c.Icon,
                UserId = c.UserId,
                Type = c.Type
            }).ToList();
            return ResultViewModel<List<CategoryViewModel>>.Success(categories);
        }

        public async Task<ResultViewModel> AddAsync(CategoryInputModel input)
        {
            var category = new Category(input.Name, input.Color, input.Icon, input.UserId, input.Type);
            var result = await _categoryRepository.AddAsync(category);
            if (result != null)
            {
                return ResultViewModel.Success();
            }
            else
            {
                return ResultViewModel.Error("Erro ao adicionar categoria");
            }
        }

        public async Task<ResultViewModel> UpdateAsync(Guid id, CategoryInputModel input)
        {
            if (string.IsNullOrEmpty(input.Name) || string.IsNullOrEmpty(input.Color) || string.IsNullOrEmpty(input.Icon))
            {
                return ResultViewModel.Error("Nome, Cor e Ícone são obrigatórios");
            }
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return ResultViewModel.Error("Categoria não encontrada");
            }
            category.UpdateCategory(input.Name, input.Color, input.Icon);
            await _categoryRepository.UpdateAsync(category);
            return ResultViewModel.Success();
        }

        public async Task<ResultViewModel> DeleteAsync(Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return ResultViewModel.Error("Categoria não encontrada");
            }
            await _categoryRepository.DeleteAsync(category);
            return ResultViewModel.Success();
        }
    }
} 