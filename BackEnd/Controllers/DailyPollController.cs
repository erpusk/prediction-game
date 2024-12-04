using BackEnd.Data.Repos;
using BackEnd.DTOs.PollVote;
using BackEnd.Models.Classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DailyPollController : ControllerBase
    {
        private readonly DailyPollRepo _dailyPollRepo;
        public DailyPollController(DailyPollRepo dailyPollRepo) {
            _dailyPollRepo = dailyPollRepo;
        }

        [HttpGet("daily-poll-match")]
        public async Task<IActionResult> GetActiveDailyPollMatch()
        {
            var activeMatch = await _dailyPollRepo.GetActiveMatchFromDb();

            if (activeMatch == null)
            {
                return NotFound("No active poll match found.");
            }

            return Ok(activeMatch);
        }

        [HttpPost("vote")]
        public async Task<IActionResult> SubmitVote([FromBody] PollVoteRequest pollVoteRequest)
        {
            var userIdClaim = User.FindFirst("userId")?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId)) {
                return Unauthorized("User id is not correct");
            }

            if (pollVoteRequest.DailyPollMatchId <= 0) {
                return BadRequest("Invalid vote data.");
            }

            var hasVoted = await _dailyPollRepo.HasUserVotedTodayAsync(userId, pollVoteRequest.DailyPollMatchId);
            if (hasVoted != null) {
                return BadRequest("User has already voted for this match today.");
            }

            var vote = new PollVote {
                DailyPollMatchId = pollVoteRequest.DailyPollMatchId,
                IsForHomeTeam = pollVoteRequest.IsForHomeTeam,
                UserId = userId,
                VoteCreatedAt = DateTime.UtcNow
            };

            await _dailyPollRepo.AddVoteAsync(vote);
            return Ok("Vote submitted successfully.");
        }

        [HttpGet("{matchId:int}/has-voted")]
        public async Task<IActionResult> HasUserVotedToday(int matchId)
        {
            var userIdClaim = User.FindFirst("userId")?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId)) {
                return Unauthorized("User id is not correct");
            }

            var result = await _dailyPollRepo.HasUserVotedTodayAsync(userId, matchId);
            return Ok(new { votedForHomeTeam = result?.IsForHomeTeam });
        }

        [HttpGet("{matchId:int}/results")]
        public async Task<IActionResult> GetVoteResults(int matchId)
        {
            var totalVotes = await _dailyPollRepo.GetTotalVotesAsync(matchId);

            if (totalVotes == 0) {
                return Ok(new {
                    TotalVotes = totalVotes,
                    HomeTeamVotes = 0,
                    AwayTeamVotes = 0,
                    HomeTeamPercentage = 0,
                    AwayTeamPercentage = 0
                });
    }

            var homeVotes = await _dailyPollRepo.GetHomeTeamVotesAsync(matchId);
            var awayVotes = totalVotes - homeVotes;

            var homeTeamPercentage = Math.Round((double)homeVotes / totalVotes * 100, 1);
            var awayTeamPercentage = Math.Round((double)awayVotes / totalVotes * 100, 1);

            if (homeTeamPercentage + awayTeamPercentage != 100) {
                awayTeamPercentage = 100 - homeTeamPercentage;
            }

            var results = new {
                TotalVotes = totalVotes,
                HomeTeamVotes = homeVotes,
                HomeTeamPercentage = homeTeamPercentage,
                AwayTeamVotes = awayVotes,
                AwayTeamPercentage = awayTeamPercentage
            };

            return Ok(results);
        }
    }
}