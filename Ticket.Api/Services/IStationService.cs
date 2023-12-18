
namespace Ticket.Api.Services
{
    public interface IStationService
    {
        Task<SearchResult> SearchStation(string searchText);
    }
}