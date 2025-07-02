using Discord.Interactions;

namespace JuscraftBot.Commands
{
  public class GeneralCommands : InteractionModuleBase<SocketInteractionContext>
  {
    [SlashCommand("ping", "Checks if the bot is online")]
    public async Task Ping()
    {
      await RespondAsync("Pong!", ephemeral: true);
    }
  }
}