using FluentValidation;
using UserApplication.DTOs;

namespace UserApplication.Validation;

public class UserDtoValidator : AbstractValidator<UserDTO>
{
    public UserDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is in incorrect format");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(50).WithMessage("Name must have 50 or less characters");
    }
}