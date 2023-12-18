using Microsoft.Extensions.Caching.Memory;
using Ticket.Api.Providers.Dto;

namespace Ticket.Api.Providers
{
    public class StationNameProviderWithCacheDecorator : IStationNameProvider
    {
        public const string CacheKey = "StationNames";
        private readonly IStationNameProvider _actualProvider;
        private readonly IMemoryCache _memoryCache;

        public StationNameProviderWithCacheDecorator(
            IStationNameProvider actualProvider, 
            IMemoryCache memoryCache)
        {
            if (actualProvider.GetType() == GetType())
            {
                throw new ArgumentException("StationNameProviderWithCacheDecorator can not decorate itself");
            }

            _actualProvider = actualProvider;
            _memoryCache = memoryCache;
        }

        public async Task<IEnumerable<StationDto>> GetStationNames()
        {
            if(_memoryCache.TryGetValue(CacheKey, out IEnumerable<StationDto>? value))
            {
                return value ?? Enumerable.Empty<StationDto>();
            }

            var stationNames = await _actualProvider.GetStationNames();
            if(stationNames == null || !stationNames.Any())
            {
                return Enumerable.Empty<StationDto>();
            }

            _memoryCache.Set(CacheKey, stationNames, TimeSpan.FromDays(1));
            return stationNames;
        }
    }
}
