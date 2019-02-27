using System;
using System.Collections.Generic;
using System.Text;

namespace borrrbot.API.OBS.ScenesService
{
    public interface ISceneItemNode
    {
        IReadOnlyList<string> childrenIds { get; }

        string id { get; }

        string parentId { get; }

        string sceneId { get; }

        string sceneNodeType { get; }
    }
}
