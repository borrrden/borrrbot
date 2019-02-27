using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace borrrbot.API.OBS.ScenesService
{
    internal abstract class SceneNodeApi : ISceneNodeApi
    {
        #region Properties

        [JsonIgnore]
        public ObsConnection Obs { get; set; }

        public string resourceId { get; set; }
        public IReadOnlyList<string> childrenIds { get; set; }
        public string id { get; set; }

        public string parentId { get; set; }
        public string sceneId { get; set; }
        public string sceneNodeType { get; set; }

        #endregion

        #region ISceneNodeApi

        public void AddToSelection()
        {
            throw new NotImplementedException();
        }

        public void Deselect()
        {
            throw new NotImplementedException();
        }

        public void DetachParent()
        {
            throw new NotImplementedException();
        }

        public int GetItemIndex()
        {
            throw new NotImplementedException();
        }

        public ISceneItemApi GetNextItem()
        {
            throw new NotImplementedException();
        }

        public ISceneItemNode GetNextNode()
        {
            throw new NotImplementedException();
        }

        public ISceneItemNode GetNextSiblingNode()
        {
            throw new NotImplementedException();
        }

        public int GetNodeIndex()
        {
            throw new NotImplementedException();
        }

        public ISceneItemFolderApi GetParent()
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<string> GetPath()
        {
            throw new NotImplementedException();
        }

        public ISceneItemApi GetPrevItem()
        {
            throw new NotImplementedException();
        }

        public ISceneItemNode GetPrevNode()
        {
            throw new NotImplementedException();
        }

        public ISceneItemNode GetPrevSiblingNode()
        {
            throw new NotImplementedException();
        }

        public ISceneApi GetScene()
        {
            throw new NotImplementedException();
        }

        //public ISelection GetSelection()
        //{
        //    throw new NotImplementedException();
        //}

        public bool HasParent()
        {
            throw new NotImplementedException();
        }

        public bool IsFolder()
        {
            throw new NotImplementedException();
        }

        public bool IsItem()
        {
            throw new NotImplementedException();
        }

        public bool IsSelected()
        {
            throw new NotImplementedException();
        }

        public void PlaceAfter(string nodeId)
        {
            throw new NotImplementedException();
        }

        public void PlaceBefore(string nodeId)
        {
            throw new NotImplementedException();
        }

        public void Remove()
        {
            throw new NotImplementedException();
        }

        public void Select()
        {
            throw new NotImplementedException();
        }

        public void SetParent(string parentId)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
