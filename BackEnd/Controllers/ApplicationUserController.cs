using BackEnd.Data.Repos;
using BackEnd.DTOs.ApplicationUser;
using BackEnd.Mappers;
using BackEnd.Models.Classes;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUserController(ApplicationUserRepo repo) : ControllerBase()
    {
        private readonly ApplicationUserRepo repo = repo;
        
        [HttpGet]  // Võiks olla tulevikus ainult kindla mängu kasutajate kuvamiseks.
        public async Task<IActionResult> GetAll(){
            var result = await repo.GetAllUsers();
            var usersAsDto = result.Select(e => e.ToApplicationUserDto()).ToList();
            return Ok(usersAsDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetApplicationUser(int id){  //Kasutaja profiili kuvamiseks.
            var result = await repo.GetUserById(id);
            if (result == null){
                return NotFound();
            }
            return Ok(result.ToApplicationUserDto());
        }
        [HttpPost]
        public async Task<IActionResult> SaveApplicationUser([FromBody]CreateApplicationUserRequestDto userDto){
            var applicationUserModel = userDto.ToApplicationUserFromCreateDto();


            var userExists = await repo.IsUserExistsInDB(applicationUserModel.Id);
            if (userExists) {
                return Conflict();
            }

            var result = await repo.SaveApplicationUserToDb(applicationUserModel);
            return CreatedAtAction(nameof(GetApplicationUser), new { id = applicationUserModel.Id }, result.ToApplicationUserDto());
        }

        [HttpPut("{id}")] //Kasutaja andmete uuendamine, kontstantseid andmeid nagu sünnipäev ei muudaks.
        public async Task<IActionResult> UpdateApplicationUser(int id, [FromBody] UpdateApplicationUserDto userDto){ 
            var userModel = await repo.GetUserById(id);

            if (userModel == null){
                return NotFound();
            }

            userModel.UserName = userDto.UserName;
            userModel.DateOfBirth = userDto.DateOfBirth;

            var result = await repo.UpdateApplicationUser(id, userModel);
            return result ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")] // Kasutaja kustutamine, kas on vaja?
        public async Task<IActionResult> DeleteApplicationUser(int id){
            var result = await repo.DeleteUserById(id);
            return result ? NoContent() : NotFound();
        }
    }
}