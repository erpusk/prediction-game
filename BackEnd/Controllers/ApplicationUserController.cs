using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Core.Pipeline;
using BackEnd.Data.Repos;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUserController(ApplicationUserRepo repo) : ControllerBase()
    {
        private readonly ApplicationUserRepo _repo = repo;
        
        [HttpGet]
        public async Task<IActionResult> GetAll(){
            var result = await _repo.GetAllUsers();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetApplicationUser(int id){
            var result = await _repo.GetUserById(id);
            if (result == null){
                return NotFound();
            }
            return Ok(result);
        }
    }
}