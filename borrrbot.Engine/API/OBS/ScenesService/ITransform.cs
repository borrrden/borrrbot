using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

using Newtonsoft.Json;

namespace borrrbot.API.OBS.ScenesService
{
    public interface ITransform
    {
        ICrop crop { get; }

        Vector2 position { get; }

        int rotation { get; }

        Vector2 scale { get; }
    }

    internal sealed class Transform : ITransform
    {
        [JsonConverter(typeof(InterfaceConverter<ICrop, Crop>))]
        public ICrop crop { get; set; }
        public Vector2 position { get; set; }
        public int rotation { get; set; }
        public Vector2 scale { get; set; }

        public override string ToString()
        {
            return $"Position [{position.X}, {position.Y}], Scale [{scale.X}, {scale.Y}], Rotation: {rotation}°";
        }
    }
}
