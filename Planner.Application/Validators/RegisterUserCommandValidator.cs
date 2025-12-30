using FluentValidation;
using Planner.Application.DTOs.Auth;

namespace Planner.Application.Validators
{
    public class RegisterUserInputModelValidator : AbstractValidator<RegisterUserInputModel>
    {
        public RegisterUserInputModelValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Nome é obrigatório.");
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("E-mail é obrigatório.")
                .EmailAddress().WithMessage("E-mail inválido.");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Senha é obrigatória.")
                .MinimumLength(6).WithMessage("A senha deve ter pelo menos 6 caracteres.");
        }
    }
}
