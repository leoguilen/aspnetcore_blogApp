using FluentValidation;
using Medium.Core.Contracts.V1.Request.Tag;

namespace Medium.App.Validators
{
    public class CreateTagRequestValidation : AbstractValidator<CreateTagRequest>
    {
        public CreateTagRequestValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MinimumLength(4);
        }
    }
}
