// 
//  IBot.cs
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

using JetBrains.Annotations;

using SimpleInjector;

namespace borrrbot.API
{
    /// <summary>
    /// This is the interface that a C# twitch bot must implement in order to be
    /// runnable by the <see cref="BotEngine"/> class.  
    /// </summary>
    public interface IBot
    {
        #region Properties

        /// <summary>
        /// Gets the bot's chatroom name
        /// </summary>
        [NotNull]
        string BotName { get; }

        /// <summary>
        /// Gets the bot's OAuth token for logging in
        /// </summary>
        [NotNull]
        HiddenString BotOAuth { get; }

        /// <summary>
        /// Gets the bot's desired channel name
        /// </summary>
        [NotNull]
        string Channel { get; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets a list of bot commands to use in the engine.
        /// </summary>
        /// <returns>The list of bot commands</returns>
        [NotNull]
        [ItemNotNull]
        IEnumerable<Type> GetBotCommands();

        /// <summary>
        /// Gets a list of bot components to use in the engine
        /// </summary>
        /// <returns></returns>
        [NotNull]
        [ItemNotNull]
        IEnumerable<Type> GetBotComponents();

        /// <summary>
        /// Registers some domain specific support types for use
        /// when instantiating bot commands or components that
        /// are not available by default in the engine
        /// </summary>
        /// <param name="container">The container to register the types in</param>
        void RegisterSupportTypes(Container container);

        #endregion
    }
}
