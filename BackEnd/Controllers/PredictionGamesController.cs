using itb2203_2024_predictiongame.Backend.Data.Repos;
using itb2203_2024_predictiongame.Backend.Models.Classes;
using Microsoft.AspNetCore.Mvc;

namespace itb2203_2024_predictiongame.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PredictionGamesController(PredictionGamesRepo repo) : ControllerBase()
    {
        private readonly PredictionGamesRepo repo = repo;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await repo.GetAllPredictionGames();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPredictionGame(int id)
        {
            var predictionGame = await repo.GetPredictionGameById(id);
            if (predictionGame == null)
            {
                return NotFound();
            }
            return Ok(predictionGame);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePredictionGame([FromBody] PredictionGame predictionGame, [FromHeader(Name = "X-UserId")] int userId)
        {
            var predictionGameExists = await repo.PredictionGameExistsInDb(predictionGame.Id);
            if (predictionGameExists)
            {
                return Conflict();
            }

            predictionGame.GameCreatorId = userId;

            var result = await repo.CreatePredictionGameToDb(predictionGame);
            return CreatedAtAction(nameof(GetPredictionGame), new { id = predictionGame.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePredictionGame(int id, [FromBody] PredictionGame predictionGame)
        {
            bool result = await repo.UpdatePredictionGame(id, predictionGame);
            return result ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePredictionGame(int id)
        {
            bool result = await repo.DeletePredictionGameById(id);
            return result ? NoContent() : NotFound();
        }
    }
}