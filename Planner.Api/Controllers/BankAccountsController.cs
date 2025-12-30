using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Planner.Application.interfaces;
using Planner.Application.DTOs.BankAccount;
using System;
using System.Threading.Tasks;

namespace Planner.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankAccountsController : ControllerBase
    {
        private readonly IBankAccountService _bankAccountService;

        public BankAccountsController(IBankAccountService bankAccountService)
        {
            _bankAccountService = bankAccountService;
        }

        [HttpGet("banks")]
        public IActionResult GetAllBanks()
        {
            var result = _bankAccountService.GetAllBanks();
            return Ok(result);
        }

        [HttpGet("banks/base")]
        public IActionResult GetBaseBanks()
        {
            var result = _bankAccountService.GetBaseBanks();
            return Ok(result);
        }

        [HttpGet("banks/{bankId}/name")]
        public async Task<IActionResult> GetBankNameByBankId(int bankId)
        {
            var result = await _bankAccountService.GetBankNameByBankIdAsync(bankId);
            return Ok(result);
        }

        [HttpGet("banks/{bankId}")]
        public async Task<IActionResult> GetBankByBankId(int bankId)
        {
            var result = await _bankAccountService.GetBankByBankIdAsync(bankId);
            return Ok(result);
        }

        [HttpPost("accounts")]
        public async Task<IActionResult> AddBankAccount([FromBody] AddBankAccountInputModel input)
        {
            var result = await _bankAccountService.AddAsync(input);
            if (!result.IsSuccess)
                return BadRequest(new { message = result.Message });
            return Ok(new { message = result.Message });
        }

        [HttpPut("accounts")]
        public async Task<IActionResult> UpdateBankAccount([FromBody] UpdateBankAccountInputModel input)
        {
            var result = await _bankAccountService.UpdateAsync(input);
            if (!result.IsSuccess)
                return BadRequest(new { message = result.Message });
            return Ok(new { message = result.Message });
        }

        [HttpDelete("accounts/{id}")]
        public async Task<IActionResult> DeleteBankAccount(Guid id)
        {
            var result = await _bankAccountService.DeleteAsync(id);
            if (!result.IsSuccess)
                return BadRequest(new { message = result.Message });
            return Ok(new { message = result.Message });
        }

        [HttpGet("accounts/user/{userId}")]
        public async Task<IActionResult> GetAccountsByUserId(Guid userId)
        {
            var result = await _bankAccountService.GetAccountsByUserIdAsync(userId);
            return Ok(result);
        }
    }
}
