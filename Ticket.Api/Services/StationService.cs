using Ticket.Api.Providers;

namespace Ticket.Api.Services
{
    public class StationService : IStationService
    {
        private readonly IStationNameProvider _stationNameProvider;

        public StationService(IStationNameProvider stationNameProvider)
        {
            _stationNameProvider = stationNameProvider;
        }

        public async Task<SearchResult> SearchStation(string searchText)
        {
            var stations = await _stationNameProvider.GetStationNames();
            var nextCharList = new List<char>();
            var foundStations = new List<string>();

            foreach (var station in stations)
            {
                if (station.StationName.StartsWith(searchText, StringComparison.InvariantCultureIgnoreCase))
                {
                    foundStations.Add(station.StationName);
                    if (station.StationName.Length > searchText.Length)
                    {
                        nextCharList.Add(station.StationName[searchText.Length]);
                    }
                }
            }

            return new SearchResult
            {
                NextCharacters = nextCharList.Distinct(),
                StationNames = foundStations
            };
        }
    }
}
