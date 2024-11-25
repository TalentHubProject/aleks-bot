use std::env;

use serenity::async_trait;
use serenity::builder::CreateInteractionResponse;
use serenity::model::application::Interaction;
use serenity::model::gateway::Ready;
use serenity::model::id::GuildId;
use serenity::prelude::*;

mod commands;
use commands::SlashCommand;

struct Handler;

#[async_trait]
impl EventHandler for Handler {
    async fn interaction_create(&self, ctx: Context, interaction: Interaction) {
        if let Interaction::Command(command) = interaction {
            println!("Received command interaction: {command:#?}");

            match command.data.name.as_str() {
                "test" => {
                    let response = commands::test_command::TestCommand::run(&ctx, &command, &command.data.options()).await;
                    if let Ok(data) = response {
                        let builder = CreateInteractionResponse::Message(data);
                        if let Err(why) = command.create_response(&ctx.http, builder).await {
                            println!("Cannot respond to slash command: {why}");
                        }
                    }
                }
                _ => {
                    println!("Unknown command: {}", command.data.name);
                }
            }
        }
    }

    async fn ready(&self, ctx: Context, ready: Ready) {
        println!("{} is connected!", ready.user.name);

        let guild_id = GuildId::new(
            env::var("GUILD_ID")
                .expect("Expected GUILD_ID in environment")
                .parse()
                .expect("GUILD_ID must be an integer"),
        );

        if let Err(why) = commands::register_commands(&ctx, Some(guild_id)).await {
            println!("Error registering commands: {why}");
        }
    }
}

#[tokio::main]
async fn main() {
    let token = env::var("DISCORD_TOKEN").expect("Expected a token in the environment");

    let mut client = Client::builder(token, GatewayIntents::empty())
        .event_handler(Handler)
        .await
        .expect("Error creating client");

    if let Err(why) = client.start().await {
        println!("Client error: {why:?}");
    }
}
