using System.ComponentModel;
using Remora.Commands.Attributes;
using Remora.Commands.Groups;
using Remora.Discord.API.Objects;
using Remora.Discord.Commands.Feedback.Services;
using Remora.Results;

namespace Aleks.Bot.CommandGroups;

/// <summary>
///     A command group for the credit command.
/// </summary>
public class CreditCommandGroup(FeedbackService feedbackService) : CommandGroup
{
    /// <summary>
    ///     Shows all the contributors to the bot.
    /// </summary>
    /// <returns>A <see cref="Result" /> representing the result of the command.</returns>
    [Command("credit")]
    [Description("Shows all the contributors to the bot.")]
    public async Task<Result> CreditCommandAync()
    {
        return (Result)await feedbackService.SendContextualEmbedAsync(
            new Embed
            {
                Title = "Credits Test",
                Description = "This bot was made by Alexis Chân Gridel. \n" +
                              "It is licensed under the [GNU General Public License v3.0](https://github.com/TalentHubProject/aleks-bot/blob/main/LICENSE). \n" +
                              "All bot's illustrations were made by Skies (aka_skies) \n" +
                              "The bot's is only intended for the use of the server [Talent Hub](https://discord.talent-hub.fr).",
            },
            ct: CancellationToken);
    }
}