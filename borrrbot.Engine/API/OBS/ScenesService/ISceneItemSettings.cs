using System;
using System.Collections.Generic;
using System.Text;

namespace borrrbot.API.OBS.ScenesService
{
    public interface ISceneItemSettings
    {
        IReadOnlyList<string> childrenIds { get; }

        string id { get; }

        bool locked { get; }

        int obsSceneItemId { get; }

        string parentId { get; }

        string sceneId { get; }

        string sceneItemId { get; }

        string sceneNodeType { get; }

        string sourceId { get; }

        ITransform transform { get; }

        bool visible { get; }
    }
}
