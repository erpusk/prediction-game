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
        public static ApplicationUserDto ToApplicationUserDto(this ApplicationUser applicationUserModel)
        {
            return new ApplicationUserDto
            {
                Id = applicationUserModel.Id,
                UserName = applicationUserModel.UserName
            };
        }
        public static ApplicationUser ToApplicationUserFromDto(this ApplicationUserDto applicationUserDto)
        {
            return new ApplicationUser
            {
                Id = applicationUserDto.Id,
                UserName = applicationUserDto.UserName
            };
        }
        public static ApplicationUser ToApplicationUserFromCreateDto(this CreateApplicationUserRequestDto applicationUserDto)
        {
            return new ApplicationUser
            {
                UserName = applicationUserDto.UserName,
                DateOfBirth = applicationUserDto.DateOfBirth,
            };
        }
        public static ApplicationUser ToApplicationUserFromUpdateDto(this UpdateApplicationUserDto applicationUserDto)
        {
            return new ApplicationUser
            {
                UserName = applicationUserDto.UserName,
                DateOfBirth = applicationUserDto.DateOfBirth,
            };
        }
    }
}