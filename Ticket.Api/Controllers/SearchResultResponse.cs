namespace Ticket.Api.Controllers
{
    public readonly record struct SearchResultResponse
    {
        public IEnumerable<string> StationNames { get; init; }
        public IEnumerable<char> NextCharacters { get; init; }
    }
}
