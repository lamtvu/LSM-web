using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace LmsBeApp_Group06.Attributes
{
    public class Time : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var timeValue = value.ToString().Split(':');
            if (timeValue.ToList().Count != 2)
            {
                return new ValidationResult("wrong format time value, format is 'HH:mm'");
            }
            if (timeValue[0].Length != 2 || timeValue[0].Length != 2)
            {
                return new ValidationResult("wrong format time value, format is 'HH:mm'");
            }
            int hour, min;
            if (!int.TryParse(timeValue[0], out hour) || !int.TryParse(timeValue[1], out min))
            {
                return new ValidationResult("wrong format time value, format is 'HH:mm'");
            }
            if ((hour > 23 || hour < 0) || (min > 59 || min < 0))
            {
                return new ValidationResult("wrong format time value, format is 'HH:mm'");
            }
            return ValidationResult.Success;
        }
    }
}
