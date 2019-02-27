using System;
using System.Collections.Generic;
using System.Text;

namespace borrrbot.API.OBS.SceneCollectionsService
{
    public interface ISceneCollectionCreateOptions
    {
        string name { get; }

        bool needsRename { get; }


    }
}
