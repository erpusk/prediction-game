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
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var dailyPollRepo = scope.ServiceProvider.GetRequiredService<DailyPollRepo>();

                while (!stoppingToken.IsCancellationRequested)
                {
                    var latestMatch = await dailyPollRepo.GetLatestActiveMatch();
                    var now = DateTime.UtcNow;

                    if (latestMatch == null || latestMatch.UtcDate.Date.AddDays(1).AddHours(3) <= now)
                    {
                        try
                        {
                            await FetchAndStoreMatchData(dailyPollRepo, stoppingToken);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error during data fetch: " + ex);
                        }
                    }

                    try {
                        await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken);
                    } catch (TaskCanceledException) {}
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during service execution: " + ex + "\nStack: " + ex.StackTrace);
            }
        }

        private async Task<DailyPollMatch?> FetchAndStoreMatchData(DailyPollRepo dailyPollRepo, CancellationToken stoppingToken) {
            var tomorrow = DateTime.UtcNow.AddDays(1).ToString("yyyy-MM-dd");
            var dayAfterTomorrow = DateTime.UtcNow.AddDays(2).ToString("yyyy-MM-dd");
            var twoDaysAfterTomorrow = DateTime.UtcNow.AddDays(3).ToString("yyyy-MM-dd");

            var rawData = await GetDataFromApi("https://api.football-data.org/v4/matches", stoppingToken);
            if (rawData?.Matches == null || !rawData.Matches.Any())
            {
                rawData = await GetDataFromApi($"https://api.football-data.org/v4/matches?dateFrom={tomorrow}&dateTo={dayAfterTomorrow}", stoppingToken);
                if (rawData?.Matches == null || !rawData.Matches.Any())
                {
                    rawData = await GetDataFromApi($"https://api.football-data.org/v4/matches?dateFrom={dayAfterTomorrow}&dateTo={twoDaysAfterTomorrow}", stoppingToken);
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

        public async Task<RootDto> GetDataFromApi(string url, CancellationToken cancellationToken)
        {
            var token = Environment.GetEnvironmentVariable("DAILY_POLL_TOKEN");
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("X-Auth-Token", token);

            var response = await _httpClient.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<RootDto>(jsonString)!;
        }
    }
}