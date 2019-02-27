// 
//  InterfaceConverter.cs
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
using System.IO;
using System.Numerics;

using borrrbot.API.OBS.ScenesService;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace borrrbot.API.OBS
{
    internal sealed class VectorConverter : JsonConverter
    {
        #region Overrides

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Vector2);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return serializer.Deserialize(reader, objectType);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var vector = (Vector2) value;
            writer.WriteStartObject();
            writer.WritePropertyName("x");
            writer.WriteValue(vector.X);
            writer.WritePropertyName("y");
            writer.WriteValue(vector.Y);
            writer.WriteEndObject();
        }

        #endregion
    }
    internal sealed class InterfaceConverter<TInterface, TConcrete> : CustomCreationConverter<TInterface>
        where TConcrete : TInterface, new()
    {
        #region Overrides

        public override TInterface Create(Type objectType)
        {
            return new TConcrete();
        }

        #endregion
    }

    internal sealed class ConcreteListTypeConverter<TInterface, TImplementation> : JsonConverter where TImplementation : TInterface 
    {
        #region Overrides

        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var res = serializer.Deserialize<List<TImplementation>>(reader);
            return res.ConvertAll(x => (TInterface) x);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }

        #endregion
    }

    internal sealed class SceneNodeApiConverter : JsonConverter
    {
        #region Overrides

        public override bool CanConvert(Type objectType)
        {
            return typeof(ISceneNodeApi).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var intermediate = JToken.ReadFrom(reader);
            var type = intermediate.SelectToken("sceneNodeType").ToObject<string>();
            switch (type) {
                case SceneNodeType._Item:
                    return intermediate.ToObject<SceneItemApi>();
                case SceneNodeType._Folder:
                    return intermediate.ToObject<SceneItemFolderApi>();
                default:
                    throw new InvalidDataException("Invalid scene node type!");
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
