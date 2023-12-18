using Ticket.Api.Providers.Dto;

namespace Ticket.Api.Providers
{
    public interface IStationNameProvider
    {
        Task<IEnumerable<StationDto>> GetStationNames();
    }
}