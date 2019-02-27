// 
//  BotEngine.cs
// 
//  This is free and unencumbered software released into the public domain.
//
//  Anyone is free to copy, modify, publish, use, compile, sell, or
//  distribute this software, either in source code form or as a compiled
//  binary, for any purpose, commercial or non-commercial, and by any
//  means.
//
//  In jurisdictions that recognize copyright laws, the author or authors
//  of this software dedicate any and all copyright interest in the
//  software to the public domain. We make this dedication for the benefit
//  of the public at large and to the detriment of our heirs and
//  successors. We intend this dedication to be an overt act of
//  relinquishment in perpetuity of all present and future rights to this
//  software under copyright law.
//
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
//  EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//  MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//  IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//  OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//  ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//  OTHER DEALINGS IN THE SOFTWARE.
//
//  For more information, please refer to <http://unlicense.org/>
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using borrrbot.API.OBS;
using borrrbot.Components;
using borrrbot.Handlers;

using JetBrains.Annotations;

using Microsoft.Extensions.Logging;

using SimpleInjector;

using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Interfaces;
using TwitchLib.Client.Models;

namespace borrrbot.API
{
    /// <summary>
    /// This is the main engine class that drives an implementation of
    /// the <see cref="IBot"/> interface
    /// </summary>
    public sealed class BotEngine
    {
        #region Variables

        [NotNull] private readonly IBot _bot;
        [NotNull] private readonly ILogger _logger;
        [NotNull] private readonly ComponentScheduler _scheduler = new ComponentScheduler();
        [NotNull] private readonly Dictionary<string, IBotCommand> CommandMap = new Dictionary<string, IBotCommand>();

        #endregion

        #region Properties

        /// <summary>
        /// Gets the channel that the bot is currently in
        /// </summary>
        public string Channel { get; }

        [NotNull] internal static Container Container { get; } = new Container();

        #endregion

        #region Constructors

        /// <summary>
        /// Static constructor
        /// </summary>
        static BotEngine()
        {
            Container.RegisterSingleton<ILoggerFactory>(() =>
            {
                var factory = new LoggerFactory();
                factory.AddConsole();
                return factory;
            });

            Container.RegisterSingleton<ITwitchClient>(() => new TwitchClient(logger: Container.GetInstance<ILoggerFactory>().CreateLogger<TwitchClient>()));
            Container.RegisterSingleton<ObsConnection>();
        }

        /// <summary>
        /// Constructor that initializes a bot engine instance with
        /// a bot to control
        /// </summary>
        /// <param name="bot">The bot to operate</param>
        public BotEngine([NotNull]IBot bot)
        {
            _bot = bot;
            Channel = _bot.Channel;
            _bot.RegisterSupportTypes(Container);
            _logger = Container.GetInstance<ILoggerFactory>().CreateLogger<BotEngine>();
            
            foreach (var commandType in _bot.GetBotCommands()) {
                var instantiated = Container.GetInstance(commandType) as IBotCommand;
                AddCommand(instantiated);
            }

            foreach (var componentType in _bot.GetBotComponents()) {
                var instantiated = Container.GetInstance(componentType) as IBotComponent;
                if (instantiated.GetType().Name == "TimedMessageComponent") {
                    continue;
                }
                AddComponent(instantiated);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets an implementation of a given interface if one is registered
        /// via dependency injection inside of the engine.
        /// </summary>
        /// <typeparam name="T">The interface type to retrieve an implementation for</typeparam>
        /// <returns>The implementation of the interface, or <c>null</c> if not registered</returns>
        public static T GetInstance<T>() where T : class => Container.GetInstance<T>();

        /// <summary>
        /// Adds a command to the bot engine
        /// </summary>
        /// <param name="command">The command to add</param>
        /// <returns><c>true</c> if the command was successfully initialized and added,
        /// otherwise <c>false</c></returns>
        public bool AddCommand(IBotCommand command)
        {
            var success = command.Initialize(this);
            if (success) {
                CommandMap[command.Shortcut] = command;
            }

            return success;
        }

        /// <summary>
        /// Adds a component to the bot engine.
        /// </summary>
        /// <param name="component">The compoment to add</param>
        public void AddComponent(IBotComponent component)
        {
            _scheduler.Schedule(component);
        }

        /// <summary>
        /// Gets a list of command names that are currently registered, excluding
        /// commands with the owner <see cref="CommandPermission"/>.  Moderator
        /// only commands have "(mod-only)" appended.
        /// </summary>
        /// <returns>The list of command names as described above</returns>
        public IEnumerable<string> CommandNames()
        {
            foreach (var pair in CommandMap.Where(x => x.Value.Permission != CommandPermission.Owner)
                .OrderBy(x => x.Key)) {
                if (pair.Value.Permission == CommandPermission.Moderator) {
                    yield return $"{pair.Key} (mod-only)";
                } else {
                    yield return pair.Key;
                }
            }
        }

        /// <summary>
        /// Starts the bot engine
        /// </summary>
        public void Start()
        {
            var creds = new ConnectionCredentials(_bot.BotName, _bot.BotOAuth);
            var client = Container.GetInstance<ITwitchClient>();
            client.Initialize(creds, _bot.Channel);
            client.OnChatCommandReceived += HandleCommand;
            client.OnConnected += (sender, args) => _scheduler.Activate(this);
            client.Connect();
        }

        #endregion

        #region Private Methods

        private void HandleCommand(object sender, OnChatCommandReceivedArgs e)
        {
            if (CommandMap.TryGetValue(e.Command.CommandText, out var command)) {
                switch (command.Permission) {
                    case CommandPermission.Owner:
                        if (!e.Command.ChatMessage.IsBroadcaster) {
                            return;
                        }

                        break;
                    case CommandPermission.Moderator:
                        if (!e.Command.ChatMessage.IsModerator && !e.Command.ChatMessage.IsBroadcaster) {
                            return;
                        }

                        break;
                    case CommandPermission.User:
                        break;
                }

                try {
                    command.HandleCommand(e.Command.ChatMessage.Username, e.Command.ArgumentsAsList);
                } catch (Exception err) {
                    _logger.LogError(err, "Error running command for {0}", e.Command.CommandText);
                }
            }
        }

        #endregion
    }
}
