using MediatR;

namespace Aleks.Plugins.Welcomer.Commands;

public record UpdateWelcomeMessageCommand(long GuildId, string newWelcomeMessage) : IRequest;