using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.DTOs.AccountDTO;
using BackEnd.DTOs.LoginDTO;
using BackEnd.Models.Classes;
using itb2203_2024_predictiongame.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtTokenService _tokenService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AccountController(UserManager<ApplicationUser> userManager, JwtTokenService tokenService, SignInManager<ApplicationUser> signInManager) {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto) {
            try {
                if (!ModelState.IsValid) 
                    return BadRequest(ModelState);
                
                var appUser = new ApplicationUser {
                    UserName = registerDto.Username,
                    Email = registerDto.Email,
                };

                var createdUser = await _userManager.CreateAsync(appUser, registerDto.Password!);

                if (createdUser.Succeeded) {
                    var roleResult = await _userManager.AddToRoleAsync(appUser, "User");

                    if (roleResult.Succeeded) {
                        return Ok(
                            new NewUserDto {
                                Username = appUser.UserName,
                                Email = appUser.Email,
                                Token = _tokenService.GenerateToken(appUser)
                            }
                        );
                    } else {
                        return BadRequest("Could not create a user");
                    }
                } else {
                    return BadRequest("User creating failed");
                }
            } catch (Exception e) {
                return StatusCode(500, e);
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == loginDto.Email.ToLower());

            if (user == null) return Unauthorized("Invalid email!");

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded) return Unauthorized("Email and/or password incorrect");
            
            return Ok(
                new NewUserDto {
                    Username = user.UserName,
                    Email = user.Email,
                    Token = _tokenService.GenerateToken(user)
                }
            );
        }
    }
}