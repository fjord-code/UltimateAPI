using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models;

public class Company
{
    [Column("CompanyId")]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Company name is a required field.")]
    [MaxLength(60, ErrorMessage = "Maximum length of the company name is 60 characters.")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "Address is a required field.")]
    [MaxLength(60, ErrorMessage = "Maximum length of the company address is 60 characters.")]
    public string? Address { get; set; }

    public string? Country { get; set; }

    public ICollection<Employee>? Employees { get; set; }
}
