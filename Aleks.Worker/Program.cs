using Aleks.Bot;
using Aleks.Data;
using Aleks.Worker;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
                       throw new InvalidOperationException("Discord:Token is not configured.");
            })
            .AddHostedService<Worker>()
            .AddDbContext<AleksDbContext>(options =>
            {
                options
                    .UseMySql(
                        hostContext.Configuration["ConnectionStrings:DefaultConnection"],
                        ServerVersion.AutoDetect(hostContext.Configuration["ConnectionStrings:DefaultConnection"]));
            })
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

using var scope = host.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<AleksDbContext>();
await dbContext.Database.MigrateAsync();

await host.RunAsync();