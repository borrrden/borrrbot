using System;
using System.Collections.Generic;
using System.Threading;

using JetBrains.Annotations;

using Newtonsoft.Json;

namespace borrrbot.API.OBS
{
    internal class JsonRcRequest : IJsonRcRequest
    {
        #region Variables

        [NotNull]private readonly Dictionary<string, object> _params = new Dictionary<string, object>();
        [NotNull]private readonly Type _responseType;

        private static int NextID = 1;

        #endregion

        #region Properties

        public int id { get; }

        public virtual string jsonrpc => "2.0";

        public string method { get; }

        [JsonProperty(PropertyName = "params")]
        public IReadOnlyDictionary<string, object> Params => _params;

        #endregion

        #region Constructors

        public JsonRcRequest(string method, [NotNull]Type responseType)
        {
            id = Interlocked.Increment(ref NextID);
            this.method = method;
            _responseType = responseType;
        }

        #endregion

        #region Public Methods

        [NotNull]
        public Type ResponseType() => _responseType;

        #endregion

        #region Protected Methods

        public void AddParam(string key, object value)
        {
            _params[key] = value;
        }

        #endregion
    }

    internal abstract class JsonRcResponse : IJsonRcResponse
    {
        [JsonConverter(typeof(InterfaceConverter<IJsonRcError, JsonRcError>))]
        public IJsonRcError error { get; set; }
        public int id { get; set; }
        public string jsonrpc { get; set; }
    }

    internal sealed class JsonRcResponse<T> : JsonRcResponse, IJsonRcResponse<T>
    {
        #region Properties

        public T result { get; set; }

        #endregion
    }

    internal sealed class JsonRcError : IJsonRcError
    {
        #region Properties

        public JsonRcErrorCode code { get; set; }

        public object data { get; set; }

        public string message { get; set; }

        #endregion
    }
}
