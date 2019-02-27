using System;
using System.Collections.Generic;
using System.Text;

namespace borrrbot.API.OBS.CustomizationService
{
    public sealed class SettingsChangedEventArgs : EventArgs
    {
        ICustomizationSettings Settings { get; }
    }

    public interface ICustomizationServiceApi
    {
        event EventHandler<SettingsChangedEventArgs> SettingsChanged;

        object GetExperimentalSettingsFormData();

        ICustomizationSettings GetSettings();

        object GetSettingsFormData();

        void RestoreDefaults();

        void SetSettings(ICustomizationSettings settings);
    }
}
