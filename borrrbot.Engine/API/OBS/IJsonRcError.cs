using System;
using System.Collections.Generic;
using System.Text;

namespace borrrbot.API.OBS
{
    public enum JsonRcErrorCode
    {
        InternalJsonRpcError = -32603,
        InternalServerError = -32000,
        InvalidParams = -32602,
        InvalidRequest = -32600,
        MethodNotFound = -32601,
        ParseError = -32700
    }

    public interface IJsonRcError
    {
        #region Properties
        JsonRcErrorCode code { get; }

        object data { get; }

        string message { get; }

        #endregion
    }
}
