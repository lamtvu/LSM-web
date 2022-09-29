using LmsBeApp_Group06.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LmsBeApp_Group06.Dtos
{
    public class RequestTeacherReadDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string CompanyName { get; set; }
        public DateTime CreateDate { get; set; }
        public int UserId { get; set; }
        public UserReadDto User { get; set; }
    }
}
