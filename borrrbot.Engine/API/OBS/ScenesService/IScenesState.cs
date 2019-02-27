using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;

namespace borrrbot.API.OBS.ScenesService
{
    public interface IScenesState
    {
        string activeSceneId { get; }

        IReadOnlyList<string> displayOrder { get; }

        IReadOnlyDictionary<string, IScene> scenes { get; }
    }

    internal sealed class ScenesState : IScenesState
    {
        public string activeSceneId { get; set; }

        public IReadOnlyList<string> displayOrder { get; set;  }

        [JsonIgnore]
        public ObsConnection Obs { get; set; }

        [JsonConverter(typeof(ConcreteListTypeConverter<IScene, Scene>))]
        public IReadOnlyDictionary<string, IScene> scenes { get; }
    }
}
