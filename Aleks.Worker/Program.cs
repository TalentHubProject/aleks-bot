// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using Aleks.Bot;
using Aleks.Data;
using Aleks.Plugins.AdvertisementGuard;
using Aleks.Plugins.AutoRoles;
using Aleks.Plugins.Experience;
using Aleks.Plugins.PersonalVocal;
using Aleks.Plugins.Welcomer;
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
            .AddDiscordBot()
            .Configure<CacheSettings>(cacheSettings =>
            {
                cacheSettings.SetSlidingExpiration<IVoiceStateUpdate>(null);
                cacheSettings.SetAbsoluteExpiration<IVoiceStateUpdate>(null);
            })
            .AddDiscordCaching()
            .AddMediatR(AppDomain.CurrentDomain.GetAssemblies())
            .AddExperiencePlugin()
            .AddWelcomerPlugin()
            .AddAdvertisementGuardPlugin()
            .AddAutoRolesPlugin()
            .AddPersonalVocalPlugin()
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