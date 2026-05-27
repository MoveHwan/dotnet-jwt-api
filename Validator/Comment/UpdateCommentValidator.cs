using FluentValidation;
using Test.DTOs.Comment;

namespace Test.Validators.Comment
{
    public class UpdateCommentValidator
        : AbstractValidator<UpdateCommentRequest>
    {
        public UpdateCommentValidator()
        {
            RuleFor(x => x.Content)
                .NotEmpty()
                .WithMessage("댓글 내용은 필수입니다.")
                .MaximumLength(300)
                .WithMessage("댓글은 300자 이하만 가능합니다.");
        }
    }
}