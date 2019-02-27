# borrrbot
A simple C# engine / framework for creating a Twitch bot

Here are the various components included in here

### API Folder

This folder contains an interface for interfacing with OBS Studio and its derivatives (Streamlabs OBS, etc) as well as the main interface for the bot and the engine responsible for running it.

### Components Folder

This folder contains the tools to create components (i.e. things that run independent of commands like repeating messages, etc)

### Handlers Folder

The interface for bot commands

### OBS Folder

Some utilities for working with OBS

## How To Create a Bot

To create a bot using this framework you basically need to implement `IBot` and create a `BotEngine`.  Here is a quick example

```c#
internal sealed class Bot : IBot
{
    // A setup for getting information from environemnt variables
    private const string BotOAuthKey = "BOT_OAUTH";
    private const string ChannelOAuthKey = "CHANNEL_OAUTH";
    private const string TwitchClientIDKey = "TWITCH_CLIENT_ID";
    public static readonly HiddenString _BotOAuth = Environment.GetEnvironmentVariable(BotOAuthKey);
    public static readonly HiddenString ChannelOAuth = Environment.GetEnvironmentVariable(ChannelOAuthKey);
    public static readonly HiddenString TwitchClientID = Environment.GetEnvironmentVariable(TwitchClientIDKey);

    public string BotName { get; }
    public HiddenString BotOAuth => _BotOAuth;
    public string Channel { get; }

    // This is assuming you have your commands in the same project.  If not replace "typeof(Bot)" with a type from
    // that assembly.  In this example I skip any commands that have "Dynamic" in the namespace of the class (e.g.
    // My.Namespace.Dynamic.CoolCommand would be skipped)
    public IEnumerable<Type> GetBotCommands() => typeof(Bot).Assembly.BotCommands()
        .Where(x => !x.Namespace.Contains("Dynamic"));

    public IEnumerable<Type> GetBotComponents() => typeof(Bot).Assembly.BotComponents();

    public void RegisterSupportTypes(Container container)
    {
        // Hook the twitch API into the engine configured with login credentials
        container.RegisterSingleton<ITwitchAPI>(() =>
        {
            var settings = new ApiSettings
            {
                AccessToken = ChannelOAuth,
                ClientId = TwitchClientID
            };

            return new TwitchAPI(BotEngine.GetInstance<ILoggerFactory>(), settings: settings);
        });
    }

    public Bot(string botName, string channel)
    {
        BotName = botName;
        Channel = channel;
    }
}

static void Main(string[] args)
{
    var bot = new Bot("botname", "channel_to_join");
    var engine = new BotEngine(bot);
    engine.Start();

    Console.ReadKey(true);
}
```

## How To Create a Command

To create a command just implement the `IBotCommand` interface with a class and make sure that the bot returns it in its `GetBotCommands()` list.  Alternatively, using `engine.AddCommand` to add it manually.  Here is a very basic example. The username and any arguments (which are a list of space separated strings that come after the command itself) will be provided to the command.  You can then use this command in a Twitch chatroom via typing `!hello`.  The default command name will be the name of the class without 'Command' (HelloCommand -> hello).  You can override this by overriding the `Shortcut` property.

```c#
public class HelloCommand : IBotCommand
{
    #region Variables

    private readonly ITwitchClient _twitchClient = BotEngine.GetInstance<ITwitchClient>();

    #endregion

    #region Overrides

    public override void HandleCommand(string username, IReadOnlyList<string> arguments)
    {
        var response = $"Hello, {username}";
        TwitchClient.SendMessage(Channel, response);
    }

    #endregion
}
```
