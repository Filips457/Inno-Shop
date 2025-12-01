using FluentValidation;
using ProductApplication.DTOs;

namespace ProductApplication.Validation;

public class ProductRequestDtoValidator : AbstractValidator<ProductRequestDTO>
{
    public ProductRequestDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required");

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0).WithMessage("Price can't be negative value!");
    }
}