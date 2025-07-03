using Discord;
using Discord.Interactions;
using Discord.WebSocket;

namespace JuscraftBot.Commands
{
  [Group("docs", "Documentation for server things")]
  public class DocsCommands : InteractionModuleBase<SocketInteractionContext>
  {
    [SlashCommand("join", "Displays instructions on how to join the server")]
    public async Task Join(
      [Summary("ping", "If specified, pings this user along with the message (and makes it visible to everyone).")] SocketUser? pingUser = null)
    {
      var embedBuilder = new EmbedBuilder()
        .WithTitle("How to join Juscraft")
        .AddField("Download the Modpack", "The latest modpack download can be found in <#1378078557108047903>. For further instructions run `/docs modpack-installation` or see https://discord.com/channels/1375856723306352691/1375857288358793308/1376330001540186133")
        .AddField("Get Added to the Whitelist", "To be added to the whitelist, you must link your account to this server. You can get a linking code by attempting to connect to the server, then you can use it here to link your account by running `/link <code>` in any channel. If you have any issues, ping/dm <@458433261958332437> or <@&1376311406714163331>.")
        .AddField("VoiceChat", "VoiceChat is included in the modpack. You will be prompted to set this up by pressing `[F6]` when you first join. If you need to edit any settings you can do so at any time using `[F6]`.")
        .AddField("Inviting Others", "Since whitelist is enabled, players must be in this server to be able to join. If you want to invite others ask first.")
        .AddField("Server IP", "The server IP is `juscraft.dudebehinddude.com`")
        .AddField("** **", "This information is also visible in https://discord.com/channels/1375856723306352691/1375857288358793308/1376330001540186133")
        .WithCurrentTimestamp()
        .WithFooter("You can run this command with '/docs join'");

      if (pingUser != null)
      {
        embedBuilder.WithAuthor(Context.User);
      }

      var embed = embedBuilder.Build();

      if (pingUser == null)
      {
        await RespondAsync(embeds: [embed], ephemeral: true);
      }
      else
      {
        await RespondAsync($"<@{pingUser.Id}>", embeds: [embed]);
      }
    }

    [SlashCommand("modpack-installation", "Displays instructions on how to install the modpack")]

    public async Task ModpackInstallation(
      [Summary("ping", "If specified, pings this user along with the message (and makes it visible to everyone).")] SocketUser? pingUser = null)
    {
      var embedBuilder = new EmbedBuilder()
        .WithTitle("How to Install Juscraft")
        .AddField("Prerequisites", """
        Before you install the modpack, there's a few things you should know:
        - You will need a launcher that supports .mrpack files. [Modrinth](https://modrinth.com/app) is recommended on Windows/MacOS and [Prism](https://prismlauncher.org/download/) on Linux/SteamOS.
          - SteamOS: In desktop mode, download Prism Launcher from the Discover software center. See more [here](https://prismlauncher.org/download/steam-deck/). You will probably need to plug in a keyboard and mouse to play. Note that you can install the modpack from a link, see "Installation - Prism".
        - You will need to download the modpack file. The latest version can be found in <#1378078557108047903>.
        """)
        .AddField("Installation - Modrinth (Recommended for Windows/MacOS)", """
        1. Download and install Modrinth from [here](https://modrinth.com/app).
        2. Open Modrinth.
        3. On the left sidebar hit "+".
        4. Select "From File" at the top, then drag in or select the modpack file.
        5. **Important**: by default Modrinth only allocates 2GB of RAM, which isn't enough for this pack. To fix this, click the settings icon (bottom left), choose "Default instance options" (left), then change "Memory allocated" to 4096 MB or higher. 6144 MB is recommended.
          - You can also change this per-instance instead, however this fixes the issue for future installations.
        6. Launch the pack by clicking play after choosing the instance on the left or in the library.
        """)
        .AddField("Installation - Prism (Recommended for Linux/SteamOS)", """
        1. Install the Prism flatpak (via your package manager or in the terminal run `flatpak install flathub org.prismlauncher.PrismLauncher`).
          - On SteamOS it is available in the Discover Software Center. For more info see [here](https://prismlauncher.org/download/steam-deck/).
        2. Open Prism.
        3. On the main app page, click "add instance" at the top left.
        4. Choose "Import" on the left
        5. Select your file by clicking browse (or enter the link to the modpack download - right click the attachment and copy file link). Then click OK.
        6. **Important**: by default Prism only allocates 2GB of RAM, which isn't enough for this pack. To fix this, on the top of the main screen, click "Settings", choose "Java" on the left, then change "Maximum memory allocation" to 4096 MiB/MB or higher. 6144 MiB/MB is recommended.
          - You can also change this per-instance instead, however this fixes the issue for future installations.
        7. Launch the pack by double clicking on it.
        """)
        .AddField("Updating", "Updating the pack requires you to install the new version of the pack using the steps above. To keep certain data (such as waypoints and mini-map progress) some files need to be copied over. For more information run `/docs modpack-updating` or see https://discord.com/channels/1375856723306352691/1375857288358793308/1376330001540186133 > Updating.")
        .AddField("** **", "This information is also visible in https://discord.com/channels/1375856723306352691/1375857288358793308/1376330001540186133")
        .WithCurrentTimestamp()
        .WithFooter("You can run this command with '/docs modpack-installation'");

      if (pingUser != null)
      {
        embedBuilder.WithAuthor(Context.User);
      }

      var embed = embedBuilder.Build();

      if (pingUser == null)
      {
        await RespondAsync(embeds: [embed], ephemeral: true);
      }
      else
      {
        await RespondAsync($"<@{pingUser.Id}>", embeds: [embed]);
      }
    }

    [SlashCommand("modpack-updating", "Displays instructions on how to update the modpack")]
    public async Task ModpackUpdating(
          [Summary("ping", "If specified, pings this user along with the message (and makes it visible to everyone).")] SocketUser? pingUser = null)
    {
      var embedBuilder = new EmbedBuilder()
        .WithTitle("How to Update Juscraft")
        .AddField("Prerequisites", """
        First, download and install the latest modpack version (for further instructions run `/docs modpack-installation` or https://discord.com/channels/1375856723306352691/1375857288358793308/1376330001540186133). Next, although this isn't technically necessary, if you want to keep your mini-map progress and settings, you will need to copy over some things from the old server profile to the new one.
        - :warning: Don't transfer any extra folders or files other than what is listed below, unless you know what you're doing! This will cause issues.
        """)
        .AddField("Basic", """
        Open the folder of the modpack installation (this can be easily done from the open folder button for the installation in Modrinth or Prism). From there, copy the following folders/files:
        - **options.txt** (saved video settings)
        - **servers.dat** (saved servers)
        - **xaero** (minimap data)
        - **shaderpacks/resourcepacks** (if you had any)
        - **schematics** (create schematicannon files)
        - **backups** (if you had any singleplayer worlds)
        - **emotes** (if you downloaded any custom emotes)
        - **saves** (if you had any singleplayer worlds)
        """)
        .AddField("Advanced", """
        If you also don't want to setup voicechat again or changed some advanced video settings and stuff that didn't carry over, you can transfer these as well. **DO NOT** transfer the entire config folder. This **WILL** cause issues if common configs were changed between updates.
        - **config/voicechat** (voicechat settings)
        - **config/sodium-extra-options.json** (advanced performance settings, e.g. disabling particles)
        """)
        .AddField("** **", "This information is also visible in https://discord.com/channels/1375856723306352691/1375857288358793308/1376330001540186133")
        .WithCurrentTimestamp()
        .WithFooter("You can run this command with '/docs modpack-updating'");

      if (pingUser != null)
      {
        embedBuilder.WithAuthor(Context.User);
      }

      var embed = embedBuilder.Build();

      if (pingUser == null)
      {
        await RespondAsync(embeds: [embed], ephemeral: true);
      }
      else
      {
        await RespondAsync($"<@{pingUser.Id}>", embeds: [embed]);
      }
    }

    [SlashCommand("modded-faq", "Displays some information about how to play modded minecraft and use certain mods")]
    public async Task ModdedFaq(
          [Summary("ping", "If specified, pings this user along with the message (and makes it visible to everyone).")] SocketUser? pingUser = null)
    {
      var embedBuilder = new EmbedBuilder()
        .WithTitle("How do I play modded minecraft?? HELP!! (FAQ, sort of?)")
        .WithDescription("""
        Calm down, it's not as complicated as you think. **Most things are explained in quests**, and you can **look up recipes in JEI**. However, you can see more information below.
        - **How do I manage Thirst??**: Thirst has been tuned to not be difficult to manage. You can drink straight from a water source (`[Shift]` + `[Right Click]`), from rain (look straight up), or with water bottles. Water bottles stack to 64 and you can purify water in a furnace, or by using a sand filter.
        - **JEI**: You will notice a list of items on the right when you enter a world. Clicking on them (or hovering over any item and pressing `[R]`) will show you the recipe. You can right click (or hover your mouse over an item and press `[U]`) to see the uses, or what recipes the item is used for.
        - **Quests**: I have made a ton of custom quests to help guide you through the modpack if you're new to the game. You don't need to strictly follow it if you don't want to, but many have rewards. You can access this by clicking the quest book in the top left of your inventory, or at any time by clicking the `` [`] `` key (next to 1)
        - **Are there parties/teams?**: Yes - run `/ftbteams party create <optional name>` to make one! It will allow you to sync quest progress, have party chat, and some other things.
        - **Can I sync my quests with someone else?**: Yes, you can! Make a party, and everyone on the team will have their quests sync with the party owner.
        - **Pondering**: Many blocks and items (mainly what's associated with Create) will have a ponder associated with them. Hold the key to get a visual tutorial on how the block works.
        - **Accessories**: You may find many relics or hats on your travels. You can equip/unequip them by clicking on the little ghost guy in your player preview in your inventory. Cosmetic slots can be shown by clicking the bar above your accessory slots.
        - **I have too many items in my inventory/hotbar!**: Craft a backpack, and open it by pressing `[G]` (`[B]` is the emote button). You can also craft a toolbelt (upgradable with pouches in an anvil), access it by holding R and clicking on what you want to swap. Both are equippable in the accessories menu.
        - **How do I emote?**: Press `[B]`. Press 'All Emotes' to access all emotes and configure your emote wheel. You can also [download custom emotes](https://emotecraft.kosmx.dev/?page=download-emotes&language=en).
        - **How do I make a colony?**: Note that there is a whole chapter in the quest book (accessed by pressing ``[`]``) for how to progress in MineColonies. MineColonies can be a lot, but you start by creating and placing down a supply ship or a supply camp. At that point you can choose what theme you want your colony to be (there are a lot). Colonies can expand to be quite large so they can't be too close to each other. Inside of your supply camp, you'll find a town hall and a builder's wand, and you will want to use the builder's wand to make a town hall. You should be able to figure it out from there, but for a more comprehensive tutorial see [the official getting started wiki](https://minecolonies.com/wiki/tutorials/getting-started/).
        - **Why do zombie villagers turn into colonists?**: You are near or inside the borders of a colony. Move further away, or just get resources by other means - there are probably better ways to automate it than villagers (yes, this includes enchanted books!)
        """)
        .AddField("** **", "This information is also visible in https://discord.com/channels/1375856723306352691/1375857288358793308/1376341172955517018")
        .WithCurrentTimestamp()
        .WithFooter("You can run this command with '/docs modded-faq'");

      if (pingUser != null)
      {
        embedBuilder.WithAuthor(Context.User);
      }

      var embed = embedBuilder.Build();

      if (pingUser == null)
      {
        await RespondAsync(embeds: [embed], ephemeral: true);
      }
      else
      {
        await RespondAsync($"<@{pingUser.Id}>", embeds: [embed]);
      }
    }

    [SlashCommand("general-faq", "Displays some information/FAQ about the server")]
    public async Task GeneralFaq(
          [Summary("ping", "If specified, pings this user along with the message (and makes it visible to everyone).")] SocketUser? pingUser = null)
    {
      var embedBuilder = new EmbedBuilder()
        .WithTitle("General FAQ")
        .WithDescription("""
        **How do I install/update/join/etc?**
        Type `/docs` in any channel and it will autofill with all the different docs commands, select the one corresponding with what information you're looking for. Additionally all this information is available in <#1375857288358793308>.
        **How do I change my roles? I want to get pinged for updates!**
        Go to <id:customize>.
        **What is performance like?**
        I have an RTX 2060 and a Ryzen 5 2600, and at 1440p I get around 200-300fps on the server and 170fps in singleplayer, at least with nothing major on my screen.
        The server has a Ryzen 7 3700X and performs decently (40 MSPT/20 TPS with 5 players online with 10 chunks server-side render distance, terrain (even new chunks) also loads extremely quickly).
        """)
        .AddField("** **", "This information is also visible in https://discord.com/channels/1375856723306352691/1375857288358793308/1378075094081077258")
        .WithCurrentTimestamp()
        .WithFooter("You can run this command with '/docs general-faq'");

      if (pingUser != null)
      {
        embedBuilder.WithAuthor(Context.User);
      }

      var embed = embedBuilder.Build();

      if (pingUser == null)
      {
        await RespondAsync(embeds: [embed], ephemeral: true);
      }
      else
      {
        await RespondAsync($"<@{pingUser.Id}>", embeds: [embed]);
      }
    }
  }
}