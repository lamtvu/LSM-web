using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LmsBeApp_Group06.Attributes
{
    public class Type : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var type = value.ToString();
            if (type != "Program" && type != "Notify")
            {
                return new ValidationResult("Type can only be \"Program\"or \"Notify\"");
            }
            return ValidationResult.Success;
        }
    }
}
