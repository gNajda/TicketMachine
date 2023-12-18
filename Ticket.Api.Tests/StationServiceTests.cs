using NSubstitute;
using Ticket.Api.Providers;
using Ticket.Api.Providers.Dto;
using Ticket.Api.Services;

namespace Ticket.Api.Tests
{
    public class StationServiceTests
    {
        private readonly IStationNameProvider _stationNameProviderMock = Substitute.For<IStationNameProvider>();
        private readonly StationService _stationService;

        public StationServiceTests()
        {
            _stationService = new StationService(_stationNameProviderMock);
        }

        [Fact]
        //I Know the naming is not ideal but I wanted to test example scenarios from the task description
        public async Task ExpectationScenarioFromTaskDescription1()
        {
            _stationNameProviderMock
                .GetStationNames()
                .Returns(
                    Task.FromResult<IEnumerable<StationDto>>(
                        [
                            new StationDto { StationName = "DARTFORD" },
                            new StationDto { StationName = "DARTON" },
                            new StationDto { StationName = "TOWER HILL" },
                            new StationDto { StationName = "DERBY" }
                        ]));

            var result = await _stationService.SearchStation("DART");
            var expectedChar = new char[] { 'F', 'O' };
            var expectedStationNames = new string[] { "DARTFORD","DARTON" };
            Assert.Contains(result.NextCharacters, x => expectedChar.Contains(x));
            Assert.Contains(result.StationNames, x => expectedStationNames.Contains(x));
        }

        [Fact]
        public async Task ExpectationScenarioFromTaskDescription2()
        {
            _stationNameProviderMock
                .GetStationNames()
                .Returns(
                    Task.FromResult<IEnumerable<StationDto>>(
                        [
                            new StationDto { StationName = "LIVERPOOL" },
                            new StationDto { StationName = "LIVERPOOL LIME STREET" },
                            new StationDto { StationName = "PADDINGTON" }
                        ]));

            var result = await _stationService.SearchStation("LIVERPOOL");
            var expectedChar = new char[] { ' ' };
            var expectedStationNames = new string[] { "LIVERPOOL", "LIVERPOOL LIME STREET" };
            Assert.Contains(result.NextCharacters, x => expectedChar.Contains(x));
            Assert.Contains(result.StationNames, x => expectedStationNames.Contains(x));
        }

        [Fact]
        public async Task ExpectationScenarioFromTaskDescription3()
        {
            _stationNameProviderMock
                .GetStationNames()
                .Returns(
                    Task.FromResult<IEnumerable<StationDto>>(
                        [
                            new StationDto { StationName = "EUSTON" },
                            new StationDto { StationName = "LONDON BRIDGE" },
                            new StationDto { StationName = "VICTORIA" }
                        ]));

            var result = await _stationService.SearchStation("KINGS CROSS");
            Assert.Empty(result.NextCharacters);
            Assert.Empty(result.StationNames);
        }
    }
}