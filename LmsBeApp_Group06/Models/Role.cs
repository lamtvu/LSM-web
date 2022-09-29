using System.ComponentModel.DataAnnotations;
using LmsBeApp_Group06.Attributes;

namespace LmsBeApp_Group06.Models
{
    public class Role
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(20)]
        [RoleName]
        public string RoleName { get; set; }
    }
}