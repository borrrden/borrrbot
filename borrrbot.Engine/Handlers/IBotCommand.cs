// 
//  IBotCommand.cs
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

using System.Collections.Generic;

using borrrbot.API;

using JetBrains.Annotations;

namespace borrrbot.Handlers
{
    /// <summary>
    /// The permission that is required to execute a give command
    /// </summary>
    public enum CommandPermission
    {
        /// <summary>
        /// Any use can use the command
        /// </summary>
        User,

        /// <summary>
        /// Moderators can use the command
        /// </summary>
        Moderator,

        /// <summary>
        /// Only the streamer can use the command
        /// </summary>
        Owner
    }

    /// <summary>
    /// This is one of the main interfaces that describes what a chat bot
    /// can do.  It is designed to reach to chat commands that start with
    /// an exclamation point (!) and carry out some arbitrary logic
    /// </summary>
    public interface IBotCommand
    {
        #region Properties

        /// <summary>
        /// Gets the permission level of the command
        /// </summary>
        CommandPermission Permission { get; }

        /// <summary>
        /// Gets the shortcut for the command
        /// </summary>
        string Shortcut { get; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Handles the command on behalf of the given user
        /// </summary>
        /// <param name="username">The user who typed the command</param>
        /// <param name="arguments">The arguments (space-split) that the command was given</param>
        void HandleCommand([NotNull]string username, [NotNull]IReadOnlyList<string> arguments);

        /// <summary>
        /// Initializes the command
        /// </summary>
        /// <param name="engine">The engine that this command will be used in</param>
        /// <returns><c>true</c> for successful initialization, <c>false</c> otherwise</returns>
        bool Initialize(BotEngine engine);

        #endregion
    }
}
