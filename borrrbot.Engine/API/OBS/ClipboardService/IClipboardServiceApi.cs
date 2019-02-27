using System;
using System.Collections.Generic;
using System.Text;

namespace borrrbot.API.OBS.ClipboardService
{
    public interface IClipboardServiceApi
    {
        void Clear();

        void Copy();

        void CopyFilters();

        bool HasFilters();

        bool HasItems();

        void PasteDuplicate(string toSourceId);

        void PasteReference();
    }
}
