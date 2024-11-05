using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BackEnd.Data.Repos;
using BackEnd.DTOs.Prediction;
using BackEnd.Mappers;
using BackEnd.Models.Classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PredictionController: ControllerBase
    {
        private readonly PredictionRepo _repo;
        public PredictionController(PredictionRepo repo){
            _repo = repo;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll ([FromQuery]int? eventId){
            var result = await _repo.GetAll(eventId);
            var resultAsDto = result.Select(x => x.toPredictionDto()).ToList();
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id){
            var result = await _repo.GetById(id);

            if (result == null) {
                return NotFound();
            }

            var resultAsDto = result.toPredictionDto();
            return Ok(resultAsDto);
        }
        [HttpPost]
        public async Task<IActionResult> CreatePrediction([FromBody] CreatePredictionRequestDto predictioDto){
            var userIdClaim = User.FindFirst("userId")?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId)) {
                return Unauthorized("User ID claim not found.");
            }

            var predictionModel = predictioDto.toPredictionFromCreateDto();
            predictionModel.PredictionMakerId = userId;

            bool predictionExists = (await _repo.GetAll(predictionModel.EventId, userId)).Any();
            if (predictionExists){
                return Conflict("User has already made a prediction for this event");
            }

            var eventWherePredictionIsAdded = await _repo.GetPredictionsEvent(predictionModel);
            if (eventWherePredictionIsAdded is null){
                return BadRequest("Event where to add the prediction was not found");
            }

            if (eventWherePredictionIsAdded.IsCompleted == true){
                return Conflict("Event has already ended");
            }

            var result = await _repo.CreatePrediction(predictionModel);
            return CreatedAtAction(nameof(GetById), new {id = predictionModel.Id}, result.toPredictionDto());
        }
    }
}