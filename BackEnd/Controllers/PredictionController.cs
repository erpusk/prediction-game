using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Data.Repos;
using BackEnd.DTOs.Prediction;
using BackEnd.Mappers;
using BackEnd.Models.Classes;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
            var predictionModel = predictioDto.toPredictionFromCreateDto();
            bool predictionExists = await _repo.GetById(predictionModel.Id) != null;
            if (predictionExists){
                return Conflict();
            }
            var result = await _repo.CreatePrediction(predictionModel);
            return CreatedAtAction(nameof(GetById), new {id = predictionModel.Id}, result.toPredictionDto());
        }
        
    }
}