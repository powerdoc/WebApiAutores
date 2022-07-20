using System.ComponentModel.DataAnnotations;

namespace WebApiAutores.Validations
{
    public class FirstLetterUpperCaseAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }

            var firstCharUpperCase = value.ToString()[0].ToString();

            if (firstCharUpperCase != firstCharUpperCase.ToUpper())
            {
                return new ValidationResult("La primera letra debe ser Mayúscula");
            }

            return ValidationResult.Success;
            //return base.IsValid(value, validationContext);  
        }
    }
}
