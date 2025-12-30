using Microsoft.AspNetCore.Mvc;
using Planner.Application.DTOs.TransactionDTOs;
using Planner.Application.interfaces;
using System;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class TransactionController : ControllerBase
{
    private readonly ITransactionService _transactionService;

    public TransactionController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    [HttpGet]
    public async Task<IActionResult> GetTransactions(
        Guid userId,
        Guid? bankAccountId = null,
        Guid? categoryId = null,
        DateTime? startDate = null,
        DateTime? endDate = null,
        bool? isPaid = null,
        bool? ignored = null,
        bool? isRecurring = null)
    {
        if (userId == Guid.Empty)
            return BadRequest(new { message = "Usuário não encontrado" });

        var result = await _transactionService.GetFilteredAsync(userId, bankAccountId, categoryId, startDate, endDate, isPaid, ignored, isRecurring);
        if (!result.IsSuccess)
            return BadRequest(new { message = result.Message });
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTransaction([FromBody] TransactionInputModel input)
    {
        var result = await _transactionService.AddAsync(input);
        if (!result.IsSuccess)
            return BadRequest(new { message = result.Message });
        return Ok(new { message = result.Message });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTransaction(Guid id, [FromBody] TransactionInputModel input)
    {
        var result = await _transactionService.UpdateAsync(id, input);
        if (!result.IsSuccess)
            return BadRequest(new { message = result.Message });
        return Ok(new { message = result.Message });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTransaction(Guid id)
    {
        var result = await _transactionService.DeleteAsync(id);
        if (!result.IsSuccess)
            return BadRequest(new { message = result.Message });
        return Ok(new { message = result.Message });
    }
}
