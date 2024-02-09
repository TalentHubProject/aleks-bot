package org.talenthub.module.xp.dba.command;

import fr.leonarddoo.dba.annotation.Command;
import fr.leonarddoo.dba.element.DBACommand;
import net.dv8tion.jda.api.events.interaction.command.SlashCommandInteractionEvent;
import org.springframework.stereotype.Component;

@Command(name = "experience_profil", description = "Command to get the experience profil of a user")
@Component
public class ExperienceProfilCmd implements DBACommand {

    @Override
    public void execute(final SlashCommandInteractionEvent event) {
        //TODO
    }
}
