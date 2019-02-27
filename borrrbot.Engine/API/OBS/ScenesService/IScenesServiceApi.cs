using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace borrrbot.API.OBS.ScenesService
{
    public interface IScenesServiceApi
    {
        /// <summary>
        /// Gets the currently showing scene in OBS
        /// </summary>
        /// <returns>An awaitable task containing the scene object</returns>
        Task<ISceneApi> GetActiveSceneAsync();

        Task<string> GetActiveSceneIdAsync();

        IObservable<ISceneItem> itemAdded { get; }

        IObservable<ISceneItem> itemRemoved { get; }

        IObservable<ISceneItem> itemUpdated { get; }

        IObservable<IScene> sceneAdded { get; }

        IObservable<IScene> sceneRemoved { get; }

        IObservable<IScene> sceneSwitched { get; }

        Task<ISceneApi> CreateSceneAsync(string name, ISceneCreateOptions options);

        Task<IScenesState> GetModelAsync();

        Task<IReadOnlyList<ISceneApi>> GetScenesAsync();

        Task<bool> MakeSceneActiveAsync(string id);

        Task<IScene> RemoveSceneAsync(string id);

        Task<string> SuggestNameAsync(string name);
    }
}
