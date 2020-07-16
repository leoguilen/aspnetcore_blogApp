using FluentValidation;
using Medium.Core.Contracts.V1.Request.Authentication;
using System.Text.RegularExpressions;

namespace Medium.App.Validators
{
    public class AuthorRegistrationRequestValidation : AbstractValidator<AuthorRegistrationRequest>
    {
        public AuthorRegistrationRequestValidation()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .MinimumLength(4)
                .Matches("^[a-zA Z0-9 ]*$");

            RuleFor(x => x.LastName)
                .NotEmpty()
                .MinimumLength(4)
                .Matches("^[a-zA Z0-9 ]*$");

            RuleFor(x => x.Username)
                .NotEmpty();

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password)
                .NotNull()
                .Must(pass =>
                    Regex.IsMatch(pass, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$"))
                .WithMessage("'Senha' não corresponde a um padrão forte");
        }
    }
}
