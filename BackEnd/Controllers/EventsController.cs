using BackEnd.Data.Repos;
using BackEnd.Models.Classes;
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEventById([FromRoute]int Id){
            var events = await _repo.GetById(Id);
            if (events == null){
                return NotFound();
            }
            return Ok(events);
        }

        [HttpPost]
        public async Task<IActionResult> AddEvent([FromBody] Event eventModel){
            var eventExists = await _repo.GetById(eventModel.Id);
            if (eventExists != null){
                return Conflict();
            }
            var result = await _repo.AddEvent(eventModel);
            return CreatedAtAction(nameof(GetEventById), new {id = eventModel.Id}, eventModel);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(int id, [FromBody] Event eventModel){
            var result = await _repo.UpdateEvent(id, eventModel);
            if (result == null){
                return NotFound();
            }

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent([FromRoute]int id){
            var result = await _repo.DeleteEvent(id);
            return result ? NoContent() : NotFound();
        }

        
    }
}