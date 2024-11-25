mod command_trait;
pub use command_trait::SlashCommand;

pub mod test_command;

use serenity::model::application::Command;
use serenity::prelude::Context;
use serenity::model::id::GuildId;
use std::error::Error;

pub async fn register_commands(ctx: &Context, guild_id: Option<GuildId>) -> Result<(), Box<dyn Error>> {
    let commands = vec![
        test_command::TestCommand::register(),
    ];

    match guild_id {
        Some(guild_id) => {
            guild_id.set_commands(&ctx.http, commands).await?;
            println!("Registered guild commands");
        }
        None => {
            Command::set_global_commands(&ctx.http, commands).await?;
            println!("Registered global commands");
        }
    }

    Ok(())
}