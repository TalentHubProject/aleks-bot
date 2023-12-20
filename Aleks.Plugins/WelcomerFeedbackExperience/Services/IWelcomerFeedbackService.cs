// // Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// // Licensed under the GNU General Public License v3.0.
// // See the LICENSE file in the project root for more information.

using Remora.Rest.Core;

namespace Aleks.Plugins.WelcomerFeedbackExperience.Services;

public interface IWelcomerFeedbackService
{
    public void AddUser(Snowflake instigator, Snowflake responder, Snowflake guildId, Snowflake channelId,
        DateTime userJoinedAt);

    public void RemoveUser(Snowflake instigator);

    public bool IsWelcomeMessage(Snowflake messageId);

    public void AddWelcomeMessageId(Snowflake messageId);
}