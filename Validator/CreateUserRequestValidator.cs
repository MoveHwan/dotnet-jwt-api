using FluentValidation;
using Test.DTOs.User;

namespace Test.Validator
{
    public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
    {
        public CreateUserRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("이름은 필수입니다")
                .MaximumLength(50);

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("이메일은 필수입니다")
                .EmailAddress().WithMessage("올바른 이메일 형식이 아닙니다");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("비밀번호는 필수입니다")
                .MinimumLength(6).WithMessage("비밀번호는 최소 6자 이상")
                .Matches("[A-Z]").WithMessage("대문자 포함해야 합니다")
                .Matches("[0-9]").WithMessage("숫자 포함해야 합니다");

            RuleFor(x => x.Age)
                .GreaterThan(0).WithMessage("나이는 1 이상이어야 합니다.");


            // 자주 쓰는 규칙
            /*
            RuleFor(x => x.Password)
                .MinimumLength(8);

            RuleFor(x => x.Name)
                .Matches("^[a-zA-Z0-9]*$");
        
            // 조건부 검증
             RuleFor(x => x.Age)
                .GreaterThan(20)
                .When(x => x.Name == "관리자");
            */

        }
    }
}

