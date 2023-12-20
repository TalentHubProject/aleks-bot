using MediatR;
using Microsoft.EntityFrameworkCore;
using Aleks.Data;
using Aleks.Plugins.Welcomer.Commands;

namespace Aleks.Plugins.Welcomer.Handlers.Commands;

public class UpdateWelcomeMessageHandler
    : AsyncRequestHandler<UpdateWelcomeMessageCommand>
{
    private readonly AleksDbContext _dbContext;

    public UpdateWelcomeMessageHandler(AleksDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    protected override async Task Handle(UpdateWelcomeMessageCommand request, CancellationToken cancellationToken)
    {
        var guild = await _dbContext.WelcomerGuilds.FirstOrDefaultAsync(t => t.GuildId == request.GuildId);

        if (guild != null)
        {
            guild.WelcomeMessage = request.newWelcomeMessage;
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}