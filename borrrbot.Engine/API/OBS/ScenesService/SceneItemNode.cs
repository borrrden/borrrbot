using System;
using System.Collections.Generic;
using System.Text;

namespace borrrbot.API.OBS.ScenesService
{
    internal sealed class SceneItemNode : ISceneItemNode
    {
        public IReadOnlyList<string> childrenIds { get; }
        public string id { get; }
        public string parentId { get; }
        public string sceneId { get; }
        public string sceneNodeType { get; }
    }
}
