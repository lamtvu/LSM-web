using System;
using System.ComponentModel.DataAnnotations;

namespace LmsBeApp_Group06.Dtos
{
    public class ClassReadDto
    {
        [Key]
        public int Id { get; set; }

        public int TeacherId { get; set; }

        public UserReadDto Teacher { get; set; }
        public UserReadDto ClassAdmin { get; set; }
        public UserReadDto Assistant { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime CreateDate { get; set; }
    }
}

