using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Data.Repos;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    public class GameParticipantController : ControllerBase
    {
        private readonly GameParticipantRepo gameParticipantRepo;

        public GameParticipantController(GameParticipantRepo repo) {
            gameParticipantRepo = repo;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGameParticipantPoints(int id, [FromQuery] int points, int pgId ){
            
            var result = await gameParticipantRepo.UpdateGameParticipantPoints(id, pgId, points);

            return result ? NoContent() : NotFound();
        }
    }
}