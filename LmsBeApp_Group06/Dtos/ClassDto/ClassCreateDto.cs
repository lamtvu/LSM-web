using System;
using System.ComponentModel.DataAnnotations;

namespace LmsBeApp_Group06.Dtos
{
    public class ClassCreateDtos
    {
        [Required]
        [MaxLength(50)]
        [MinLength(5)]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
