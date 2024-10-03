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
            var events = await _repo.GetAll();
            return Ok(events);
        }

        [HttpGet("Id")]
        public async Task<IActionResult> GetEventhById(int Id){
            var events = await _repo.GetById(Id);
            if (events == null){
                return NotFound();
            }
            return Ok(events);
        }

        
    }
}