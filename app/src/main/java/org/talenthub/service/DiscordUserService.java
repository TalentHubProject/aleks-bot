package org.talenthub.service;

import lombok.AllArgsConstructor;
import org.springframework.stereotype.Service;
import org.talenthub.module.xp.persistence.Level;
import org.talenthub.module.xp.repository.LevelRepository;
import org.talenthub.persistence.DiscordUser;
import org.talenthub.repository.DiscordUserRepository;

import java.util.Optional;

@Service
@AllArgsConstructor
public class DiscordUserService {

    private final DiscordUserRepository discordUserRepository;
    private final LevelRepository levelRepository;


    public DiscordUser getDiscordUser(final long discordId){
        return discordUserRepository.findById(discordId).orElse(createDiscordUser(discordId));
    }

    private DiscordUser createDiscordUser(final long discordId){

        Optional<Level> firstLevel = levelRepository.findById(1);

        if(firstLevel.isEmpty()){
            throw new IllegalStateException("No levels found");
        }

        DiscordUser discordUser = new DiscordUser(discordId, firstLevel.get());
        discordUserRepository.save(discordUser);
        return discordUser;
    }

    public void updateDiscordUser(final DiscordUser discordUser){
        discordUserRepository.save(discordUser);
    }
}
