using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.DTOs.ApplicationUser
{
    public class UpdateApplicationUserDto
    {
        public string? UserName { get; set; }
        public DateTime DateOfBirth { get; set; } 
        public IFormFile? ProfilePicture { get; set; }
    }
}