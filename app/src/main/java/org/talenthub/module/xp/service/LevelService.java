package org.talenthub.module.xp.service;

import lombok.AllArgsConstructor;
import net.dv8tion.jda.api.entities.Member;
import net.dv8tion.jda.api.entities.channel.middleman.GuildMessageChannel;
import org.springframework.stereotype.Service;
import org.talenthub.module.xp.persistence.Level;
import org.talenthub.module.xp.repository.LevelRepository;
import org.talenthub.persistence.DiscordUser;
import org.talenthub.service.ConfigService;
import org.talenthub.service.DiscordUserService;
import org.talenthub.service.MessageBroadcasterService;

@Service
@AllArgsConstructor
public class LevelService {

    private final DiscordUserService discordUserService;
    private final LevelRepository levelRepository;
    private final MessageBroadcasterService messageBroadcasterService;
    private final ConfigService configService;

    public void addXp(final GuildMessageChannel channel, final Member member, final long xp){

        DiscordUser discordUser = discordUserService.getDiscordUser(member.getIdLong());

        discordUser.setXp(discordUser.getXp() + xp);

        if(discordUser.getLevel().getMaxXp() <= discordUser.getXp()){

            Level newLevel = levelUp(discordUser);

            messageBroadcasterService.broadcastBasicMessageEmbed(channel, "Félécitation **" + member.getEffectiveName() + "**! Tu as atteint le niveau " + newLevel.getId() + "!");

        }else{
            discordUserService.updateDiscordUser(discordUser);
        }

    }

    private Level levelUp(final DiscordUser discordUser){

        Level newlevel = levelRepository.findNextLevelByMaxXp(discordUser.getXp()).orElse(createNewlevel(discordUser.getLevel()));
        discordUser.setLevel(newlevel);

        discordUserService.updateDiscordUser(discordUser);

        return newlevel;
    }

    public void checkAndGenerateFirsLevel() {

        if (levelRepository.count() == 0) {

            int maxXp = configService.getInt("first-level-xp");
            Level level = new Level(1, maxXp);

            levelRepository.save(level);

        }
    }

    private Level createNewlevel(Level level){
        Level newLevel = new Level(level.getId() + 1, (long) (level.getMaxXp() * 1.2));
        levelRepository.save(newLevel);
        return newLevel;
    }



}
