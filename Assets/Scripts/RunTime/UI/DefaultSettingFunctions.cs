using UnityEngine;
using Managers;


namespace UI
{
    public sealed class DefaultSettingFunctions : MonoBehaviour
    {
        public void setMasterVolume(SliderEventData eventData)
        {
            SingletonManager.inst.soundManager.setMasterVolume(eventData.percentOfTurnOnUnits);
        }

        public void setSFXVolume(SliderEventData eventData)
        {
            SingletonManager.inst.soundManager.setVolume(Scriptable_Objects.GameAudioType.SFX, eventData.percentOfTurnOnUnits);
            SingletonManager.inst.soundManager.playSFX("beep");
        }

        public void setMusicVolume(SliderEventData eventData)
        {
            SingletonManager.inst.soundManager.setVolume(Scriptable_Objects.GameAudioType.MUSIC, eventData.percentOfTurnOnUnits);
        }

        public void setVoiceVolume(SliderEventData eventData)
        {
            SingletonManager.inst.soundManager.setVolume(Scriptable_Objects.GameAudioType.VOICE, eventData.percentOfTurnOnUnits);
            SingletonManager.inst.soundManager.playVoice("hello world");
        }

    }

}
