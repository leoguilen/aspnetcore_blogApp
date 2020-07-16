using FluentValidation;
using Medium.Core.Contracts.V1.Request.Author;

namespace Medium.App.Validators
{
    public class UpdateAuthorRequestValidation : AbstractValidator<UpdateAuthorRequest>
    {
        public UpdateAuthorRequestValidation()
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
        }
    }
}
