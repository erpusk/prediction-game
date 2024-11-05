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

                var userIdClaim = User.FindFirst("id");
                if (userIdClaim == null) {
                    return Unauthorized("User id not found");
                }

                int userId = int.Parse(userIdClaim.Value);


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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPredictionGame(int id)
        {
            var predictionGame = await repo.GetPredictionGameById(id);
            if (predictionGame == null)
            {
                return NotFound();
            }
            var userIdClaim = User.FindFirst("id")?.Value;
            int? userId = userIdClaim != null ? int.Parse(userIdClaim) : (int?)null;
            return Ok(predictionGame.ToPredictionGameDto(userId));
        }

        [HttpPost]
        public async Task<IActionResult> CreatePredictionGame([FromBody] CreatePredictionGameRequestDto predictionGameDto)
        {
            var userIdClaim = User.FindFirst("id")?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId)) {
                return Unauthorized();
            }
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
            do
            {
                predictionGameModel.UniqueCode = RandomString.GetString(Types.ALPHABET_LOWERCASE, 6);
            } while (await repo.PredictionGameExistsWithCode(predictionGameModel.UniqueCode));

            var result = await repo.CreatePredictionGameToDb(predictionGameModel);

            return CreatedAtAction(nameof(GetPredictionGame), new { id = predictionGameModel.Id }, result.ToPredictionGameDto(userId));
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
            if (predictionGameDto.UniqueCode != null && predictionGameDto.UniqueCode != predictionGameModel.UniqueCode)
            {
                return BadRequest("UniqueCode cannot be modified.");
            }
            bool result = await repo.UpdatePredictionGame(id, predictionGameModel);
            return result ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePredictionGame(int id)
        {
            bool result = await repo.DeletePredictionGameById(id);
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