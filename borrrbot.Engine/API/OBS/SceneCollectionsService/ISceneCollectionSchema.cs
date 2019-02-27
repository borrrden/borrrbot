using System;
using System.Collections.Generic;
using System.Text;

namespace borrrbot.API.OBS.SceneCollectionsService
{
    public interface ISceneCollectionSchema
    {
        string id { get; }

        string name { get; }

        IReadOnlyList<object> scenes { get; }

        IReadOnlyList<object> sources { get; }
    }
}
