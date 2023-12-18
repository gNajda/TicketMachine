namespace Ticket.Api.Configuration
{
    public record struct CentralSystemConfiguration
    {
        public const string ConfigurationSection = "CentralSystem";
        public string Url { get; set; }
        public int? RetryCount { get; set; }
        public int? RetrySleepTimeInMilliseconds { get; set; }
        public bool CacheEnabled { get; set; }
    }
}
