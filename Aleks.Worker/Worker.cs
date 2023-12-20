using System.Text.Json;
using Remora.Discord.Commands.Services;
using Remora.Rest.Core;

namespace Aleks.Worker;

public class Worker(
    ILogger<Worker> logger,
    SlashService slashService,
    IConfiguration configuration)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

        await InitializeSlashCommands(stoppingToken);
    }

    private async Task InitializeSlashCommands(CancellationToken stoppingToken)
    {
        var guildId = configuration["GuildId"]
                      ?? throw new ArgumentNullException("GuildId is not configured.");

        var guildIdParsed = ulong.Parse(guildId);

        logger.LogInformation("Guild ID: {GuildId}", guildIdParsed);

        var development = configuration["DOTNET_ENVIRONMENT"] == "Development";

        logger.LogInformation("Your bot is running in {Environment} mode.", development
            ? "Development"
            : "Production");

        var updateSlash = development switch
        {
            true => await slashService.UpdateSlashCommandsAsync(new Snowflake(guildIdParsed), ct: stoppingToken),
            false => await slashService.UpdateSlashCommandsAsync(ct: stoppingToken)
        };

        if (!updateSlash.IsSuccess)
        {
            logger.LogWarning("Failed to update slash commands: {Error}", JsonSerializer.Serialize(updateSlash));
        }
    }
}