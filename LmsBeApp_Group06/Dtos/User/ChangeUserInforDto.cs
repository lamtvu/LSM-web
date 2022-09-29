using System.ComponentModel.DataAnnotations;
using LmsBeApp_Group06.Attributes;

namespace LmsBeApp_Group06.Dtos
{
    public class ChangeUserInforDto
    {

        [Gender]
        public string Gender { get; set; }
        [MinLength(5)]
        [MaxLength(50)]
        public string FullName { get; set; }
        [MaxLength(15)]
        [Phone]
        public string Phone { get; set; }
    }
}
