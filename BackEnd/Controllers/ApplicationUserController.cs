using BackEnd.Data.Repos;
using BackEnd.Models.Classes;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUserController(ApplicationUserRepo repo) : ControllerBase()
    {
        private readonly ApplicationUserRepo repo = repo;
        
        [HttpGet]
        public async Task<IActionResult> GetAll(){
            var result = await repo.GetAllUsers();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetApplicationUser(int id){
            var result = await repo.GetUserById(id);
            if (result == null){
                return NotFound();
            }
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> SaveApplicationUser([FromBody] ApplicationUser user){
            var userExists = await repo.IsUserExistsInDB(user.Id);

            if (userExists) {
                return Conflict();
            }

            var result = await repo.SaveApplicationUserToDb(user);
            return CreatedAtAction(nameof(GetApplicationUser), new { id = user.Id }, result);
        }

        [HttpPut("{id}")] 
        public async Task<IActionResult> UpdateApplicationUser(int id, [FromBody]ApplicationUser user){
            var result = await repo.UpdateApplicationUser(id, user);
            return result ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApplicationUser(int id){
            var result = await repo.DeleteUserById(id);
            return result ? NoContent() : NotFound();
        }
    }
}