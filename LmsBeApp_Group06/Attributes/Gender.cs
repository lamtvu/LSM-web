using System.ComponentModel.DataAnnotations;

namespace LmsBeApp_Group06.Attributes
{
    public class Gender : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var gender = value.ToString();
            if (gender != "Male" && gender != "Female" && gender != "Other")
            {
                return new ValidationResult("Gender can only be \"Male\", \"Female\" or \"Other\"");
            }
            return ValidationResult.Success;
        }
    }
}