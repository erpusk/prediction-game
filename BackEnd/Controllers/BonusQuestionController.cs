using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Data.Repos;
using BackEnd.Models.Classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;

namespace BackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    [EnableCors("MyPolicy")]
    public class BonusQuestionController(BonusQuestionRepo repo) : ControllerBase()
    {
        private readonly BonusQuestionRepo _repo = repo;

        [HttpGet]
        public async Task<IActionResult> GetbonusQuestions(){
            var result = await _repo.GetBonusQuestions();
            return Ok(result);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetBonusQuestion(int Id){
            var result = await _repo.GetBonusQuestion(Id);
            if (result == null){
                return NotFound("BonusQuestion was not found");
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> PostPredictionGameBonusQuestion([FromBody]BonusQuestion bonusQuestionModel){
            var bonusQuestionExists = await _repo.GetBonusQuestion(bonusQuestionModel.Id);
            if (bonusQuestionExists != null){
                return BadRequest("Question already exists");
            }
            var result = await _repo.AddBonusQuestion(bonusQuestionModel);
            return CreatedAtAction(nameof(GetBonusQuestion), new {Id = bonusQuestionModel.Id}, bonusQuestionModel);
        }

        [HttpGet("predictionGame/{predictionGameId}")]
        public async Task<IActionResult> GetPredictionGamesBonusQuestions(int predictionGameId){
            var result = await _repo.GetPredictionGamesQuestions(predictionGameId);
            if (result == null){
                return NotFound("BonusQuestion was not found");
            }
            return Ok(result);
        }
    }
}