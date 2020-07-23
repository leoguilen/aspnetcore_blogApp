using FluentValidation;
using Medium.Core.Contracts.V1.Request.Author;
using System.Text.RegularExpressions;

namespace Medium.App.Validators
{
    public class UpdateAuthorRequestValidation : AbstractValidator<UpdateAuthorRequest>
    {
        public UpdateAuthorRequestValidation()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .MinimumLength(4)
                .Must(first =>
                    Regex.IsMatch(first, @"^[A-Z][a-zA-Z]*$"));

            RuleFor(x => x.LastName)
                .NotEmpty()
                .MinimumLength(4)
                .Must(first =>
                    Regex.IsMatch(first, @"^[A-Z][a-zA-Z]*$"));

            RuleFor(x => x.Username)
                .NotEmpty();

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();
        }
    }
}
