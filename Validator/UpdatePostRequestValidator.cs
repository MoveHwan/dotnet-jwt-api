using FluentValidation;
using Test.DTOs.Post;

namespace Test.Validator
{
    public class UpdatePostRequestValidator : AbstractValidator<UpdatePostRequest>
    {
        public UpdatePostRequestValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("제목은 필수입니다")
                .MaximumLength(100).WithMessage("제목은 100자 이하입니다");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("내용은 필수입니다")
                .MinimumLength(10).WithMessage("내용은 최소 10자 이상이어야 합니다");
        }
    }
}
