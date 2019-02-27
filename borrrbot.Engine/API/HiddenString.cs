// 
//  HiddenString.cs
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

using System.Diagnostics;

namespace borrrbot.API
{
    /// <summary>
    /// This is a helper class for use during debugger, especially debugging on stream.
    /// It does not actually hide anything (it is still easily accessible) but by default
    /// if you are debugging the string will show up as '&lt;hidden&gt;' in the debugger
    /// so you don't accidentally show something sensitive on stream.
    /// </summary>
    public sealed class HiddenString
    {
        #region Variables

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string _str;

        #endregion

        #region Properties

        /// <summary>
        /// Use this for comparing with <c>null</c> instead of directly
        /// saying <c>val == null</c>
        /// </summary>
        public bool IsNull => _str == null;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor that initializes the object with a string
        /// </summary>
        /// <param name="str">The string value to store</param>
        public HiddenString(string str)
        {
            _str = str;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Converts a string to a hidden string
        /// </summary>
        /// <param name="str">The string to create the hidden string with</param>
        public static implicit operator HiddenString(string str) => new HiddenString(str);

        /// <summary>
        /// Converts a hidden string to a string (for use programatically)
        /// </summary>
        /// <param name="str">The hidden string to extract the string from</param>
        public static implicit operator string(HiddenString str) => str?._str;

        #endregion

        #region Overrides

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            switch (obj) {
                case HiddenString hs:
                    return hs._str == _str;
                case string s:
                    return _str == s;
                default:
                    return false;
            }
        }
        /// <inheritdoc />
        /// <inheritdoc />
        public override int GetHashCode()
        {
            return _str?.GetHashCode() ?? 0;
        }

        /// <inheritdoc />
        public override string ToString() => "<hidden>";

        #endregion
    }
}
