using System;

namespace borrrbot.API.OBS
{
    public sealed class ObsException : Exception
    {
        #region Properties

        public JsonRcErrorCode Code { get; }

        #endregion

        #region Constructors

        internal ObsException(IJsonRcError error) : base(error.message)
        {
            Code = error.code;
        }

        #endregion
    }
}
