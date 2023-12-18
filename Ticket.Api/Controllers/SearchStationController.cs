using Microsoft.AspNetCore.Mvc;
using Ticket.Api.Services;

namespace Ticket.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SearchStationController : ControllerBase
    {
        private readonly IStationService _stationService;

        public SearchStationController(IStationService stationService)
        {
            _stationService = stationService;
        }

        [HttpGet]
        [Route("searchStation")]
        public async Task<SearchResultResponse> SearchStation(string searchText)
        {
            var searchResult = await _stationService.SearchStation(searchText);
            return new SearchResultResponse
            {
                NextCharacters = searchResult.NextCharacters,
                StationNames = searchResult.StationNames
            };
        }
    }
}
