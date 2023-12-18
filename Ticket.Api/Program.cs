using Microsoft.Extensions.Caching.Memory;
using Polly;
using Polly.Extensions.Http;
using Ticket.Api.Configuration;
using Ticket.Api.Providers;
using Ticket.Api.Services;

namespace Ticket.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddSwaggerGen();
            builder.Services.AddMemoryCache();
            var centralSystemConfiguration = builder.Configuration
                .GetSection(CentralSystemConfiguration.ConfigurationSection)
                .Get<CentralSystemConfiguration>();

            AsyncPolicy<HttpResponseMessage> retryPolicy = centralSystemConfiguration switch
            {
                _ when centralSystemConfiguration.RetryCount.HasValue
                && centralSystemConfiguration.RetrySleepTimeInMilliseconds.HasValue => HttpPolicyExtensions
                                                                                       .HandleTransientHttpError()
                                                                                       .WaitAndRetryAsync(centralSystemConfiguration.RetryCount.Value,
                                                                                                         y => TimeSpan.FromMicroseconds(centralSystemConfiguration.RetrySleepTimeInMilliseconds.Value)),
                _ when centralSystemConfiguration.RetryCount.HasValue => HttpPolicyExtensions
                                                                         .HandleTransientHttpError()
                                                                         .RetryAsync(centralSystemConfiguration.RetryCount.Value),
                _ => Policy.NoOpAsync<HttpResponseMessage>()
            };

            builder.Services
                .AddHttpClient<StationNameProvider>(x => 
                {
                    x.BaseAddress = new Uri(centralSystemConfiguration.Url);
                })
                .AddPolicyHandler(retryPolicy);

            builder.Services.AddSingleton<IStationNameProvider>(
                x => centralSystemConfiguration.CacheEnabled
                    ? new StationNameProviderWithCacheDecorator(x.GetRequiredService<StationNameProvider>(), x.GetRequiredService<IMemoryCache>())
                    : x.GetRequiredService<StationNameProvider>());

            builder.Services.AddSingleton<IStationService, StationService>();

            var app = builder.Build();

            //warmup data
            if (centralSystemConfiguration.CacheEnabled)
            {
                var cachedStationNameProvider = app.Services.GetRequiredService<IStationNameProvider>();
                try
                {
                    cachedStationNameProvider.GetStationNames().GetAwaiter();
                }
                catch(Exception ex)
                {
                    //we can retry, log problem or halt run all together
                }
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
