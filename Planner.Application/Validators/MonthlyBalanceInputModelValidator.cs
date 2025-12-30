using FluentValidation;
using Planner.Application.DTOs.BalanceDTOs;

namespace Planner.Application.Validators
{
    public class MonthlyBalanceInputModelValidator : AbstractValidator<MonthlyBalanceInputModel>
    {
        public MonthlyBalanceInputModelValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("UserId é obrigatório");

            RuleFor(x => x.Year)
                .InclusiveBetween(2000, 3000)
                .WithMessage("Ano deve estar entre 2000 e 3000");

            RuleFor(x => x.Month)
                .InclusiveBetween(1, 12)
                .WithMessage("Mês deve estar entre 1 e 12");
        }
    }
}