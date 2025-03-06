using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects;

public record EmployeeForCreationDto
{
    [Required(ErrorMessage = $"Employee {nameof(Name)} is a required field.")]
    [MaxLength(30, ErrorMessage = $"Maximum length for the {nameof(Name)} is 30 characters.")]
    public string? Name { get; init; }

    [Required(ErrorMessage = $"{nameof(Age)} is a required field.")]
    [Range(18, int.MaxValue, ErrorMessage = "Age is required and can't be lower than 18.")]
    public int Age { get; init; }

    [Required(ErrorMessage = $"{nameof(Position)} is a required field.")]
    [MaxLength(20, ErrorMessage = $"Maximum length for the {nameof(Position)} is 20 characters.")]
    public string? Position { get; init; }
}
