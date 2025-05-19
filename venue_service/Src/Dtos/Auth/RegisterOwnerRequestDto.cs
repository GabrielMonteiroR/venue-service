using System.ComponentModel.DataAnnotations;
using DocsBRValidator.Attributes;

namespace venue_service.Src.Dtos.Auth;

public class RegisterUserRequestDto
{
    [Required(ErrorMessage = "Nome é obrigatório.")]
    [StringLength(100, MinimumLength = 2)]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Sobrenome é obrigatório.")]
    [StringLength(100, MinimumLength = 2)]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Email é obrigatório.")]
    [EmailAddress(ErrorMessage = "Email inválido.")]
    public string Email { get; set; }

    public IFormFile? Image { get; set; }

    [Required(ErrorMessage = "Senha é obrigatória.")]
    [MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres.")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Telefone é obrigatório.")]
    [RegularExpression(@"^\(?[1-9]{2}\)? ?9?[6-9]\d{3}-?\d{4}$",
        ErrorMessage = "Telefone inválido para padrão brasileiro.")]
    public string Phone { get; set; }

    [Required(ErrorMessage = "CPF é obrigatório.")]
    [CPF(ErrorMessage = "CPF inválido.")]
    public string Cpf { get; set; }
}
