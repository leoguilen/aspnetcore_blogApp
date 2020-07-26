using FluentValidation;
using Medium.Core.Contracts.V1.Request.Tag;

namespace Medium.App.Validators
{
    public class UpdateTagRequestValidation : AbstractValidator<UpdateTagRequest>
    {
        public UpdateTagRequestValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MinimumLength(4);
        }
    }
}
