using FluentValidation;
using Planner.Application.DTOs.TransactionDTOs;

namespace Planner.Application.Validators
{
    public class AddTransactionInputModelValidator : AbstractValidator<TransactionInputModel>
    {
        public AddTransactionInputModelValidator()
        {
            RuleFor(x => x.Value)
                .GreaterThan(0).WithMessage("O valor da transação deve ser maior que zero.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("A descrição é obrigatória.");

            RuleFor(x => x.AccountId)
                .NotEmpty().WithMessage("Conta bancária é obrigatória.");

            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage("Categoria é obrigatória.")
                .When(x => x.Type != Planner.Domain.Enums.TypeTransaction.Transfer);

            When(x => x.RepeatCount > 1, () =>
            {
                RuleFor(x => x.RepeatCount)
                    .GreaterThan(0).WithMessage("A quantidade de repetições deve ser maior que zero.");
            });

            RuleFor(x => x.Type)
                .IsInEnum().WithMessage("Tipo de transação inválido.");
        }
    }
}
