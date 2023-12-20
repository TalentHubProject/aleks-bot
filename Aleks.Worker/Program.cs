using Aleks.Bot;
using Aleks.Data;
using Aleks.Worker;
using MediatR;
using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Caching.Extensions;
using Remora.Discord.Caching.Services;
using Remora.Discord.Hosting.Extensions;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services
            .AddDiscordService(discordService =>
            {
                var configuration = discordService.GetRequiredService<IConfiguration>();

                return configuration.GetValue<string?>("BotToken") ??
                       throw new InvalidOperationException("BotToken is not configured.");
            })
            .AddHostedService<Worker>()
            .AddSqlServer<AleksDbContext>("aleksDb")
            .AddMediatR(AppDomain.CurrentDomain.GetAssemblies())
            .AddDiscordBot()
            .Configure<CacheSettings>(cacheSettings =>
            {
                cacheSettings.SetSlidingExpiration<IVoiceStateUpdate>(null);
                cacheSettings.SetAbsoluteExpiration<IVoiceStateUpdate>(null);
            })
            .AddDiscordCaching()
            ;
    })
    .ConfigureLogging(
        c => c
            .AddConsole()
            .AddFilter("System.Net.Http.HttpClient.*", LogLevel.Warning))
    .Build();

await host.RunAsync();