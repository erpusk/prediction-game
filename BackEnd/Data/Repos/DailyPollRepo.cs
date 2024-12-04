using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Models.Classes;
using itb2203_2024_predictiongame.Backend.Data;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Data.Repos
{
    public class DailyPollRepo(DataContext context) {
        private readonly DataContext _context = context;

        public async Task AddVoteAsync(PollVote vote)
        {
            _context.PollVotes.Add(vote);
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetTotalVotesAsync(int matchId)
        {
            return await _context.PollVotes
                .Where(v => v.DailyPollMatchId == matchId)
                .CountAsync();
        }

        public async Task<int> GetHomeTeamVotesAsync(int matchId)
        {
            return await _context.PollVotes
                .Where(v => v.DailyPollMatchId == matchId && v.IsForHomeTeam)
                .CountAsync();
        }

        public async Task AddMatchToDb(DailyPollMatch match)
        {
            await _context.DailyPollMatches.AddAsync(match);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMatch(DailyPollMatch match)
        {
            _context.DailyPollMatches.Update(match);
            await _context.SaveChangesAsync();
        }

        public async Task DeactivateAllMatchesAsync()
        {
            var activeMatches = await _context.DailyPollMatches
                .Where(m => m.IsActiveMatch)
                .ToListAsync();

            foreach (var match in activeMatches)
            {
                match.IsActiveMatch = false;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<DailyPollMatch?> GetActiveMatchFromDb() => await _context.DailyPollMatches.FirstOrDefaultAsync(m => m.IsActiveMatch);

        public async Task<DailyPollMatch?> GetLatestActiveMatch() => await _context.DailyPollMatches
            .Where(m => m.IsActiveMatch == true)
            .OrderByDescending(m => m.UtcDate)
            .FirstOrDefaultAsync();

        public async Task<DailyPollMatch?> GetMatchById(int matchId) => await _context.DailyPollMatches.FirstOrDefaultAsync(m => m.Id == matchId);

        public async Task<PollVote?> HasUserVotedTodayAsync(int userId, int matchId)
        {
            var vote = await _context.PollVotes
                .FirstOrDefaultAsync(v => v.UserId == userId && v.DailyPollMatchId == matchId && v.VoteCreatedAt.Date == DateTime.UtcNow.Date);

            return vote != null ? vote : null;
        }
    }
}