using System;
using System.Collections.Generic;
using System.Text;

namespace borrrbot.API.OBS.CustomizationService
{
    public interface ICustomizationServiceState
    {
        float chatZoomFactor { get; }

        bool enableBTTVEmotes { get; }

        bool enableFFZEmotes { get; }

        object experimental { get; }

        bool hideViewerCount { get; }

        bool leftDock { get; }

        bool livePreviewEnabled { get; }

        bool livedockCollapsed { get; }

        float livedockSize { get; }

        bool nightMode { get; }

        bool performanceMode { get; }

        float previewSize { get; }

        bool updateStreamInfoOnLive { get; }
    }
}
