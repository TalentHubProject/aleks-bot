// // Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// // Licensed under the GNU General Public License v3.0.
// // See the LICENSE file in the project root for more information.

using Aleks.Plugins.WelcomerFeedbackExperience.Responders;
using Aleks.Plugins.WelcomerFeedbackExperience.Services;
using Microsoft.Extensions.DependencyInjection;
using Remora.Discord.Gateway.Extensions;

namespace Aleks.Plugins.WelcomerFeedbackExperience;

public static class Setup
{
    public static IServiceCollection AddWelcomerFeedbackExperiencePlugin(this IServiceCollection services)
    {
        return services
                .AddSingleton<IWelcomerFeedbackService, WelcomerFeedbackService>()
                .AddResponder<WelcomerFeedbackGuildMemberJoinResponder>()
                .AddResponder<WelcomerFeedbackMessageResponder>()
            ;
    }
}