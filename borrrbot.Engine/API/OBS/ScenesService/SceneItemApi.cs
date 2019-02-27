using System;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace borrrbot.API.OBS.ScenesService
{
    internal sealed class SceneItemApi : SceneNodeApi, ISceneItemApi
    {
        #region Properties

        public string name { get; set; }
        public bool locked { get; set; }
        public int obsSceneItemId { get; set; }
        public string sceneItemId { get; set; }
        public string sourceId { get; set; }

        [JsonConverter(typeof(InterfaceConverter<ITransform, Transform>))]
        public ITransform transform { get; set; }

        public bool visible { get; set; }

        #endregion

        #region ISceneItemActions

        public Task CenterOnScreenAsync()
        {
            var request = new JsonRcRequest("centerOnScreen", typeof(JsonRcResponse<object>));
            request.AddParam("resource", resourceId);
            return Obs.SendRequestAsync<object>(request);
        }

        public Task FitToScreenAsync()
        {
            throw new NotImplementedException();
        }

        public Task FlipXAsync()
        {
            throw new NotImplementedException();
        }

        public Task FlipYAsync()
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync()
        {
            throw new NotImplementedException();
        }

        public Task ResetTransformAsync()
        {
            throw new NotImplementedException();
        }

        public Task RotateAsync(float deg)
        {
            var request = new JsonRcRequest("rotate", typeof(JsonRcResponse<object>));
            request.AddParam("resource", resourceId);
            request.AddParam("args", new[] { deg });
            return Obs.SendRequestAsync<object>(request);
        }

        public Task SetContentCropAsync()
        {
            throw new NotImplementedException();
        }

        public Task SetSettingsAsync(ISceneItemSettings settings)
        {
            throw new NotImplementedException();
        }

        public Task SetTransformAsync(IPartialTransform transform)
        {
            var request = new JsonRcRequest("setTransform", typeof(JsonRcResponse<object>));
            request.AddParam("resource", resourceId);
            request.AddParam("args", new[] { transform });
            return Obs.SendRequestAsync<object>(request);
        }

        public Task SetVisibilityAsync(bool visible)
        {
            var request = new JsonRcRequest("setVisibility", typeof(JsonRcResponse<object>));
            request.AddParam("resource", resourceId);
            request.AddParam("args", new[] { visible });
            return Obs.SendRequestAsync<object>(request);
        }

        public Task StretchToScreenAsync()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
