using FluentValidation;
using ProductApplication.DTOs;

namespace ProductApplication.Validation;

public class ProductDtoValidator : AbstractValidator<ProductDTO>
{
    public ProductDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required");

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0).WithMessage("Price can't be negative value!");

        RuleFor(x => x.CreationTime)
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Creation time can't be in future!");
    }
}