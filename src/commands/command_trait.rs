use serenity::builder::{CreateCommand, CreateInteractionResponseMessage};
use serenity::model::application::ResolvedOption;
use serenity::prelude::Context;
use serenity::model::application::CommandInteraction;
use serenity::{async_trait, Result as SerenityResult};

#[async_trait]
pub trait SlashCommand: Send + Sync {
    fn register() -> CreateCommand where Self: Sized;
    
    async fn run<'a>(
        ctx: &Context,
        command: &CommandInteraction,
        options: &[ResolvedOption<'a>],
    ) -> SerenityResult<CreateInteractionResponseMessage>;

    fn name() -> &'static str;
    fn description() -> &'static str;
}
