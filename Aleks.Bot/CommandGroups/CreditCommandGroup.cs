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
                Description = "This bot was made by Alexis Chân Gridel. " +
                              "It is licensed under the GNU General Public License v3.0. ",
            },
            ct: CancellationToken);
    }
}