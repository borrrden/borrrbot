// 
//  IAudioSourceApi.cs
// 
//  This is free and unencumbered software released into the public domain.
//
//  Anyone is free to copy, modify, publish, use, compile, sell, or
//  distribute this software, either in source code form or as a compiled
//  binary, for any purpose, commercial or non-commercial, and by any
//  means.
//
//  In jurisdictions that recognize copyright laws, the author or authors
//  of this software dedicate any and all copyright interest in the
//  software to the public domain. We make this dedication for the benefit
//  of the public at large and to the detriment of our heirs and
//  successors. We intend this dedication to be an overt act of
//  relinquishment in perpetuity of all present and future rights to this
//  software under copyright law.
//
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
//  EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//  MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//  IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//  OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//  ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//  OTHER DEALINGS IN THE SOFTWARE.
//
//  For more information, please refer to <http://unlicense.org/>
//

using System;
using System.Threading.Tasks;

using JetBrains.Annotations;

using Newtonsoft.Json;

namespace borrrbot.API.OBS.AudioService
{
    public interface IAudioSourceApi : IAudioSource
    {
        #region Public Methods

        Task<IAudioSource> GetModelAsync();

        Task SetDeflectionAsync(float deflection);

        Task SetMulAsync(float mul);

        Task SetMutedAsync(bool muted);

        Task SetSettingsAsync(IAudioSource settings);

        Task SubscribeVolmeterAsync(Action<IVolmeter> callback);

        #endregion
    }

    internal sealed class AudioSourceApi : IAudioSourceApi
    {
        #region Properties
        
        [JsonIgnore]
        public ObsConnection Obs { get; set; }

        public int audioMixers { get; set; }

        [JsonConverter(typeof(InterfaceConverter<IFader, Fader>))]
        public IFader fader { get; set; }

        public bool forceMono { get; set; }
        public int monitoringType { get; set; }
        public bool muted { get; set; }
        public string resourceId { get; set; }
        public string sourceId { get; set; }
        public int syncOffset { get; set; }

        #endregion

        #region IAudioSourceApi

        public async Task<IAudioSource> GetModelAsync()
        {
            return await Obs.SendRequestAsync<AudioSourceApi>("getModel", resourceId)
                .ConfigureAwait(false);
        }

        public Task SetDeflectionAsync(float deflection)
        {
            return Obs.SendSimpleRequestAsync("setDeflection", resourceId, deflection);
        }

        public Task SetMulAsync(float mul)
        {
            return Obs.SendSimpleRequestAsync("setMul", resourceId, mul);
        }

        public Task SetMutedAsync(bool muted)
        {
            return Obs.SendSimpleRequestAsync("setMuted", resourceId, muted);
        }

        public Task SetSettingsAsync(IAudioSource settings)
        {
            return Obs.SendSimpleRequestAsync("setSettings", resourceId, settings);
        }

        public Task SubscribeVolmeterAsync(Action<IVolmeter> callback)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
