using System.Reflection;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using DotNetEnv;

namespace JuscraftBot
{
  public class Program
  {
    private static DiscordSocketClient? _client;
    private static InteractionService? _interactionService;

    public static async Task Main()
    {
      var config = new DiscordSocketConfig
      {
        GatewayIntents = GatewayIntents.All | GatewayIntents.MessageContent
      };
      _client = new DiscordSocketClient(config);

      Env.Load();

      string? discordToken = Environment.GetEnvironmentVariable("TOKEN");
      string? serverID = Environment.GetEnvironmentVariable("GUILD_ID");

      if (string.IsNullOrEmpty(discordToken))
      {
        Console.WriteLine("Error: TOKEN environment variable not set. Please create a .env file.");
        return;
      }
      if (string.IsNullOrEmpty(serverID))
      {
        Console.WriteLine("Error: GUILD_ID environment variable not set. Please create a .env file.");
        return;
      }
      if (!ulong.TryParse(serverID, out ulong guildId) || guildId == 0)
      {
        Console.WriteLine("Error: GUILD_ID environment variable cannot be parsed as a ulong.");
        return;
      }

      // For slash commands
      _interactionService = new InteractionService(_client.Rest);
      _interactionService.Log += Log;
      await _interactionService.AddModulesAsync(Assembly.GetEntryAssembly(), null); // add command modules automatically

      // Setup
      _client.Log += Log;
      _client.Ready += async () =>
      {
        Console.WriteLine("Bot is online!");

        try
        {
          await _interactionService!.RegisterCommandsToGuildAsync(guildId, true);
          Console.WriteLine($"Registered commands to Guild ID: {guildId}");
        }
        catch (Exception ex)
        {
          Console.WriteLine($"Error registering guild commands: {ex.Message}");
        }
      };
      _client.InteractionCreated += HandleInteractionAsync;

      await _client.LoginAsync(TokenType.Bot, discordToken);
      await _client.StartAsync();
      await Task.Delay(-1);
    }

    private static Task Log(LogMessage message)
    {
      // Simple console logging
      Console.WriteLine($"[{message.Severity}] {message.Source}: {message.Message} {message.Exception}");
      return Task.CompletedTask;
    }

    private static async Task HandleInteractionAsync(SocketInteraction interaction)
    {
      try
      {
        var context = new SocketInteractionContext(_client!, interaction);
        var result = await _interactionService!.ExecuteCommandAsync(context, null);

        if (!result.IsSuccess)
        {
          Console.WriteLine($"Command execution failed: {result.ErrorReason}");
          if (!interaction.HasResponded)
          {
            await interaction.RespondAsync($"Error: {result.ErrorReason}", ephemeral: true);
          }
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Error handling interaction: {ex.Message} {ex.StackTrace}");
        if (!interaction.HasResponded)
        {
          await interaction.RespondAsync("An unexpected error occurred!", ephemeral: true);
        }
      }
    }
  }
}