using System;
using System.Collections.Generic;
using System.Text;

namespace borrrbot.API.OBS.SceneCollectionsService
{
    public interface ISceneCollectionsManifestEntry
    {
        bool deleted { get; }

        string id { get; }

        string modified { get; }

        string name { get; }

        bool needsRename { get; }

        int serverId { get; }
    }
}
