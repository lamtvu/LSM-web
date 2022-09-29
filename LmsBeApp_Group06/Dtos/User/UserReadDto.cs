using System;
using System.ComponentModel.DataAnnotations;
using LmsBeApp_Group06.Models;

namespace LmsBeApp_Group06.Dtos
{
    public class UserReadDto
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public bool IsLock { get; set; }

        public string FullName { get; set; }

        public string Image { get; set; }

        public string Gender { get; set; }

        public string Phone { get; set; }

        public int RoleId { get; set; }

        public bool Verify { get; set; }
        public Role Role { get; set; }

        public DateTime CreateDate { get; set; }
    }
}