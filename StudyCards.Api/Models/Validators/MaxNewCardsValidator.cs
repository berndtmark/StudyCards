using StudyCards.Api.Models.Request;
using System.ComponentModel.DataAnnotations;

namespace StudyCards.Api.Models.Validators;

public interface IAddUpdateDeckRequest
{
    public int ReviewsPerDay { get; set; }
    public int NewCardsPerDay { get; set; }
}

public class MaxNewCardsValidator : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (validationContext.ObjectInstance is not IAddUpdateDeckRequest model)
        {
            return new ValidationResult("Invalid object type");
        }

        if (model.NewCardsPerDay <= model.ReviewsPerDay)
        {
            return ValidationResult.Success;
        }

        return new ValidationResult(
            "NewCardsPerDay must be less than or equal to ReviewsPerDay",
            [nameof(AddDeckRequest.NewCardsPerDay)]
        );
    }
}