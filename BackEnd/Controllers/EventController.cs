using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Data.Repos;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventController: ControllerBase
    {
        private readonly EventRepo _repo;

        public EventController(EventRepo repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(){
            var matches = await _repo.GetAll();
            return Ok(matches);
        }

        [HttpGet("Id")]
        public async Task<IActionResult> GetEventhById(int Id){
            var match = await _repo.GetById(Id);
            if (match == null){
                return NotFound();
            }
            return Ok(match);
        }

        
    }
}