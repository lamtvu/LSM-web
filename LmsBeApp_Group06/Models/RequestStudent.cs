using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using LmsBeApp_Group06.Dtos;

namespace LmsBeApp_Group06.Models
{
    public class RequestStudent
    {
        [Required]
        [Key]
        public int Id { get; set; }

        public int SenderId { get; set; }

        [Required]
        [ForeignKey(nameof(SenderId))]
        public User Sender { get; set; }

        public int ClassId { get; set; }

        [Required]
        [ForeignKey(nameof(ClassId))]
        public Class Class { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime CreateDate { get; set; }
    }
}
