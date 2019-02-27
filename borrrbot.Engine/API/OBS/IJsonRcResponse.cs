using System;
using System.Collections.Generic;
using System.Text;

namespace borrrbot.API.OBS
{
    public interface IJsonRcResponse
    {
        #region Properties

        IJsonRcError error { get; }
        
        int id { get; }

        string jsonrpc { get; }

        #endregion
    }

    public interface IJsonRcResponse<out T> : IJsonRcResponse
    {
        T result { get; }
    }
}
