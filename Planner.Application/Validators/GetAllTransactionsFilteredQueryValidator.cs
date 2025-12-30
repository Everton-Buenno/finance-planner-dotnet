using FluentValidation;
using Planner.Application.DTOs.TransactionDTOs;

namespace Planner.Application.Validators
{
    public class GetAllTransactionsFilteredInputModelValidator : AbstractValidator<GetAllTransactionsFilteredInputModel>
    {
        public GetAllTransactionsFilteredInputModelValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("Usuário é obrigatório");
        }
    }
}
