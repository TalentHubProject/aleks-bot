package org.talenthub.module.xp.listener;

import lombok.AllArgsConstructor;
import net.dv8tion.jda.api.events.session.ReadyEvent;
import net.dv8tion.jda.api.hooks.ListenerAdapter;
import org.springframework.stereotype.Component;
import org.talenthub.module.xp.service.LevelService;

@Component
@AllArgsConstructor
public class ReadyListener extends ListenerAdapter {

    private final LevelService levelService;

    @Override
    public void onReady(final ReadyEvent event) {

        levelService.checkAndGenerateFirsLevel();

    }
}
