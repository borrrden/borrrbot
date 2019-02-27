// 
//  ISceneApi.cs
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

using Newtonsoft.Json;

namespace borrrbot.API.OBS.ScenesService
{
    /// <summary>
    /// An interface representing an editable scene in OBS
    /// </summary>
    public interface ISceneApi : IScene
    {
        #region Public Methods

        /// <summary>
        /// Add a source into the scene with the given id and options
        /// </summary>
        /// <param name="sourceId">The id of the source to add into the scene</param>
        /// <param name="options">The options to use when adding the source</param>
        /// <returns>The scene item for further editing</returns>
        Task<ISceneItemApi> AddSourceAsync(string sourceId, ISceneNodeAddOptions options);

        /// <summary>
        /// Checks whether the scene can add a given source or not
        /// </summary>
        /// <param name="sourceId">The ID of the source to attempt to add</param>
        /// <returns><c>true</c> if the add can occur, otherwise <c>false</c></returns>
        Task<bool> CanAddSourceAsync(string sourceId);

        /// <summary>
        /// Creates a new source with the given name and type, and adds it
        /// to the scene
        /// </summary>
        /// <param name="name">The name of the source to add</param>
        /// <param name="type">The type of the source to add</param>
        /// <returns>The scene item for further editing</returns>
        Task<ISceneItemApi> CreateAndAddSourceAsync(string name, string type);

        /// <summary>
        /// Creates a folder with the given name in the scene
        /// </summary>
        /// <param name="name">The name of the folder to add</param>
        /// <returns>The scene folder item for further editing</returns>
        Task<ISceneItemFolderApi> CreateFolderAsync(string name);

        /// <summary>
        /// Gets a folder inside the scene given its folder ID
        /// </summary>
        /// <param name="sceneFolderId">The ID of the folder to retrieve</param>
        /// <returns>The scene folder item for editing</returns>
        Task<ISceneItemFolderApi> GetFolderAsync(string sceneFolderId);

        /// <summary>
        /// Gets a list of folders in the current scene
        /// </summary>
        /// <returns>A list of scene folder items in the scene for editing</returns>
        Task<IReadOnlyList<ISceneItemFolderApi>> GetFoldersAsync();

        /// <summary>
        /// Gets a scene item in the current scene given its ID.
        /// A scene item can be either a node or a folder.
        /// </summary>
        /// <param name="sceneItemId">The ID of the item to retrieve</param>
        /// <returns>The scene item for editing</returns>
        Task<ISceneItemApi> GetItemAsync(string sceneItemId);

        /// <summary>
        /// Gets a list of items in the current scene
        /// </summary>
        /// <returns>A list of scene items for editing</returns>
        Task<IReadOnlyList<ISceneItemApi>> GetItemsAsync();

        /// <summary>
        /// Gets the model for this scene in the case of
        /// abbreviated responses (usually not used)
        /// </summary>
        /// <returns>The scene item representing the current scene</returns>
        Task<IScene> GetModelAsync();

        /// <summary>
        /// Gets a node from the scene given its scene ID
        /// </summary>
        /// <param name="sceneNodeId">The scene ID of the node to retrieve</param>
        /// <returns></returns>
        Task<ISceneNodeApi> GetNodeAsync(string sceneNodeId);

        /// <summary>
        /// Gets a node from the scene given its name
        /// </summary>
        /// <param name="name">The name of the node to retrieve</param>
        /// <returns>The scene node item for editing</returns>
        Task<ISceneNodeApi> GetNodeByNameAsync(string name);

        /// <summary>
        /// Gets all nodes in the scene, nested or otherwise
        /// </summary>
        /// <returns>A list of nodes, including nested nodes</returns>
        Task<IReadOnlyList<ISceneNodeApi>> GetNodesAsync();

        /// <summary>
        /// Gets all root nodes in the scene, excluding nested ones.
        /// </summary>
        /// <returns>A list of root nodes</returns>
        Task<IReadOnlyList<ISceneNodeApi>> GetRootNodesAsync();

        //ISelection GetSelection(string itemsList);

        //ISelection GetSelection(IReadOnlyList<string> itemsList);

        //ISelection GetSelection(ISceneItemNode itemsList);

        //ISelection GetSelection(IReadOnlyList<ISceneItemNode> itemsList);

        /// <summary>
        /// Makes the scene active in OBS (thereby displaying it)
        /// </summary>
        /// <returns>An awaitable task representing completion</returns>
        Task MakeActiveAsync();

        /// <summary>
        /// Removes the scene from OBS
        /// </summary>
        /// <returns>An awaitable task representing completion</returns>
        Task RemoveAsync();

        /// <summary>
        /// Removes a folder from the scene by ID
        /// </summary>
        /// <param name="folderId">The ID of the folder to remove</param>
        /// <returns>An awaitable task representing completion</returns>
        Task RemoveFolderAsync(string folderId);

        /// <summary>
        /// Removes an item from the scene by ID
        /// </summary>
        /// <param name="sceneItemId">The ID of the item to remove</param>
        /// <returns>An awaitable task representing completion</returns>
        Task RemoveItemAsync(string sceneItemId);

        /// <summary>
        /// Sets the name of the scene to the new value
        /// </summary>
        /// <param name="newName">The new name for the scene</param>
        /// <returns>An awaitable task representing completion</returns>
        Task SetNameAsync(string newName);

        #endregion
    }

    internal sealed class SceneApi : ISceneApi
    {
        #region Properties

        [JsonIgnore]
        public ObsConnection Obs { get; set; }

        public string resourceId { get; set; }
        public string id { get; set; }
        public string name { get; set; }

        [JsonConverter(typeof(ConcreteListTypeConverter<ISceneItemNode, SceneItemNode>))]
        public IReadOnlyList<ISceneItemNode> nodes { get; set;  }

        #endregion

        #region Overrides

        public override string ToString()
        {
            return $"{name} ({id})";
        }

        #endregion

        #region ISceneApi

        public async Task<ISceneItemApi> AddSourceAsync(string sourceId, ISceneNodeAddOptions options)
        {
            var retVal = await Obs.SendRequestAsync<SceneItemApi>("addSource", resourceId, sourceId, options)
                .ConfigureAwait(false);
            retVal.Obs = Obs;
            return retVal;
        }

        public async Task<bool> CanAddSourceAsync(string sourceId)
        {
            return await Obs.SendRequestAsync<bool>("canAddSource", resourceId, sourceId)
                .ConfigureAwait(false);
        }

        public async Task<ISceneItemApi> CreateAndAddSourceAsync(string name, string type)
        {
            var retVal = await Obs.SendRequestAsync<SceneItemApi>("createAndAddSource", resourceId,
                name, type).ConfigureAwait(false);
            retVal.Obs = Obs;
            return retVal;
        }

        public async Task<ISceneItemFolderApi> CreateFolderAsync(string name)
        {
            var retVal = await Obs.SendRequestAsync<SceneItemFolderApi>("createFolder", resourceId, name)
                .ConfigureAwait(false);
            retVal.Obs = Obs;
            return retVal;
        }

        public async Task<ISceneItemFolderApi> GetFolderAsync(string sceneFolderId)
        {
            var retVal = await Obs.SendRequestAsync<SceneItemFolderApi>("getFolder", resourceId, sceneFolderId)
                .ConfigureAwait(false);
            retVal.Obs = Obs;
            return retVal;
        }

        public async Task<IReadOnlyList<ISceneItemFolderApi>> GetFoldersAsync()
        {
            var retVal = await Obs.SendRequestAsync<List<SceneItemFolderApi>>("getFolders", resourceId)
                .ConfigureAwait(false);
            foreach (var entry in retVal) {
                entry.Obs = Obs;
            }

            return retVal;
        }

        public async Task<ISceneItemApi> GetItemAsync(string sceneItemId)
        {
            var retVal = await Obs.SendRequestAsync<SceneItemApi>("getItem", resourceId, sceneItemId)
                .ConfigureAwait(false);
            retVal.Obs = Obs;
            return retVal;
        }

        public async Task<IReadOnlyList<ISceneItemApi>> GetItemsAsync()
        {
            var retVal = await Obs.SendRequestAsync<List<SceneItemApi>>("getItems", resourceId)
                .ConfigureAwait(false);
            foreach (var entry in retVal) {
                entry.Obs = Obs;
            }

            return retVal;
        }

        public async Task<IScene> GetModelAsync()
        {
            return await Obs.SendRequestAsync<Scene>("getModel", resourceId).ConfigureAwait(false);
        }

        public async Task<ISceneNodeApi> GetNodeAsync(string sceneNodeId)
        {
            var retVal = await Obs.SendRequestAsync<SceneNodeApi>("getNode", resourceId, sceneNodeId)
                .ConfigureAwait(false);
            retVal.Obs = Obs;
            return retVal;
        }

        public async Task<ISceneNodeApi> GetNodeByNameAsync(string name)
        {
            var retVal = await Obs.SendRequestAsync<SceneNodeApi>("getNodeByName", resourceId, name)
                .ConfigureAwait(false);
            retVal.Obs = Obs;
            return retVal;
        }

        public async Task<IReadOnlyList<ISceneNodeApi>> GetNodesAsync()
        {
            var retVal = await Obs.SendRequestAsync<List<SceneNodeApi>>("getNodes", resourceId)
                .ConfigureAwait(false);
            foreach (var entry in retVal) {
                entry.Obs = Obs;
            }

            return retVal;
        }

        public async Task<IReadOnlyList<ISceneNodeApi>> GetRootNodesAsync()
        {
            var retVal = await Obs.SendRequestAsync<List<SceneNodeApi>>("getRootNodes", resourceId)
                .ConfigureAwait(false);
            foreach (var entry in retVal) {
                entry.Obs = Obs;
            }

            return retVal;
        }

        public Task MakeActiveAsync()
        {
            return Obs.SendSimpleRequestAsync("makeActive", resourceId);
        }

        public Task RemoveAsync()
        {
            return Obs.SendSimpleRequestAsync("remove", resourceId);
        }

        public Task RemoveFolderAsync(string folderId)
        {
            return Obs.SendSimpleRequestAsync("removeFolder", resourceId, folderId);
        }

        public Task RemoveItemAsync(string sceneItemId)
        {
            return Obs.SendSimpleRequestAsync("removeItem", resourceId, sceneItemId);
        }

        public Task SetNameAsync(string newName)
        {
            return Obs.SendSimpleRequestAsync("setName", resourceId, newName);
        }

        #endregion
    }
}
