using BackEnd.Data.Repos;
using BackEnd.DTOs.Event;
using BackEnd.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [ApiController]
    [Route("api/PredictionGames/{predictionGameId}/[controller]")]
    [Authorize]
    public class EventController: ControllerBase
    {
        private readonly EventRepo _repo;

        public EventController(EventRepo repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int predictionGameId){
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
        [Authorize(Policy = "UserIsGameCreator")]
        public async Task<IActionResult> AddEvent([FromBody] CreateEventRequestDto eventDto, int predictionGameId){
            var eventModel = eventDto.ToEventFromCreateDTO(predictionGameId);

            var eventExists = await _repo.GetById(eventModel.Id);
            if (eventExists != null){
                return Conflict();
            }
            var result = await _repo.AddEvent(eventModel);
            return CreatedAtAction(nameof(GetEventById), new {predictionGameId = predictionGameId, id = eventModel.Id}, eventModel.ToEventDto());
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "UserIsGameCreator")]
        public async Task<IActionResult> UpdateEvent(int predictionGameId, int id, [FromBody] UpdateEventRequestDto eventDto){
            var eventModel = await _repo.GetById(id);
            
            if (eventModel == null) {
                return NotFound();
            }
            if (eventDto.EventDate > DateTime.UtcNow) {
                return BadRequest("Cannot add scores until the event has passed");
            }  

            eventModel.TeamA = eventDto.TeamA;
            eventModel.TeamB = eventDto.TeamB;
            eventModel.PredictionGameId = predictionGameId;
            eventModel.EventDate = eventDto.EventDate.ToUniversalTime();
            eventModel.TeamAScore = eventDto.TeamAScore;
            eventModel.TeamBScore = eventDto.TeamBScore;
            eventModel.IsCompleted = eventDto.IsCompleted;

            var result = await _repo.UpdateEvent(id, eventModel);
            return result ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "UserIsGameCreator")]
        public async Task<IActionResult> DeleteEvent([FromRoute]int id){
            var result = await _repo.DeleteEvent(id);
            return result ? NoContent() : NotFound();
        }

        
    }
}