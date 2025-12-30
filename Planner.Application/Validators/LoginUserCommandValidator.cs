using FluentValidation;
using Planner.Application.DTOs.Auth;

namespace Planner.Application.Validators
{
    public class LoginUserInputModelValidator : AbstractValidator<LoginUserInputModel>
    {
        public LoginUserInputModelValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("E-mail é obrigatório.")
                .EmailAddress().WithMessage("E-mail inválido.");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Senha é obrigatória.");
        }
    }
}
