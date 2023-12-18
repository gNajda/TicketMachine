namespace Ticket.Api.Providers.Dto
{
    public readonly record struct StationDto
    {
        public string StationCode { get; init; }
        public string StationName { get; init; }
    }
}
