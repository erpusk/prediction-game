using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Data.Repos;
using BackEnd.Mappers;
using BackEnd.Models.Classes;
using Newtonsoft.Json;

namespace BackEnd.Service
{
    public class DailyPollMatchFetcherService : BackgroundService
    {
        private readonly HttpClient _httpClient;
        private readonly IServiceProvider _serviceProvider;
        public DailyPollMatchFetcherService(HttpClient httpClient, IServiceProvider serviceProvider)
        {
            _httpClient = httpClient;
            _serviceProvider = serviceProvider;

            _httpClient.Timeout = TimeSpan.FromSeconds(60);

        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var targetTime = DateTime.UtcNow.Date.AddDays(1).AddHours(3);
            var initialDelay = targetTime - DateTime.UtcNow;

            try
            {
                using var scope = _serviceProvider.CreateScope();
                var dailyPollRepo = scope.ServiceProvider.GetRequiredService<DailyPollRepo>();

                await FetchAndStoreMatchData(dailyPollRepo);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during initial data fetch: " + ex);
            }

            await Task.Delay(initialDelay, stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var dailyPollRepo = scope.ServiceProvider.GetRequiredService<DailyPollRepo>();

                    var activeMatch = await FetchAndStoreMatchData(dailyPollRepo);

                    var now = DateTime.UtcNow;
                    targetTime = now.Date.AddDays(1).AddHours(3);
                    var delay = targetTime - now;

                    await Task.Delay(delay, stoppingToken);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        private async Task<DailyPollMatch?> FetchAndStoreMatchData(DailyPollRepo dailyPollRepo) {
            var tomorrow = DateTime.UtcNow.AddDays(1).ToString("yyyy-MM-dd");
            var dayAfterTomorrow = DateTime.UtcNow.AddDays(2).ToString("yyyy-MM-dd");
            var twoDaysAfterTomorrow = DateTime.UtcNow.AddDays(3).ToString("yyyy-MM-dd");

            var rawData = await GetDataFromApi("https://api.football-data.org/v4/matches");
            if (!rawData.Matches.Any())
            {
                rawData = await GetDataFromApi($"https://api.football-data.org/v4/matches?dateFrom={tomorrow}&dateTo={dayAfterTomorrow}");
                if (!rawData.Matches.Any())
                {
                    rawData = await GetDataFromApi($"https://api.football-data.org/v4/matches?dateFrom={dayAfterTomorrow}&dateTo={twoDaysAfterTomorrow}");
                    if (rawData == null || !rawData.Matches.Any())
                    {
                        Console.WriteLine("No matches found for the next three days.");
                        return null;
                    }
                }
            }

            var competitionRankings = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
            {
                { "UEFA Champions League", 1 },
                { "FIFA World Cup", 2 },
                { "European Championship", 3 },
                { "Premier League", 4 },
                { "Bundesliga", 5 },
                { "Primera Division", 6 },
                { "Serie A", 7 },
                { "Ligue 1", 8 },
                { "Copa Libertadores", 9 },
                { "Eredivisie", 10 },
                { "Championship", 11 },
                { "Primeira Liga", 12},
                { "Campeonato Brasileiro Série A", 13},
            };

            var leagueMatches = rawData.Matches
                .Where(match => match.Competition != null && match.Competition.Name != null)
                .GroupBy(match =>
                {
                    string competitionName = match.Competition.Name;
                    return competitionRankings.TryGetValue(competitionName, out int rank) ? rank : int.MaxValue;
                })
                .OrderBy(group => group.Key)
                .FirstOrDefault();

            if (leagueMatches == null) {
                return null;
            }

            var latestMatch = leagueMatches
                .OrderByDescending(match => match.UtcDate)
                .FirstOrDefault();

            if (latestMatch == null) {
                return null;
            }

            await dailyPollRepo.DeactivateAllMatchesAsync();

            var existingMatch = await dailyPollRepo.GetMatchById(latestMatch.Id);
            if (existingMatch != null)
            {
                existingMatch.IsActiveMatch = true;
                await dailyPollRepo.UpdateMatch(existingMatch);
                return existingMatch;
            }

            var activeMatch = latestMatch.ToDailyPollMatch();
            activeMatch.IsActiveMatch = true;

            await dailyPollRepo.AddMatchToDb(activeMatch);
            return activeMatch;
        }

        public async Task<RootDto> GetDataFromApi(string url)
        {
            var token = "7637b3a5b3064ec999ac2671b115d7a7";
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("X-Auth-Token", token);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<RootDto>(jsonString)!;
        }
    }
}