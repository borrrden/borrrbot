// 
//  IPartialTransform.cs
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

using System.Numerics;

using Newtonsoft.Json;

namespace borrrbot.API.OBS.ScenesService
{
    /// <summary>
    /// An interface representing the main elements of an item's transform
    /// </summary>
    public interface IPartialTransform
    {
        #region Properties

        /// <summary>
        /// Gets the cropbox for the element
        /// </summary>
        ICrop crop { get; }

        /// <summary>
        /// Gets the position of the element on screen
        /// </summary>
        Vector2 position { get; }

        /// <summary>
        /// Get the angle of rotation of the element
        /// </summary>
        float rotation { get; }

        /// <summary>
        /// Gets the scale of the element
        /// </summary>
        Vector2 scale { get; }

        #endregion
    }

    /// <summary>
    /// The default implementation of <see cref="IPartialTransform"/>
    /// </summary>
    public sealed class PartialTransform : IPartialTransform
    {
        #region Properties

        /// <inheritdoc />
        [JsonConverter(typeof(InterfaceConverter<ICrop, Crop>))]
        public ICrop crop { get; set; }

        /// <inheritdoc />
        [JsonConverter(typeof(VectorConverter))]
        public Vector2 position { get; set; }

        /// <inheritdoc />
        public float rotation { get; set; }

        /// <inheritdoc />
        [JsonConverter(typeof(VectorConverter))]
        public Vector2 scale { get; set; }

        #endregion

        #region Overrides

        /// <inheritdoc />
        public override string ToString()
        {
            return $"Position [{position.X}, {position.Y}], Scale [{scale.X}, {scale.Y}], Rotation: {rotation}°";
        }

        #endregion
    }
}
