using System.ComponentModel.DataAnnotations;
using DocumentValidator;

namespace venue_service.Src.Attributes;

public class CPFAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        if(value is null) return false;

        var cpf = value.ToString();
        return CpfValidation.Validate(cpf);
    }
}
