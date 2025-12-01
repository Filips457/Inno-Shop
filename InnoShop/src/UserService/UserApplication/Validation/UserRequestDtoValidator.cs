using FluentValidation;
using UserApplication.DTOs;

namespace UserApplication.Validation;

public class UserRequestDtoValidator : AbstractValidator<UserRequestDTO>
{
    public UserRequestDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is in incorrect format");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(50).WithMessage("Name must have 50 or less characters");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Password must have 6 or more characters");
    }
}