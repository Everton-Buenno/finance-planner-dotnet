using FluentValidation;
using Planner.Application.DTOs.TransactionDTOs;
using Planner.Domain.Enums;

namespace Planner.Application.Validators
{
    public class TransactionInputModelValidator : AbstractValidator<TransactionInputModel>
    {
        public TransactionInputModelValidator()
        {
            RuleFor(x => x.DateTransaction)
                .NotEmpty()
                .WithMessage("Data da transação é obrigatória");

            RuleFor(x => x.Value)
                .GreaterThan(0)
                .WithMessage("Valor deve ser maior que zero");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Descrição é obrigatória")
                .MaximumLength(255)
                .WithMessage("Descrição deve ter no máximo 255 caracteres");

            RuleFor(x => x.Type)
                .IsInEnum()
                .WithMessage("Tipo de transação inválido");

            RuleFor(x => x.AccountId)
                .NotEmpty()
                .WithMessage("Conta bancária é obrigatória");

            RuleFor(x => x.CreditCardId)
                .NotEmpty()
                .WithMessage("Cartão de crédito é obrigatório para despesas de cartão de crédito")
                .When(x => x.Type == TypeTransaction.CreditCardExpense);

            RuleFor(x => x.CreditCardId)
                .Null()
                .WithMessage("Cartão de crédito só pode ser informado para despesas de cartão de crédito")
                .When(x => x.Type != TypeTransaction.CreditCardExpense);

            RuleFor(x => x.RepeatCount)
                .GreaterThan(0)
                .WithMessage("Quantidade de repetições deve ser maior que zero");
        }
    }
}