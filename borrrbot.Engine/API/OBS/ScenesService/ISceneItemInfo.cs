using System;
using System.Collections.Generic;
using System.Text;

namespace borrrbot.API.OBS.ScenesService
{
    public interface ISceneItemInfo
    {
        ICrop crop { get; }

        string id { get; }

        bool locked { get; }

        float rotation { get; }

        float scaleX { get; }

        float scaleY { get; }

        string sourceId { get; }

        bool visible { get; }

        int x { get; }

        int y { get; }
    }
}
