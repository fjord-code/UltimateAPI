using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects;

public abstract record EmployeeForManipulationDto
{
    [Required(ErrorMessage = "Employee name is a required field.")]
    [MaxLength(30, ErrorMessage = $"Maximum length for the {nameof(Name)} is 30 characters.")]
    public string? Name { get; init; }

    [Range(18, int.MaxValue, ErrorMessage = $"{nameof(Age)} is a required field and it can't be lower than 18.")]
    public int Age { get; init; }

    [Required(ErrorMessage = $"{nameof(Position)} is a required field.")]
    [MaxLength(20, ErrorMessage = $"Maximum length for the {nameof(Position)} is 20 characters.")]
    public string? Position { get; init; }
}
