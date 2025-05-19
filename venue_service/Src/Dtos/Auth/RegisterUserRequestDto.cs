using System.ComponentModel.DataAnnotations;
using DocsBRValidator.Attributes;

namespace venue_service.Src.Dtos.Auth;

public class RegisterUserRequestDto
{
    [Required(ErrorMessage = "First name is required.")]
    [StringLength(100, MinimumLength = 2)]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Last name is required.")]
    [StringLength(100, MinimumLength = 2)]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email.")]
    public string Email { get; set; }

    public IFormFile? Image { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Phone is required.")]
    [RegularExpression(@"^\(?[1-9]{2}\)? ?9?[6-9]\d{3}-?\d{4}$",
        ErrorMessage = "Invalid phone for Brazilian format.")]
    public string Phone { get; set; }

    [Required(ErrorMessage = "CPF is required.")]
    [CPF(ErrorMessage = "Invalid CPF.")]
    public string Cpf { get; set; }
}
