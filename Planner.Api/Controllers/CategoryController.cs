using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Planner.Application.DTOs.CategoryDTOs;
using Planner.Application.interfaces;
using System;
using System.Threading.Tasks;

namespace Planner.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> GetCategoryById([FromRoute] Guid id)
        {
            var result = await _categoryService.GetByIdAsync(id);
            if (!result.IsSuccess)
                return BadRequest(new { message = result.Message });
            return Ok(result);
        }

        [HttpGet("get-all/{userId}")]
        public async Task<IActionResult> GetAllCategories([FromRoute] Guid userId)
        {
            var result = await _categoryService.GetAllByUserIdAsync(userId);
            if (!result.IsSuccess)
                return BadRequest(new { message = result.Message });
            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryInputModel input)
        {
            var result = await _categoryService.AddAsync(input);
            if (!result.IsSuccess)
                return BadRequest(new { message = result.Message });
            return Ok(new { message = result.Message });
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateCategory([FromRoute] Guid id, [FromBody] CategoryInputModel input)
        {
            var result = await _categoryService.UpdateAsync(id, input);
            if (!result.IsSuccess)
                return BadRequest(new { message = result.Message });
            return Ok(new { message = result.Message });
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
        {
            var result = await _categoryService.DeleteAsync(id);
            if (!result.IsSuccess)
                return BadRequest(new { message = result.Message });
            return Ok(new { message = result.Message });
        }
    }
}
