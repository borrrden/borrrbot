using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace borrrbot.API.OBS.SceneCollectionsService
{
    public interface ISceneCollectionsServiceApi
    {
        event EventHandler CollectionWillSwitch;

        ISceneCollectionsManifestEntry activeCollection { get; }

        IObservable<ISceneCollectionsManifestEntry> collectionAdded { get; }

        IObservable<ISceneCollectionsManifestEntry> collectionRemoved { get; }

        IObservable<ISceneCollectionsManifestEntry> collectionSwitched { get; }

        IObservable<ISceneCollectionsManifestEntry> collectionUpdated { get; }

        IReadOnlyList<ISceneCollectionsManifestEntry> collections { get; }

        Task Create(ISceneCollectionCreateOptions options);

        Task<IReadOnlyList<ISceneCollectionSchema>> FetchSceneCollectionsSchema();

        Task Load(string id);


    }
}
