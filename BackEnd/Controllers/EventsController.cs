using BackEnd.Data.Repos;
using BackEnd.DTOs.Event;
using BackEnd.Mappers;
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
        public async Task<IActionResult> GetAll([FromQuery] int? predictionGameId){
            var events = await _repo.GetAll(predictionGameId);
            var eventsAsDto = events.Select(e => e.ToEventDto()).ToList();
            return Ok(eventsAsDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEventById([FromRoute]int id){
            var events = await _repo.GetById(id);
            if (events == null){
                return NotFound();
            }
            return Ok(events.ToEventDto());
        }

        [HttpPost]
        public async Task<IActionResult> AddEvent([FromBody] CreateEventRequestDto eventDto){
            var eventModel = eventDto.ToEventFromCreateDTO();

            var eventExists = await _repo.GetById(eventModel.Id);
            if (eventExists != null){
                return Conflict();
            }
            var result = await _repo.AddEvent(eventModel);
            return CreatedAtAction(nameof(GetEventById), new {id = eventModel.Id}, eventModel.ToEventDto());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(int id, [FromBody] UpdateEventRequestDto eventDto){
            var eventModel = await _repo.GetById(id);
            
            if (eventModel == null) {
                return NotFound();
            }
            if (eventDto.EventDate > DateTime.UtcNow) {
                return BadRequest("Cannot add scores until the event has passed");
            }  

            eventModel.TeamA = eventDto.TeamA;
            eventModel.TeamB = eventDto.TeamB;
            eventModel.PredictionGameId = eventDto.PredictionGameId;
            eventModel.EventDate = eventDto.EventDate;
            eventModel.TeamAScore = eventDto.TeamAScore;
            eventModel.TeamBScore = eventDto.TeamBScore;
            eventModel.IsCompleted = eventDto.IsCompleted;

            var result = await _repo.UpdateEvent(id, eventModel);
            return result ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent([FromRoute]int id){
            var result = await _repo.DeleteEvent(id);
            return result ? NoContent() : NotFound();
        }

        
    }
}