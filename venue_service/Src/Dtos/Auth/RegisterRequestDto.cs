using System.ComponentModel.DataAnnotations;

namespace venue_service.Src.Dtos.Auth;

public class RegisterRequestDto
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

    [Required(ErrorMessage = "Senha é obrigatória.")]
    [MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres.")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Telefone é obrigatório.")]
    [RegularExpression(@"^\(?[1-9]{2}\)? ?9?[6-9]\d{3}-?\d{4}$",
        ErrorMessage = "Telefone inválido para padrão brasileiro.")]
    public string Phone { get; set; }

    [Required(ErrorMessage = "RoleId é obrigatório.")]
    [Range(1, 3, ErrorMessage = "RoleId deve ser entre 1 e 3.")]
    public int RoleId { get; set; }
}
