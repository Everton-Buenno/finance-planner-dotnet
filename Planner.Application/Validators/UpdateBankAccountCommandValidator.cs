using FluentValidation;
using Planner.Application.DTOs.BankAccount;

namespace Planner.Application.Validators
{
    public class UpdateBankAccountInputModelValidator : AbstractValidator<UpdateBankAccountInputModel>
    {
        public UpdateBankAccountInputModelValidator()
        {
            RuleFor(b => b.Id)
                .NotEmpty().WithMessage("ID da conta bancária é obrigatório");
        }
    }
}
