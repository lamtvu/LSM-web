using LmsBeApp_Group06.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LmsBeApp_Group06.Dtos
{
    public class ReportReadDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public UserReadDto Sender { get; set; }
        public int ClassId { get; set; }
    }
}
