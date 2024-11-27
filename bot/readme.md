![Bot avatar](https://cdn.discordapp.com/embed/avatars/3.png&fit=cover&mask=circle)

# bot

> Made with [bot.ts](https://ghom.gitbook.io/bot-ts/) by **ergazia**  
> CLI version: `9.0.3`  
> Bot.ts version: `v8.0.0-Capi`  
> Licence: `ISC`

## Description

  
This bot is private and cannot be invited in other servers.

## Specifications

You can find the documentation of bot.ts [here](https://ghom.gitbook.io/bot-ts/).  
Below you will find the specifications for **bot**.

## Configuration file

```ts
import { Config } from "#core/config"
import { Options, Partials } from "discord.js"
import { z } from "zod"

export const config = new Config({
  ignoreBots: true,
  openSource: true,
  envSchema: z.object({}),
  permissions: ["Administrator"],
  client: {
    intents: [
      "Guilds",
      "GuildMessages",
      "GuildMessageReactions",
      "GuildMessageTyping",
      "DirectMessages",
      "DirectMessageReactions",
      "DirectMessageTyping",
      "MessageContent",
    ],
    partials: [Partials.Channel],
    makeCache: Options.cacheWithLimits({
      ...Options.DefaultMakeCacheSettings,

      // don't cache reactions
      ReactionManager: 0,
    }),
    sweepers: {
      ...Options.DefaultSweeperSettings,
      messages: {
        // every hour (in second)
        interval: 60 * 60,

        // 6 hours
        lifetime: 60 * 60 * 6,
      },
    },
  },
})

export default config.options

```

## Cron jobs

> No cron jobs have been created yet.

## Commands

### Slash commands

- [/ping](src/slash/ping.native.ts) - Get the bot ping  
- [/help](src/slash/help.native.ts) - Show slash command details or list all slash commands

### Textual commands

- [eval](src/commands/eval.native.ts) - JS code evaluator  
- [info](src/commands/info.native.ts) - Get information about bot  
- [turn](src/commands/turn.native.ts) - Turn on/off command handling  
- [terminal](src/commands/terminal.native.ts) - Run shell command from Discord  
- [database](src/commands/database.native.ts) - Run SQL query on database  
- [help](src/commands/help.native.ts) - Help menu

## Buttons

- [pagination](src/buttons/pagination.native.ts) - The pagination button

## Listeners

### Cron  

- [ready](src/listeners/cron.ready.native.ts) - Launch all cron jobs  

### Button  

- [interactionCreate](src/listeners/button.interactionCreate.native.ts) - Handle the interactions for buttons  

### Slash  

- [interactionCreate](src/listeners/slash.interactionCreate.native.ts) - Handle the interactions for slash commands  
- [guildCreate](src/listeners/slash.guildCreate.native.ts) - Deploy the slash commands to the new guild  
- [ready](src/listeners/slash.ready.native.ts) - Deploy the slash commands  

### Command  

- [messageCreate](src/listeners/command.messageCreate.native.ts) - Handle the messages for commands  

### Pagination  

- [messageReactionAdd](src/listeners/pagination.messageReactionAdd.native.ts) - Handle the reactions for pagination  
- [messageDelete](src/listeners/pagination.messageDelete.native.ts) - Remove existing deleted paginator  

### Log  

- [afterReady](src/listeners/log.afterReady.native.ts) - Just log that bot is ready

## Database

Using **pg@latest** as database.  
Below you will find a list of all the tables used by **bot**.

> No tables have been created yet.

## Information

This readme.md is dynamic, it will update itself with the latest information.  
If you see a mistake, please report it and an update will be made as soon as possible.

- Used by: **10** Discord guilds
- Last update date: **11/27/2024**
