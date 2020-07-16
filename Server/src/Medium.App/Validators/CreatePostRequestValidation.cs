﻿using FluentValidation;
using Medium.Core.Contracts.V1.Request;

namespace Medium.App.Validators
{
    public class CreatePostRequestValidation : AbstractValidator<CreatePostRequest>
    {
        public CreatePostRequestValidation()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(50);
        }
    }
}
