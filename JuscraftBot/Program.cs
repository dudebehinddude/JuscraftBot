using System.Reflection;
using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using DotNetEnv;
using Serilog;
using Serilog.Events;

namespace JuscraftBot
{
  public class Program
  {
    private static DiscordSocketClient? _client;
    private static InteractionService? _interactionService;

    public static async Task Main()
    {
      Log.Logger = new LoggerConfiguration()
                  .MinimumLevel.Verbose()
                  .Enrich.FromLogContext()
                  .WriteTo.Console()
                  .CreateLogger();

      _client = new DiscordSocketClient();

      _client.Log += LogAsync;

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
        Log.Error("Error: TOKEN environment variable not set. Please create a .env file.");
        return;
      }
      if (string.IsNullOrEmpty(serverID))
      {
        Log.Error("Error: GUILD_ID environment variable not set. Please create a .env file.");
        return;
      }
      if (!ulong.TryParse(serverID, out ulong guildId) || guildId == 0)
      {
        Log.Error("Error: GUILD_ID environment variable cannot be parsed as a ulong.");
        return;
      }

      // For slash commands
      _interactionService = new InteractionService(_client);
      _interactionService.Log += LogAsync;
      _interactionService.SlashCommandExecuted += SlashCommandExecuted;
      await _interactionService.AddModulesAsync(Assembly.GetEntryAssembly(), null); // add command modules automatically

      // Setup
      _client.Log += LogAsync;
      _client.Ready += async () =>
      {
        Log.Information("Bot is online!");

        try
        {
          await _interactionService!.RegisterCommandsToGuildAsync(guildId, true);
          Log.Information($"Registered commands to Guild ID: {guildId}");
        }
        catch (Exception ex)
        {
          Log.Warning($"Error registering guild commands: {ex.Message}");
        }
      };
      _client.InteractionCreated += HandleInteractionAsync;

      await _client.LoginAsync(TokenType.Bot, discordToken);
      await _client.StartAsync();
      await Task.Delay(-1);
    }

    private static async Task LogAsync(LogMessage message)
    {
      var severity = message.Severity switch
      {
        LogSeverity.Critical => LogEventLevel.Fatal,
        LogSeverity.Error => LogEventLevel.Error,
        LogSeverity.Warning => LogEventLevel.Warning,
        LogSeverity.Info => LogEventLevel.Information,
        LogSeverity.Verbose => LogEventLevel.Verbose,
        LogSeverity.Debug => LogEventLevel.Debug,
        _ => LogEventLevel.Information
      };
      Log.Write(severity, message.Exception, "[{Source}] {Message}", message.Source, message.Message);
      await Task.CompletedTask;
    }

    private static async Task HandleInteractionAsync(SocketInteraction interaction)
    {
      var context = new SocketInteractionContext(_client, interaction);
      var result = await _interactionService!.ExecuteCommandAsync(context, null);

      if (!result.IsSuccess)
      {
        Log.Warning($"Command execution failed: {result.ErrorReason}");
        if (!interaction.HasResponded)
        {
          await interaction.RespondAsync($"Error: {result.ErrorReason}", ephemeral: true);
        }
      }

    }

    private static async Task SlashCommandExecuted(SlashCommandInfo commandInfo, IInteractionContext ctx, Discord.Interactions.IResult result)
    {
      if (!result.IsSuccess)
      {
        var response = result.Error switch
        {
          InteractionCommandError.UnmetPrecondition => $"{result.ErrorReason}",
          InteractionCommandError.UnknownCommand => "Unknown command",
          InteractionCommandError.BadArgs => "Invalid number or arguments",
          InteractionCommandError.Exception => $"Command exception: {result.ErrorReason}",
          InteractionCommandError.Unsuccessful => "Command could not be executed",
          _ => "An unknown error occurred"
        };
        await ctx.Interaction.RespondAsync(response, ephemeral: true);
      }
    }
  }
}