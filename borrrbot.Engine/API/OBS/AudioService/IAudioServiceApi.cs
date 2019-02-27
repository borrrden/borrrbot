// 
//  IAudioServiceApi.cs
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

using System.Collections.Generic;
using System.Threading.Tasks;

using JetBrains.Annotations;

namespace borrrbot.API.OBS.AudioService
{
    /// <summary>
    /// An interface representing the top level audio service inside of OBS
    /// </summary>
    public interface IAudioServiceApi
    {
        #region Public Methods

        /// <summary>
        /// Gets a list of audio devices that are available to OBS
        /// </summary>
        /// <returns>A read-only list of available audio devices</returns>
        Task<IReadOnlyList<IAudioDevice>> GetDevicesAsync();

        /// <summary>
        /// Gets a handle to an audio source, given its OBS ID.
        /// </summary>
        /// <param name="sourceId">The ID of the audio source to retrieve</param>
        /// <returns>The handle to the audio source</returns>
        Task<IAudioSourceApi> GetSourceAsync(string sourceId);

        /// <summary>
        /// Gets all handles to all audio sources present in the entire
        /// OBS workspace
        /// </summary>
        /// <returns>A list of audio sources</returns>
        Task<IReadOnlyList<IAudioSourceApi>> GetSourcesAsync();

        /// <summary>
        /// Gets all handles to the audio sources present in the current scene
        /// </summary>
        /// <returns>A list of audio sources</returns>
        Task<IReadOnlyList<IAudioSourceApi>> GetSourcesForCurrentSceneAsync();

        /// <summary>
        /// Gets all handles to the audio devices in the scene with the given name
        /// </summary>
        /// <param name="sceneId">The ID of the scene to get audio sources from</param>
        /// <returns>A list of audio sources</returns>
        Task<IReadOnlyList<IAudioSourceApi>> GetSourcesForSceneAsync(string sceneId);

        #endregion
    }

    internal sealed class AudioServiceApi : IAudioServiceApi
    {
        private const string Resource = "AudioService";

        [NotNull]private readonly ObsConnection _obs;

        public AudioServiceApi([NotNull] ObsConnection obs)
        {
            _obs = obs;
        }

        public async Task<IReadOnlyList<IAudioDevice>> GetDevicesAsync()
        {
            return await _obs.SendRequestAsync<List<AudioDevice>>("getDevices", Resource)
                .ConfigureAwait(false);
        }

        public async Task<IAudioSourceApi> GetSourceAsync(string sourceId)
        {
            var retVal = await _obs.SendRequestAsync<AudioSourceApi>("getSource", Resource, sourceId)
                .ConfigureAwait(false);
            retVal.Obs = _obs;
            return retVal;
        }

        public async Task<IReadOnlyList<IAudioSourceApi>> GetSourcesAsync()
        {
            var retVal = await _obs.SendRequestAsync<List<AudioSourceApi>>("getSources", Resource)
                .ConfigureAwait(false);
            foreach (var entry in retVal) {
                entry.Obs = _obs;
            }

            return retVal;
        }

        public async Task<IReadOnlyList<IAudioSourceApi>> GetSourcesForCurrentSceneAsync()
        {
            var retVal = await _obs.SendRequestAsync<List<AudioSourceApi>>("getSourcesForCurrentScene", Resource)
                .ConfigureAwait(false);
            foreach (var entry in retVal) {
                entry.Obs = _obs;
            }

            return retVal;
        }

        public async Task<IReadOnlyList<IAudioSourceApi>> GetSourcesForSceneAsync(string sceneId)
        {
            var retVal = await _obs.SendRequestAsync<List<AudioSourceApi>>("getSourcesForScene", Resource, 
                sceneId).ConfigureAwait(false);
            foreach (var entry in retVal) {
                entry.Obs = _obs;
            }

            return retVal;
        }
    }
}
