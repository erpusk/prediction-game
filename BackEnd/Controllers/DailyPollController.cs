using BackEnd.Data.Repos;
using BackEnd.DTOs.PollVote;
using BackEnd.Models.Classes;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DailyPollController : ControllerBase
    {
        private readonly DailyPollRepo _dailyPollRepo;
        public DailyPollController(DailyPollRepo dailyPollRepo) {
            _dailyPollRepo = dailyPollRepo;
        }

        [HttpGet("daily-poll-match")]
        public async Task<IActionResult> GetActiveDailyPollMatch()
        {
            var activeMatch = await _dailyPollRepo.GetActiveMatchAsync();

            if (activeMatch == null)
            {
                return NotFound("No active poll match found.");
            }

            return Ok(activeMatch);
        }

        [HttpPost("vote")]
        public async Task<IActionResult> SubmitVote([FromBody] PollVoteRequest pollVoteRequest)
        {
            if (pollVoteRequest.DailyPollMatchId <= 0) {
                return BadRequest("Invalid vote data.");
            }

            var vote = new PollVote {
                DailyPollMatchId = pollVoteRequest.DailyPollMatchId,
                IsForHomeTeam = pollVoteRequest.IsForHomeTeam
            };

            await _dailyPollRepo.AddVoteAsync(vote);

            return Ok("Vote submitted successfully.");
        }

        [HttpGet("results/{matchId}")]
        public async Task<IActionResult> GetVoteResults(int matchId)
        {
            var totalVotes = await _dailyPollRepo.GetTotalVotesAsync(matchId);

            if (totalVotes == 0) {
                return NotFound("No votes found for this match.");
            }

            var homeVotes = await _dailyPollRepo.GetHomeTeamVotesAsync(matchId);
            var awayVotes = totalVotes - homeVotes;

            var results = new {
                TotalVotes = totalVotes,
                HomeTeamVotes = homeVotes,
                HomeTeamPercentage = (double)homeVotes / totalVotes * 100,
                AwayTeamVotes = awayVotes,
                AwayTeamPercentage = (double)awayVotes / totalVotes * 100
            };

            return Ok(results);
        }
    }
}