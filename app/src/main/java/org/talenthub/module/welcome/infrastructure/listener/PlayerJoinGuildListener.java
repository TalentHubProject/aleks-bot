package org.talenthub.module.welcome.infrastructure.listener;

import lombok.AllArgsConstructor;
import net.dv8tion.jda.api.entities.channel.concrete.TextChannel;
import net.dv8tion.jda.api.events.guild.member.GuildMemberJoinEvent;
import net.dv8tion.jda.api.hooks.ListenerAdapter;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.stereotype.Component;
import org.talenthub.service.ConfigService;

import java.util.Objects;

@Component
@AllArgsConstructor
public class PlayerJoinGuildListener extends ListenerAdapter {

    private final ConfigService configService;

    private final Logger LOGGER = LoggerFactory.getLogger(PlayerJoinGuildListener.class);

    @Override
    public void onGuildMemberJoin(final GuildMemberJoinEvent event) {

        // Get the welcome channel in the config
        TextChannel welcomeChannel = event.getGuild().getTextChannelById(configService.getString("welcome-channel-id"));

        // If the channel is not found, use the default channel of the guild
        Objects.requireNonNullElse(welcomeChannel, event.getGuild().getDefaultChannel());

        try {

            // Send the welcome message
            Objects.requireNonNull(welcomeChannel).sendMessage("""
                    *%s, vient de rejoindre le serveur ! Répondez à ce message pour lui souhaiter la bienvenue et gagner de l'expérience.*
                    Bienvenue sur %s, %s ! ? Empowering Creators, Uniting Passions, Inspriring Growth ? ; construisons ensemble des projets ambitieux, main dans la main.
                    """
                    .formatted(
                            event.getMember().getEffectiveName(),
                            event.getGuild().getName(),
                            event.getMember().getAsMention()
                    )
            ).queue();

        } catch (NullPointerException e) {
            // Log if the welcome channel is not found
            LOGGER.error("The welcome channel is not found in the guild " + event.getGuild().getName());
        }


    }
}
