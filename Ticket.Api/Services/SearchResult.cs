namespace Ticket.Api.Services
{
    public readonly record struct SearchResult
    {
        public IEnumerable<string> StationNames { get; init; }
        public IEnumerable<char> NextCharacters { get; init; }
    }
}
