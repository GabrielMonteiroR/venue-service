using DocumentValidator;
using System.ComponentModel.DataAnnotations;

namespace venue_service.Src.Attributes
{
    public class CNPJAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is null) return false;
            var cnpj = value.ToString();
            return CnpjValidation.Validate(cnpj);
        }
    }
}
