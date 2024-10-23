using BackEnd.Data.Repos;
using BackEnd.DTOs.PredictionGame;
using BackEnd.Mappers;
using itb2203_2024_predictiongame.Backend.Data.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace itb2203_2024_predictiongame.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PredictionGamesController(PredictionGamesRepo repo) : ControllerBase()
    {
        private readonly PredictionGamesRepo repo = repo;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await repo.GetAllPredictionGames();
            var resultAsDto = result.Select(s => s.ToPredictionGameDto()).ToList();
            return Ok(resultAsDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPredictionGame(int id)
        {
            var predictionGame = await repo.GetPredictionGameById(id);
            if (predictionGame == null)
            {
                return NotFound();
            }
            return Ok(predictionGame.ToPredictionGameDto());
        }

        [HttpPost]
        public async Task<IActionResult> CreatePredictionGame([FromBody] CreatePredictionGameRequestDto predictionGameDto, [FromHeader(Name = "UserId")] int userId)
        {
            var gameCreator = await repo.GetUserById(userId);
            if (gameCreator == null)
            {
                return Conflict();
            }

            var predictionGameModel = predictionGameDto.ToPredictionGameFromCreateDTO(gameCreator);

            var predictionGameExists = await repo.PredictionGameExistsInDb(predictionGameModel.Id);
            if (predictionGameExists)
            {
                return Conflict();
            }



            var result = await repo.CreatePredictionGameToDb(predictionGameModel);
            return CreatedAtAction(nameof(GetPredictionGame), new { id = predictionGameModel.Id }, result.ToPredictionGameDto());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePredictionGame(int id, [FromBody] UpdatePredictionGameDto predictionGameDto)
        {
            var predictionGameModel = await repo.GetPredictionGameById(id);
            if (predictionGameModel == null) {
                return NotFound();
            }
            
            predictionGameModel.PredictionGameTitle = predictionGameDto.PredictionGameTitle;
            predictionGameModel.StartDate = predictionGameDto.StartDate;
            predictionGameModel.EndDate = predictionGameDto.EndDate;
            predictionGameModel.Privacy = predictionGameDto.Privacy;
            predictionGameModel.Events = predictionGameDto.Events?.Select(e => e.ToEvent()).ToList();
            predictionGameModel.UniqueCode = predictionGameDto.UniqueCode;
            // predictionGameModel.GameCreator = updateDto.GameCreator != null
            //                     ? ApplicationUserMappers.ToApplicationUserFromDto(updateDto.GameCreator)
            //                     : null;
            
            bool result = await repo.UpdatePredictionGame(id, predictionGameModel);
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