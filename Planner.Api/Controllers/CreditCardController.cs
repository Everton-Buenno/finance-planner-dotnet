using Microsoft.AspNetCore.Mvc;
using Planner.Application.DTOs.CreditCardDTOs;
using Planner.Application.interfaces;
using System;
using System.Threading.Tasks;

namespace Planner.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreditCardController : ControllerBase
    {
        private readonly ICreditCardService _creditCardService;

        public CreditCardController(ICreditCardService creditCardService)
        {
            _creditCardService = creditCardService;
        }

     
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(Guid userId)
        {
            if (userId == Guid.Empty)
                return BadRequest(new { message = "UserId é obrigatório" });

            var result = await _creditCardService.GetByUserIdAsync(userId);
            
            if (!result.IsSuccess)
                return BadRequest(new { message = result.Message });

            return Ok(result);
        }

     
        [HttpGet("user/{userId}/active")]
        public async Task<IActionResult> GetActiveByUserId(Guid userId)
        {
            if (userId == Guid.Empty)
                return BadRequest(new { message = "UserId é obrigatório" });

            var result = await _creditCardService.GetActiveByUserIdAsync(userId);
            
            if (!result.IsSuccess)
                return BadRequest(new { message = result.Message });

            return Ok(result);
        }

       
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(new { message = "ID é obrigatório" });

            var result = await _creditCardService.GetByIdAsync(id);
            
            if (!result.IsSuccess)
                return BadRequest(new { message = result.Message });

            return Ok(result);
        }

      
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreditCardInputModel input)
        {
            if (input == null)
                return BadRequest(new { message = "Dados de entrada são obrigatórios" });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _creditCardService.CreateAsync(input);
            
            if (!result.IsSuccess)
                return BadRequest(new { message = result.Message });

            return CreatedAtAction(nameof(GetById), new { id = result.Data }, result);
        }

      
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCreditCardInputModel input)
        {
            if (id == Guid.Empty)
                return BadRequest(new { message = "ID é obrigatório" });

            if (input == null)
                return BadRequest(new { message = "Dados de entrada são obrigatórios" });

            if (id != input.Id)
                return BadRequest(new { message = "ID da URL não confere com o ID do body" });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _creditCardService.UpdateAsync(input);
            
            if (!result.IsSuccess)
                return BadRequest(new { message = result.Message });

            return Ok(result);
        }

       
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(new { message = "ID é obrigatório" });

            var result = await _creditCardService.DeleteAsync(id);
            
            if (!result.IsSuccess)
                return BadRequest(new { message = result.Message });

            return Ok(result);
        }

       
        [HttpGet("user/{userId}/invoices")]
        public async Task<IActionResult> GetInvoicesByUserAndMonth(Guid userId, [FromQuery] int year, [FromQuery] int month)
        {
            if (userId == Guid.Empty)
                return BadRequest(new { message = "UserId é obrigatório" });

            if (year < 2000 || year > 3000)
                return BadRequest(new { message = "Ano deve estar entre 2000 e 3000" });

            if (month < 1 || month > 12)
                return BadRequest(new { message = "Mês deve estar entre 1 e 12" });

            var result = await _creditCardService.GetInvoicesByUserAndMonthAsync(userId, year, month);
            
            if (!result.IsSuccess)
                return BadRequest(new { message = result.Message });

            return Ok(result);
        }

       
        [HttpGet("{id}/invoice")]
        public async Task<IActionResult> GetInvoiceByCardAndMonth(Guid id, [FromQuery] int year, [FromQuery] int month)
        {
            if (id == Guid.Empty)
                return BadRequest(new { message = "ID é obrigatório" });

            if (year < 2000 || year > 3000)
                return BadRequest(new { message = "Ano deve estar entre 2000 e 3000" });

            if (month < 1 || month > 12)
                return BadRequest(new { message = "Mês deve estar entre 1 e 12" });

            var result = await _creditCardService.GetInvoiceByCardAndMonthAsync(id, year, month);
            
            if (!result.IsSuccess)
                return BadRequest(new { message = result.Message });

            return Ok(result);
        }

        [HttpPost("{id}/invoice/pay")]
        public async Task<IActionResult> PayInvoice(Guid id, [FromQuery] int year, [FromQuery] int month)
        {
            if (id == Guid.Empty)
                return BadRequest(new { message = "ID é obrigatório" });

            if (year < 2000 || year > 3000)
                return BadRequest(new { message = "Ano deve estar entre 2000 e 3000" });

            if (month < 1 || month > 12)
                return BadRequest(new { message = "Mês deve estar entre 1 e 12" });

            var result = await _creditCardService.PayInvoiceAsync(id, year, month);
            
            if (!result.IsSuccess)
                return BadRequest(new { message = result.Message });

            return Ok(result);
        }
    }
}