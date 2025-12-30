using FluentValidation;
using Planner.Application.DTOs.BankAccount;

namespace Planner.Application.Validators
{
    public class DeleteBankAccountInputModelValidator : AbstractValidator<DeleteBankAccountInputModel>
    {
        public DeleteBankAccountInputModelValidator()
        {
            RuleFor(b => b.BankAccountId)
                .NotEmpty().WithMessage("Id da conta bancária é obrigatório");
        }
    }
}
