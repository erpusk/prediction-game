using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Data.Repos;
using BackEnd.Models.Classes;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PredictionGamesController(PredictionGamesRepo repo) : ControllerBase()
    {
        private readonly PredictionGamesRepo repo = repo;

        [HttpGet]
        public async Task<IActionResult> GetAll(){
            var result = await repo.GetAllPredictionGames();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPredictionGame(int id){
            var predictionGame = await repo.GetPredictionGameById(id);
            if(predictionGame == null) {
                return NotFound();
            }
            return Ok(predictionGame);
        }

        [HttpPost]
        public async Task<IActionResult> SavePredictionGame([FromBody] PredictionGame predictionGame){
            var predictionGameExists = await repo.PredictionGameExistsInDb(predictionGame.Id);
            if(predictionGameExists) { 
                return Conflict();
            }
            var result = await repo.SavePredictionGameToDb(predictionGame);
            return CreatedAtAction(nameof(GetPredictionGame), new { id = predictionGame.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePredictionGame(int id, [FromBody] PredictionGame predictionGame){
            bool result = await repo.UpdatePredictionGame(id, predictionGame);
            return result ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePredictionGame(int id) {
            bool result = await repo.DeletePredictionGameById(id);
            return result ? NoContent() : NotFound();
        }
    }
}