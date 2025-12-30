using Planner.Application.DTOs;
using Planner.Application.DTOs.CreditCardDTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Planner.Application.interfaces
{
    public interface ICreditCardService
    {
    
        Task<ResultViewModel<IEnumerable<CreditCardViewModel>>> GetByUserIdAsync(Guid userId);

     
        Task<ResultViewModel<IEnumerable<CreditCardViewModel>>> GetActiveByUserIdAsync(Guid userId);

        Task<ResultViewModel<CreditCardViewModel>> GetByIdAsync(Guid id);

        Task<ResultViewModel<Guid>> CreateAsync(CreditCardInputModel input);

        Task<ResultViewModel> UpdateAsync(UpdateCreditCardInputModel input);

        Task<ResultViewModel> DeleteAsync(Guid id);

        Task<ResultViewModel<IEnumerable<CreditCardInvoiceViewModel>>> GetInvoicesByUserAndMonthAsync(Guid userId, int year, int month);

        Task<ResultViewModel<CreditCardInvoiceViewModel>> GetInvoiceByCardAndMonthAsync(Guid creditCardId, int year, int month);
        
        Task<ResultViewModel> PayInvoiceAsync(Guid creditCardId, int year, int month);
    }
}