using System.ComponentModel.DataAnnotations;

namespace LmsBeApp_Group06.Attributes
{
    public class RoleName : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var rolename = value.ToString();
            if (rolename != "Admin" && rolename != "Student" && rolename != "Teacher" && rolename != "Instructor")
            {
                return new ValidationResult("Role name can only be Admin, Student, Teacher, Instructor");
            }
            return ValidationResult.Success;
        }
    }
}