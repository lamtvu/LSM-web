using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;


namespace LmsBeApp_Group06.Models
{
    public class Content
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(50)]
        [MinLength(5)]
        [Required]
        public string Name { get; set; }
        public string FileType { get; set; }
        public int SectionId { get; set; }
        [ForeignKey(nameof(SectionId))]
        public Section Section { get; set; }

        public ICollection <Progress> Progresses { get; set; }
    }
}
