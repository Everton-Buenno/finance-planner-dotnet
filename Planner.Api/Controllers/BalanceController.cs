using Microsoft.AspNetCore.Mvc;
using Planner.Application.DTOs.BalanceDTOs;
using Planner.Application.Helpers;
using Planner.Application.interfaces;


namespace Planner.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BalanceController : ControllerBase
    {
        private readonly IBalanceService _balanceService;
        private readonly ITransactionService _transactionService;

        public BalanceController(IBalanceService balanceService, ITransactionService transactionService)
        {
            _balanceService = balanceService;
            _transactionService = transactionService;
        }

   
        [HttpGet("monthly")]
        public async Task<IActionResult> GetMonthlyBalance(
            [FromQuery] Guid userId, 
            [FromQuery] int year, 
            [FromQuery] int month, 
            [FromQuery] Guid? bankAccountId = null)
        {
            if (userId == Guid.Empty)
                return BadRequest(new { message = "UserId é obrigatório" });

            var result = await _balanceService.GetMonthlyBalanceAsync(userId, year, month, bankAccountId);
            
            if (!result.IsSuccess)
                return BadRequest(new { message = result.Message });

            return Ok(result);
        }


        [HttpPost("monthly")]
        public async Task<IActionResult> GetMonthlyBalance([FromBody] MonthlyBalanceInputModel input)
        {
            if (input == null)
                return BadRequest(new { message = "Dados de entrada são obrigatórios" });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _balanceService.GetMonthlyBalanceAsync(input.UserId, input.Year, input.Month, input.BankAccountId);
            
            if (!result.IsSuccess)
                return BadRequest(new { message = result.Message });

            return Ok(result);
        }

      
        [HttpGet("monthly-with-transactions")]
        public async Task<IActionResult> GetMonthlyBalanceWithTransactions(
            [FromQuery] Guid userId, 
            [FromQuery] int year, 
            [FromQuery] int month, 
            [FromQuery] Guid? bankAccountId = null)
        {
            if (userId == Guid.Empty)
                return BadRequest(new { message = "UserId é obrigatório" });

            var balanceResult = await _balanceService.GetMonthlyBalanceAsync(userId, year, month, bankAccountId);
            if (!balanceResult.IsSuccess)
                return BadRequest(new { message = balanceResult.Message });

            var startDate = DateTimeHelper.CreateUtcStartOfMonth(year, month);
            var endDate = DateTimeHelper.CreateUtcEndOfMonth(year, month);
            
            var transactionsResult = await _transactionService.GetFilteredAsync(userId, bankAccountId, null, startDate, endDate, null, null, null);

            return Ok(new 
            { 
                balance = balanceResult.Data,
                transactions = transactionsResult.IsSuccess ? transactionsResult.Data : null,
                isSuccess = true,
                message = "Dados carregados com sucesso"
            });
        }
    }
}