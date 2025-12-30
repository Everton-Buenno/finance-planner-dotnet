using FluentValidation;
using Planner.Application.DTOs.BankAccount;

namespace Planner.Application.Validators
{
    public class AddBankAccountInputModelValidator : AbstractValidator<AddBankAccountInputModel>
    {
        public AddBankAccountInputModelValidator()
        {
            RuleFor(b => b.UserId)
                .NotEmpty().WithMessage("Usuário é obrigatório");
            RuleFor(b => b.BankId)
                .NotEmpty().WithMessage("Banco é obrigatório");
            RuleFor(b => b.Description)
                .NotEmpty().WithMessage("Descrição é obrigatória")
                .MaximumLength(250).WithMessage("Descrição não pode exceder 250 caracteres");
            RuleFor(b => b.Color)
                .NotEmpty().WithMessage("Cor é obrigatória")
                .MaximumLength(7).WithMessage("Cor não pode exceder 7 caracteres");
            RuleFor(b => b.Type)
                .IsInEnum().WithMessage("Tipo de conta inválido");
        }
    }
}
