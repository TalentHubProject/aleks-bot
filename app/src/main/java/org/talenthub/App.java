package org.talenthub;

import net.dv8tion.jda.api.JDABuilder;
import net.dv8tion.jda.api.entities.Activity;

import javax.security.auth.login.LoginException;
import java.util.Objects;

public class App {

    public static void main(String[] args) {

        try {
            Objects.requireNonNull(args[0], "You must provide a token as the first argument.");

            JDABuilder.createDefault(args[0])
                    .setActivity(Activity.competing("discord.talent-hub.fr"))
                    .build();

            System.out.println("Bot is now running.");
        } catch (LoginException e) {
            System.out.println("Error during the login process, please check your token.");
            System.exit(1);
        }
    }
}
