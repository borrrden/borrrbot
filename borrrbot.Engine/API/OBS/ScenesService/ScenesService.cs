// 
//  ScenesService.cs
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
using System.Collections.Generic;
using System.Threading.Tasks;

using JetBrains.Annotations;

namespace borrrbot.API.OBS.ScenesService
{
    internal sealed class ScenesServiceApi : IScenesServiceApi
    {
        #region Constants

        private const string Resource = "ScenesService";

        #endregion

        #region Variables

        [NotNull] private readonly ObsConnection _obs;

        #endregion

        #region Properties

        public IObservable<ISceneItem> itemAdded { get; }
        public IObservable<ISceneItem> itemRemoved { get; }
        public IObservable<ISceneItem> itemUpdated { get; }
        public IObservable<IScene> sceneAdded { get; }
        public IObservable<IScene> sceneRemoved { get; }
        public IObservable<IScene> sceneSwitched { get; }

        #endregion

        #region Constructors

        public ScenesServiceApi([NotNull]ObsConnection obs)
        {
            _obs = obs;
        }

        #endregion

        #region IScenesServiceApi

        public async Task<ISceneApi> CreateSceneAsync(string name, ISceneCreateOptions options)
        {
            var response = await _obs.SendRequestAsync<SceneApi>("createScene", Resource, name, options).ConfigureAwait(false);
            response.Obs = _obs;
            return response;
        }

        public async Task<ISceneApi> GetActiveSceneAsync()
        {
            var retVal = await _obs.SendRequestAsync<SceneApi>("activeScene", Resource);
            retVal.Obs = _obs;
            return retVal;
        }

        public Task<string> GetActiveSceneIdAsync()
        {
            return _obs.SendRequestAsync<string>("activeSceneId", Resource);
        }

        public async Task<IScenesState> GetModelAsync()
        {
            var response = await _obs.SendRequestAsync<ScenesState>("getModel", Resource).ConfigureAwait(false);
            response.Obs = _obs;
            return response;
        }

        public async Task<IReadOnlyList<ISceneApi>> GetScenesAsync()
        {
            var response = await _obs.SendRequestAsync<IReadOnlyList<SceneApi>>("getScenes", Resource).ConfigureAwait(false);
            foreach (var item in response) {
                item.Obs = _obs;
            }

            return response;
        }

        public Task<bool> MakeSceneActiveAsync(string id)
        {
            return _obs.SendRequestAsync<bool>("makeSceneActive", Resource, id);
        }

        public async Task<IScene> RemoveSceneAsync(string id)
        {
            return await _obs.SendRequestAsync<Scene>("removeScene", Resource, id).ConfigureAwait(false);
        }

        public Task<string> SuggestNameAsync(string name)
        {
            return _obs.SendRequestAsync<string>("suggestName", Resource, name);
        }

        #endregion
    }
}
