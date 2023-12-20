// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using Remora.Rest.Core;
using Aleks.Data.Domain.PersonalVocal;
using Aleks.Plugins.PersonalVocal.Model;

namespace Aleks.Plugins.PersonalVocal.Services;

public interface IPersonalVocalService
{
    void JoinVoiceChannel(Snowflake userId, Snowflake channelId);
    void LeaveVoiceChannel(Snowflake userId);
    
    Snowflake? GetVoiceChannel(Snowflake userId);
    
    void CreateVoiceChannel(Snowflake userId, Snowflake guildId, Snowflake channelId);
    
    void DeleteVoiceChannel(Snowflake userId, Snowflake guildId);
    
    bool HasVoiceChannel(Snowflake userId, Snowflake guildId);
    
    Tuple<UserVocalChannel, Snowflake>? GetVoiceChannel(Snowflake userId, Snowflake guildId);
}