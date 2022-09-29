using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LmsBeApp_Group06.Models
{
    public class Invitation
    {
        [Required]
        [Key]
        public int Id { get; set; }
        public int ReceiverId { get; set; }

        [Required]
        [ForeignKey(nameof(ReceiverId))]
        public User Receiver { get; set; }

        public int ClassId { get; set; }

        [Required]
        [ForeignKey(nameof(ClassId))]
        public Class Class { get; set; }
    }
}
