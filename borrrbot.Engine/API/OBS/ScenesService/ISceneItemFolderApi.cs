// 
//  ISceneItemFolderApi.cs
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

namespace borrrbot.API.OBS.ScenesService
{
    /// <summary>
    /// An interface representing an editable folder in an OBS scene
    /// </summary>
    public interface ISceneItemFolderApi : ISceneNodeApi
    {
        #region Properties

        /// <summary>
        /// Gets the name of the folder
        /// </summary>
        string name { get; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds the folder to the scene with the given ID
        /// </summary>
        /// <param name="sceneNodeId">The ID of the scene to add the folder to</param>
        /// <returns>An awaitable task representing the method completion</returns>
        Task AddAsync(string sceneNodeId);

        /// <summary>
        /// Gets a list of folders that are directly inside the folder
        /// </summary>
        /// <returns>The list of folders inside</returns>
        Task<IReadOnlyList<ISceneItemFolderApi>> GetFoldersAsync();

        /// <summary>
        /// Gets a list of items that are directly inside the folder
        /// </summary>
        /// <returns>The list of items inside</returns>
        Task<IReadOnlyList<ISceneItemApi>> GetItemsAsync();

        /// <summary>
        /// Gets a model representing this folder in the case of
        /// an abbreviated response from OBS (not usually used)
        /// </summary>
        /// <returns>The model with the details of the folder</returns>
        Task<ISceneItemFolder> GetModelAsync();

        /// <summary>
        /// Gets all subfolders of the folder recursively
        /// </summary>
        /// <returns>The list of folders inside the folder</returns>
        Task<IReadOnlyList<ISceneItemFolderApi>> GetNestedFoldersAsync();

        /// <summary>
        /// Gets the IDs of all the subfolders of the folder recursively
        /// </summary>
        /// <returns>A list of folder IDs</returns>
        Task<IReadOnlyList<string>> GetNestedFoldersIdsAsync();

        /// <summary>
        /// Gets all subitems of the folder recursively
        /// </summary>
        /// <returns>A list of items for editing</returns>
        Task<IReadOnlyList<ISceneItemApi>> GetNestedItemsAsync();

        /// <summary>
        /// Gets the IDs of all the subitems of the folder recursively
        /// </summary>
        /// <returns>A list of item IDs</returns>
        Task<IReadOnlyList<string>> GetNestedItemsIdsAsync();

        /// <summary>
        /// Gets all subnodes of the folder recursively
        /// </summary>
        /// <returns>A list of nodes for editing</returns>
        Task<IReadOnlyList<ISceneNodeApi>> GetNestedNodesAsync();

        /// <summary>
        /// Gets the IDs of all the subnodes of the folder recursively
        /// </summary>
        /// <returns>A list of item IDs</returns>
        Task<IReadOnlyList<string>> GetNestedNodesIdsAsync();

        /// <summary>
        /// Gets a list of nodes that are directly inside the folder
        /// </summary>
        /// <returns>The list of nodes inside</returns>
        Task<IReadOnlyList<ISceneNodeApi>> GetNodesAsync();

        /// <summary>
        /// Sets the name of the folder to a new value
        /// </summary>
        /// <param name="newName">The new name for the folder</param>
        /// <returns>An awaitable task representing the method completion</returns>
        Task SetNameAsync(string newName);

        /// <summary>
        /// Deletes the folder and moves its contents up one level
        /// </summary>
        /// <returns>An awaitable task representing the method completion</returns>
        Task UngroupAsync();

        #endregion
    }

    internal sealed class SceneItemFolderApi : SceneNodeApi, ISceneItemFolderApi
    {
        #region Properties

        public string name { get; set; }

        #endregion

        #region ISceneItemFolderApi

        public Task AddAsync(string sceneNodeId)
        {
            return Obs.SendSimpleRequestAsync("add", resourceId, sceneNodeId);
        }

        public async Task<IReadOnlyList<ISceneItemFolderApi>> GetFoldersAsync()
        {
            var retVal = await Obs.SendRequestAsync<List<SceneItemFolderApi>>("getFolders", resourceId)
                .ConfigureAwait(false);
            foreach (var item in retVal) {
                item.Obs = Obs;
            }

            return retVal;
        }

        public async Task<IReadOnlyList<ISceneItemApi>> GetItemsAsync()
        {
            var retVal = await Obs.SendRequestAsync<List<SceneItemApi>>("getItems", resourceId)
                .ConfigureAwait(false);
            foreach (var item in retVal) {
                item.Obs = Obs;
            }

            return retVal;
        }

        public Task<ISceneItemFolder> GetModelAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyList<ISceneItemFolderApi>> GetNestedFoldersAsync()
        {
            var retVal = await Obs.SendRequestAsync<List<SceneItemFolderApi>>("getNestedFolders", resourceId)
                .ConfigureAwait(false);
            foreach (var item in retVal) {
                item.Obs = Obs;
            }

            return retVal;
        }

        public async Task<IReadOnlyList<string>> GetNestedFoldersIdsAsync()
        {
            return await Obs.SendRequestAsync<List<string>>("getNestedFoldersIds", resourceId)
                .ConfigureAwait(false);
        }

        public async Task<IReadOnlyList<ISceneItemApi>> GetNestedItemsAsync()
        {
            var retVal = await Obs.SendRequestAsync<List<SceneItemApi>>("getNestedItems", resourceId)
                .ConfigureAwait(false);
            foreach (var item in retVal) {
                item.Obs = Obs;
            }

            return retVal;
        }

        public async Task<IReadOnlyList<string>> GetNestedItemsIdsAsync()
        {
            return await Obs.SendRequestAsync<List<string>>("getNestedItemsIds", resourceId)
                .ConfigureAwait(false);
        }

        public async Task<IReadOnlyList<ISceneNodeApi>> GetNestedNodesAsync()
        {
            var retVal = await Obs.SendRequestAsync<List<SceneNodeApi>>("getNestedNodes", resourceId)
                .ConfigureAwait(false);
            foreach (var item in retVal) {
                item.Obs = Obs;
            }

            return retVal;
        }

        public async Task<IReadOnlyList<string>> GetNestedNodesIdsAsync()
        {
            return await Obs.SendRequestAsync<List<string>>("getNestedNodesIds", resourceId)
                .ConfigureAwait(false);
        }

        public async Task<IReadOnlyList<ISceneNodeApi>> GetNodesAsync()
        {
            var retVal = await Obs.SendRequestAsync<List<SceneNodeApi>>("getNodes", resourceId)
                .ConfigureAwait(false);
            foreach (var item in retVal) {
                item.Obs = Obs;
            }

            return retVal;
        }

        public Task SetNameAsync(string newName)
        {
            return Obs.SendSimpleRequestAsync("setName", resourceId, newName);
        }

        public Task UngroupAsync()
        {
            return Obs.SendSimpleRequestAsync("ungroup", resourceId);
        }

        #endregion
    }
}
