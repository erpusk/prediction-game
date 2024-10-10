using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.DTOs.ApplicationUser;
using BackEnd.Models.Classes;

namespace BackEnd.Mappers
{
    public static class ApplicationUserMappers
    {
        public static ApplicationUserDto ToApplicationUserDto(this ApplicationUser applicationUser) {
            return new ApplicationUserDto {
                Id = applicationUser.Id,
                Username = applicationUser.UserName
            };
        }
        public static ApplicationUser ToApplicationUserFromDto(this ApplicationUserDto applicationUserDto) {
            return new ApplicationUser {
                Id = applicationUserDto.Id,
                UserName = applicationUserDto.Username
            };
        }
    }
}