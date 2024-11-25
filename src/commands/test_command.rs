use serenity::builder::{CreateCommand, CreateInteractionResponseMessage};
use serenity::model::application::{CommandInteraction, ResolvedOption};
use serenity::prelude::Context;
use serenity::{async_trait, Result as SerenityResult};

use super::SlashCommand;

pub struct TestCommand;

#[async_trait]
impl SlashCommand for TestCommand {
    fn register() -> CreateCommand {
        CreateCommand::new(Self::name())
            .description(Self::description())
    }

    async fn run<'a>(
        _ctx: &Context,
        _command: &CommandInteraction,
        _options: &[ResolvedOption<'a>],
    ) -> SerenityResult<CreateInteractionResponseMessage> {
        Ok(CreateInteractionResponseMessage::new().content("Test command executed!"))
    }

    fn name() -> &'static str {
        "test"
    }

    fn description() -> &'static str {
        "A test command"
    }
}