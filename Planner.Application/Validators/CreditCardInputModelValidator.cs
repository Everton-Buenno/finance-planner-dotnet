using FluentValidation;
using Planner.Application.DTOs.CreditCardDTOs;

namespace Planner.Application.Validators
{
    public class CreditCardInputModelValidator : AbstractValidator<CreditCardInputModel>
    {
        public CreditCardInputModelValidator()
        {
            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Descrição é obrigatória")
                .MaximumLength(100)
                .WithMessage("Descrição deve ter no máximo 100 caracteres");

            RuleFor(x => x.CreditLimit)
                .GreaterThan(0)
                .WithMessage("Limite de crédito deve ser maior que zero")
                .LessThan(1000000)
                .WithMessage("Limite de crédito deve ser menor que R$ 1.000.000");

            RuleFor(x => x.DueDay)
                .InclusiveBetween(1, 31)
                .WithMessage("Dia de vencimento deve estar entre 1 e 31");

            RuleFor(x => x.ClosingDay)
                .InclusiveBetween(1, 31)
                .WithMessage("Dia de fechamento deve estar entre 1 e 31");

            RuleFor(x => x.AccountId)
                .NotEmpty()
                .WithMessage("Conta bancária é obrigatória");

            RuleFor(x => x)
                .Must(x => x.DueDay != x.ClosingDay)
                .WithMessage("Dia de vencimento não pode ser igual ao dia de fechamento");
        }
    }
}