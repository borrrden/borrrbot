// 
//  ICrop.cs
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

namespace borrrbot.API.OBS.ScenesService
{
    /// <summary>
    /// An interface representing a crop box on a visual
    /// element inside of OBS
    /// </summary>
    public interface ICrop
    {
        #region Properties

        /// <summary>
        /// Gets the bottom Y value of the box
        /// </summary>
        string bottom { get; }

        /// <summary>
        /// Gets the left X value of the box
        /// </summary>
        string left { get; }

        /// <summary>
        /// Gets the right X value of the box
        /// </summary>
        string right { get; }

        /// <summary>
        /// Gets the top Y value of the box
        /// </summary>
        string top { get; }

        #endregion
    }

    internal sealed class Crop : ICrop
    {
        #region Properties

        public string bottom { get; set; }
        public string left { get; set; }
        public string right { get; set; }
        public string top { get; set; }

        #endregion
    }
}
