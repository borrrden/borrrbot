using System;
using System.Collections.Generic;
using System.Text;

namespace borrrbot.API.OBS.ScenesService
{
    public interface ISceneNodeApi : ISceneItemNode
    {
        void AddToSelection();

        void Deselect();

        void DetachParent();

        int GetItemIndex();

        ISceneItemApi GetNextItem();

        ISceneItemNode GetNextNode();

        ISceneItemNode GetNextSiblingNode();

        int GetNodeIndex();

        ISceneItemFolderApi GetParent();

        IReadOnlyList<string> GetPath();

        ISceneItemApi GetPrevItem();

        ISceneItemNode GetPrevNode();

        ISceneItemNode GetPrevSiblingNode();

        ISceneApi GetScene();

        //ISelection GetSelection();

        bool HasParent();

        bool IsFolder();

        bool IsItem();

        bool IsSelected();

        void PlaceAfter(string nodeId);

        void PlaceBefore(string nodeId);

        void Remove();

        void Select();

        void SetParent(string parentId);
    }
}
