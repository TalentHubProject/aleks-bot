// // Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// // Licensed under the GNU General Public License v3.0.
// // See the LICENSE file in the project root for more information.

using Remora.Rest.Core;

namespace Aleks.Plugins.WelcomerFeedbackExperience.Services;

public sealed record WelcomerFeedbackUser
    (Snowflake Instigator, IList<Snowflake> Responders, DateTime UserJoinedAt)
{
    public bool CanBeWelcomed =>
        !Responders.Contains(Instigator) && DateTime.UtcNow - UserJoinedAt < TimeSpan.FromMinutes(5);
}