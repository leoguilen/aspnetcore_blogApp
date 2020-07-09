using FluentValidation;
using Medium.Core.Contracts.V1.Request;

namespace Medium.App.Validators
{
    public class UserRegistrationRequestValidation : AbstractValidator<UserRegistrationRequest>
    {
        public UserRegistrationRequestValidation()
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
                .NotEmpty()
                .MinimumLength(6)
                .WithMessage("Senha deve conter mais de 6 caracteres");
        }
    }
}
