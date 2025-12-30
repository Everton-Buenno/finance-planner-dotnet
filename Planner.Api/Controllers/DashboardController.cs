using Microsoft.AspNetCore.Mvc;
using Planner.Application.interfaces;
using System;
using System.Threading.Tasks;

namespace Planner.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("expenses-by-category")]
        public async Task<IActionResult> GetExpensesByCategory(
            [FromQuery] Guid userId,
            [FromQuery] int year,
            [FromQuery] int month)
        {
            if (userId == Guid.Empty)
                return BadRequest(new { message = "UserId é obrigatório" });

            if (year < 2000 || year > 3000)
                return BadRequest(new { message = "Ano deve estar entre 2000 e 3000" });

            if (month < 1 || month > 12)
                return BadRequest(new { message = "Mês deve estar entre 1 e 12" });

            var result = await _dashboardService.GetExpensesByCategoryAsync(userId, year, month);

            if (!result.IsSuccess)
                return BadRequest(new { message = result.Message });

            return Ok(result);
        }

       
        [HttpGet("income-by-category")]
        public async Task<IActionResult> GetIncomeByCategory(
            [FromQuery] Guid userId,
            [FromQuery] int year,
            [FromQuery] int month)
        {
            if (userId == Guid.Empty)
                return BadRequest(new { message = "UserId é obrigatório" });

            if (year < 2000 || year > 3000)
                return BadRequest(new { message = "Ano deve estar entre 2000 e 3000" });

            if (month < 1 || month > 12)
                return BadRequest(new { message = "Mês deve estar entre 1 e 12" });

            var result = await _dashboardService.GetIncomeByCategoryAsync(userId, year, month);

            if (!result.IsSuccess)
                return BadRequest(new { message = result.Message });

            return Ok(result);
        }

  
        [HttpGet("summary")]
        public async Task<IActionResult> GetDashboardSummary(
            [FromQuery] Guid userId,
            [FromQuery] int year,
            [FromQuery] int month)
        {
            if (userId == Guid.Empty)
                return BadRequest(new { message = "UserId é obrigatório" });

            if (year < 2000 || year > 3000)
                return BadRequest(new { message = "Ano deve estar entre 2000 e 3000" });

            if (month < 1 || month > 12)
                return BadRequest(new { message = "Mês deve estar entre 1 e 12" });

            var result = await _dashboardService.GetDashboardSummaryAsync(userId, year, month);

            if (!result.IsSuccess)
                return BadRequest(new { message = result.Message });

            return Ok(result);
        }


        [HttpGet("credit-cards-summary")]
        public async Task<IActionResult> GetCreditCardsSummary(
            [FromQuery] Guid userId,
            [FromQuery] int year,
            [FromQuery] int month)
        {
            if (userId == Guid.Empty)
                return BadRequest(new { message = "UserId é obrigatório" });

            if (year < 2000 || year > 3000)
                return BadRequest(new { message = "Ano deve estar entre 2000 e 3000" });

            if (month < 1 || month > 12)
                return BadRequest(new { message = "Mês deve estar entre 1 e 12" });

            var result = await _dashboardService.GetCreditCardsSummaryAsync(userId, year, month);

            if (!result.IsSuccess)
                return BadRequest(new { message = result.Message });

            return Ok(result);
        }
    }
}
