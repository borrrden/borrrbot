// 
//  AssemblyExtensions.cs
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

namespace borrrbot.API
{
    /// <summary>
    /// A few extensions on the Assembly class for finding types
    /// </summary>
    public static class AssemblyExtensions
    {
        #region Public Methods

        /// <summary>
        /// Finds all the types in a given assembly that are of type
        /// <see cref="Handlers.IBotCommand"/>
        /// </summary>
        /// <param name="assembly">The assembly to search in</param>
        /// <returns>All types that implement the bot command interface</returns>
        public static IEnumerable<Type> BotCommands(this Assembly assembly)
        {
            var commands = from type in assembly.GetTypes()
                where !type.IsAbstract
                where type.GetInterface("borrrbot.Handlers.IBotCommand") != null
                select type;

            foreach (var commandType in commands) {
                yield return commandType;
            }
        }

        /// <summary>
        /// Finds all the types in a given assembly that are of type
        /// <see cref="Components.IBotComponent"/>
        /// </summary>
        /// <param name="assembly">The assembly to search in</param>
        /// <returns>All types that implement the bot command interface</returns>
        public static IEnumerable<Type> BotComponents(this Assembly assembly)
        {
            var commands = from type in assembly.GetTypes()
                where !type.IsAbstract
                where type.GetInterface("borrrbot.Components.IBotComponent") != null
                select type;

            foreach (var commandType in commands) {
                yield return commandType;
            }
        }

        #endregion
    }
}
