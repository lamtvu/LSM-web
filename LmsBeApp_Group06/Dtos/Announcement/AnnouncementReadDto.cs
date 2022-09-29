using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LmsBeApp_Group06.Dtos.Announcement
{
    public class AnnouncementReadDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreateDate { get; set; }
        public string Type { get; set; }
        public int ClassId { get; set; }
    }
}
