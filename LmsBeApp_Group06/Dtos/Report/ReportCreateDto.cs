using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LmsBeApp_Group06.Dtos.Report
{
    public class ReportCreateDto
    {
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }
    }
}
