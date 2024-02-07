package org.talenthub.module.xp.infrastructure.listener;

import lombok.AllArgsConstructor;
import net.dv8tion.jda.api.entities.channel.ChannelType;
import net.dv8tion.jda.api.events.guild.voice.GuildVoiceUpdateEvent;
import net.dv8tion.jda.api.hooks.ListenerAdapter;
import org.springframework.stereotype.Component;
import org.talenthub.module.xp.infrastructure.service.LevelService;

import java.time.LocalDateTime;
import java.time.temporal.ChronoUnit;
import java.util.HashMap;
import java.util.Map;

@Component
@AllArgsConstructor
public class VoicePresenceListener extends ListenerAdapter {

    private final LevelService levelService;

    private final Map<Long, LocalDateTime> MEMBER_IN_VOICE = new HashMap<>();

    @Override
    public void onGuildVoiceUpdate(final GuildVoiceUpdateEvent event) {

        if(! (event.getChannelLeft().getType().equals(ChannelType.VOICE) || event.getChannelJoined().getType().equals(ChannelType.VOICE)) ) return;
        if(event.getMember().getUser().isBot()) return;
        if(event.getVoiceState().isMuted()) return;
        if(event.getVoiceState().isDeafened()) return;

        if(event.getChannelJoined() == null) {

            MEMBER_IN_VOICE.put(event.getMember().getIdLong(), LocalDateTime.now());

        } else if (event.getChannelLeft() == null) {

            LocalDateTime joined = MEMBER_IN_VOICE.get(event.getMember().getIdLong());
            LocalDateTime left = LocalDateTime.now();

            long minutes = joined.until(left, ChronoUnit.MINUTES);

            levelService.addXp(event.getChannelLeft().asVoiceChannel(), event.getEntity(), minutes);

        }

    }
}
