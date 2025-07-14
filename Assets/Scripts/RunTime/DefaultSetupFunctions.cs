using UI;

using UnityEngine;

public sealed class DefaultSetupFunctions : MonoBehaviour
{

    public void setUpMasterVolume(SelectableBase masterVolumeSlider)
    {
        if (masterVolumeSlider is SelectableSlider slider)
        {
            slider.setValue(Util.Settings.getMasterVolume());
            slider.drawSliderUnits();
        }
    }


    public void setUpMusicVolume(SelectableBase musicVolumeSlider)
    {
        if (musicVolumeSlider is SelectableSlider slider)
        {
            slider.setValue(Util.Settings.getMusicVolume());
            slider.drawSliderUnits();
        }
    }

    public void setUpSfxVolume(SelectableBase sfxVolumeSlider)
    {
        if (sfxVolumeSlider is SelectableSlider slider)
        {
            slider.setValue(Util.Settings.getSfxVolume());
            slider.drawSliderUnits();
        }
    }

    public void setUpVoiceVolume(SelectableBase voiceVolumeSlider)
    {
        if (voiceVolumeSlider is SelectableSlider slider)
        {
            slider.setValue(Util.Settings.getVoiceVolume());
        }
    }
}
