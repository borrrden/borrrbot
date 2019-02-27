using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace borrrbot.API.OBS.ScenesService
{
    public interface ISceneItemActions
    {
        Task CenterOnScreenAsync();

        Task FitToScreenAsync();

        Task FlipXAsync();

        Task FlipYAsync();

        Task RemoveAsync();

        Task ResetTransformAsync();

        Task RotateAsync(float deg);

        Task SetContentCropAsync();

        Task SetSettingsAsync(ISceneItemSettings settings);

        Task SetTransformAsync(IPartialTransform transform);

        Task SetVisibilityAsync(bool visible);

        Task StretchToScreenAsync();
    }
}
