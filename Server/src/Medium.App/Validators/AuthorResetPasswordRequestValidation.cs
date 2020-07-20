using FluentValidation;
using Medium.Core.Contracts.V1.Request.Authentication;
using System.Text.RegularExpressions;

namespace Medium.App.Validators
{
    public class AuthorResetPasswordRequestValidation : AbstractValidator<AuthorResetPasswordRequest>
    {
        public AuthorResetPasswordRequestValidation()
        {
            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .Must(pass =>
                    Regex.IsMatch(pass, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$"))
                .WithMessage("'Senha' não corresponde a um padrão forte");
        }
    }
}
