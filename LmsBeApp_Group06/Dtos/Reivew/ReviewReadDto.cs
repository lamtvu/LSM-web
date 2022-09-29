using LmsBeApp_Group06.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LmsBeApp_Group06.Dtos.Reivew
{
    public class ReviewReadDto
    {
        public int Id { get; set; }
        public int Star { get; set; }
        public string Comment { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreateDate { get; set; }

        public int CourseId { get; set; }

        public int SenderId { get; set; }
        public UserReadDto Sender { get; set; }

    }
}
