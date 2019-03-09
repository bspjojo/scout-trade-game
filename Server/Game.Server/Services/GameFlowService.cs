using System;
using System.Threading.Tasks;
using Game.Server.Services.Models;
using Game.Server.DataRepositories;

namespace Game.Server.Services
{
    public interface IGameFlowService
    {
        Task<ScoreServiceResult> ExecuteUpdateScoreFlow(string countryId, int year, ConsumptionResources consumptionResourcesRecorded);
    }

    public class GameFlowService : IGameFlowService
    {
        private readonly IGameDataService _gameDataService;
        private readonly IGameHubService _gameHubService;
        private readonly IGameScoreService _gameScoreService;

        public GameFlowService(IGameDataService gameDataService, IGameHubService gameHubService, IGameScoreService gameScoreService)
        {
            _gameDataService = gameDataService;
            _gameHubService = gameHubService;
            _gameScoreService = gameScoreService;
        }

        public async Task<ScoreServiceResult> ExecuteUpdateScoreFlow(string countryId, int year, ConsumptionResources consumptionResourcesRecorded)
        {
            var gameInformation = await _gameDataService.GetGameInformationForACountry(countryId);

            var breakEven = await _gameDataService.GetBreakEvenForACountry(countryId);
            var targetsForCurrentYear = await _gameDataService.GetTargetsForACountryForAYear(countryId, gameInformation.CurrentYear);

            _gameScoreService.CalculateYearValues(breakEven, targetsForCurrentYear, consumptionResourcesRecorded, out var currentYearExcess, out var currentYearScores, out var nextYearTargets);

            // var scores = country.Years[year].Scores;
            // var excess = country.Years[year].Excess;

            //await _gameHubService.ScoresUpdated(gameId, c)

            return new ScoreServiceResult
            {
                NextYearTarget = nextYearTargets,
                Excess = currentYearExcess,
                Scores = currentYearScores
            };
        }
    }
}