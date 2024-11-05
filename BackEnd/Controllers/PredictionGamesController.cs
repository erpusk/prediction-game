using BackEnd.Data.Repos;
using BackEnd.DTOs.PredictionGame;
using BackEnd.DTOs.PredictionGameDTO;
using BackEnd.Mappers;
using itb2203_2024_predictiongame.Backend.Data.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using RandomString4Net;

namespace itb2203_2024_predictiongame.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PredictionGamesController(PredictionGamesRepo repo) : ControllerBase()
    {
        private readonly PredictionGamesRepo repo = repo;

            [HttpGet]
            public async Task<IActionResult> GetOnlyRelatedGames() {

                var userIdClaim = User.FindFirst("userId")?.Value;
                if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId)) {
                    return Unauthorized("User id not found");
                }

                var result = await repo.GetPredictionGamesRelatedWithUser(userId);
                var resultAsDto = result.Select(s => s.ToPredictionGameDto(userId)).ToList();
                return Ok(resultAsDto);
            }

        //[HttpGet]
        //public async Task<IActionResult> GetAll()
        //{
        //    var result = await repo.GetAllPredictionGames();
        //    var resultAsDto = result.Select(s => s.ToPredictionGameDto()).ToList();
        //    return Ok(resultAsDto);
        //}

        [HttpGet("{predictionGameId:int}")]
        public async Task<IActionResult> GetPredictionGame(int predictionGameId)
        {
            var predictionGame = await repo.GetPredictionGameById(predictionGameId);
            if (predictionGame == null)
            {
                return NotFound();
            }
            var userIdClaim = User.FindFirst("userId")?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId)) {
                return Unauthorized("User id is not correct");
            }
            return Ok(predictionGame.ToPredictionGameDto(userId));
        }

        [HttpPost]
        public async Task<IActionResult> CreatePredictionGame([FromBody] CreatePredictionGameRequestDto predictionGameDto)
        {
            var userIdClaim = User.FindFirst("userId")?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId)) {
                return Unauthorized("User id is not correct");
            }
            var gameCreator = await repo.GetUserById(userId);
            if (gameCreator == null)
            {
                return Conflict();
            }

            string uniqueCode;
            do {
                uniqueCode = RandomString.GetString(Types.ALPHABET_LOWERCASE, 6);
            } while (await repo.PredictionGameExistsWithCode(uniqueCode));

            var predictionGameModel = predictionGameDto.ToPredictionGameFromCreateDTO(gameCreator, uniqueCode);

            var predictionGameExists = await repo.PredictionGameExistsInDb(predictionGameModel.Id);
            if (predictionGameExists)
            {
                return Conflict();
            }

            var result = await repo.CreatePredictionGameToDb(predictionGameModel);

            return CreatedAtAction(nameof(GetPredictionGame), new { predictionGameId = predictionGameModel.Id }, result.ToPredictionGameDto(userId));
        }

        [HttpPut("{predictionGameId:int}")]
        [Authorize(Policy = "UserIsGameCreator")]
        public async Task<IActionResult> UpdatePredictionGame(int predictionGameId, [FromBody] UpdatePredictionGameDto predictionGameDto)
        {
            var predictionGameModel = await repo.GetPredictionGameById(predictionGameId);
            if (predictionGameModel == null) {
                return NotFound();
            }
            
            predictionGameModel.PredictionGameTitle = predictionGameDto.PredictionGameTitle;
            predictionGameModel.StartDate = predictionGameDto.StartDate;
            predictionGameModel.EndDate = predictionGameDto.EndDate;
            predictionGameModel.Privacy = predictionGameDto.Privacy;
            predictionGameModel.Events = predictionGameDto.Events?.Select(e => e.ToEvent()).ToList();
            predictionGameModel.UniqueCode = predictionGameDto.UniqueCode;
            
            bool result = await repo.UpdatePredictionGame(predictionGameId, predictionGameModel);
            return result ? NoContent() : NotFound();
        }

        [HttpDelete("{predictionGameId:int}")]
        [Authorize(Policy = "UserIsGameCreator")]
        public async Task<IActionResult> DeletePredictionGame(int predictionGameId)
        {
            bool result = await repo.DeletePredictionGameById(predictionGameId);
            return result ? NoContent() : NotFound();
        }
        [HttpPost("{uniqueCode}/join")]
        public async Task<IActionResult> JoinGame(string uniqueCode, [FromBody] JoinGameRequestDto request)
        {
            int requestUserId = request.UserId;
            var game = await repo.GetPredictionGameByCode(uniqueCode);
            if (game == null)
            {
                return NotFound("Game not found.");
            }
            int gameId = game.Id;
            var user = await repo.GetUserById(requestUserId);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            var isAlreadyParticipant = await repo.IsUserAlreadyInGame(requestUserId, gameId);
            if (isAlreadyParticipant)
            {
                return BadRequest("User is already a participant in this game.");
            }
            await repo.JoinGameAsync(user, gameId);

            return Ok("Successfully joined the game.");
        }

    }
}