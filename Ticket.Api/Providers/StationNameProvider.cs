using Newtonsoft.Json;
using Ticket.Api.Providers.Dto;

namespace Ticket.Api.Providers
{
    public class StationNameProvider : IStationNameProvider
    {
        private readonly string _endpoint = "coding-challenge/master/station_codes.json";
        private readonly HttpClient _httpClient;

        public StationNameProvider(
            HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<StationDto>> GetStationNames()
        {
            var httpResponse = await _httpClient.GetAsync(_endpoint);
            httpResponse.EnsureSuccessStatusCode();
            var jsonContent = await httpResponse.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<StationDto>>(jsonContent) ?? Enumerable.Empty<StationDto>();
        }
    }
}
