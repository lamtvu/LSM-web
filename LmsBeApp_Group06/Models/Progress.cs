using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LmsBeApp_Group06.Models
{
    public class Progress
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        [Required]
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        public int ContentId { get; set; }

        [Required]
        [ForeignKey(nameof(ContentId))]
        public Content Content { get; set; }

    }
}
