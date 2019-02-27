using System;
using System.Collections.Generic;

namespace borrrbot.API.OBS
{
    public interface IJsonRcRequest
    {
        #region Properties

        int id { get; }

        string jsonrpc { get; }

        string method { get; }

        IReadOnlyDictionary<string, object> Params { get; }

        Type ResponseType();

        #endregion
    }
}
