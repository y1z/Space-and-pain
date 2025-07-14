using UnityEngine;

namespace Util
{
    public static class Settings
    {
        public const string MASTER_VOLUME_KEY = "master_volume";
        public const string MUSIC_VOLUME_KEY = "music_volume";
        public const string VOICE_VOLUME_KEY = "voice_volume";
        public const string SFX_VOLUME_KEY = "sfx_volume";
        public const string FULL_SCREEN_KEY = "full_screen";

        public const float DEFAULT_VOLUME = 0.5F;
        public const float DEFAULT_MASTER_VOLUME = 1.0f;

        public static void init()
        {
            if (!PlayerPrefs.HasKey(MASTER_VOLUME_KEY))
            {
                PlayerPrefs.SetFloat(MASTER_VOLUME_KEY, DEFAULT_MASTER_VOLUME);
            }

            if (!PlayerPrefs.HasKey(MUSIC_VOLUME_KEY))
            {
                PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, DEFAULT_VOLUME);
            }

            if (!PlayerPrefs.HasKey(VOICE_VOLUME_KEY))
            {
                PlayerPrefs.SetFloat(VOICE_VOLUME_KEY, DEFAULT_VOLUME);
            }

            if (!PlayerPrefs.HasKey(SFX_VOLUME_KEY))
            {
                PlayerPrefs.SetFloat(SFX_VOLUME_KEY, DEFAULT_VOLUME);
            }

            if (!PlayerPrefs.HasKey(FULL_SCREEN_KEY))
            {
                // this is a bool
                PlayerPrefs.SetInt(FULL_SCREEN_KEY, 1);
            }

        }

        public static float getMasterVolume()
        {
            return PlayerPrefs.GetFloat(MASTER_VOLUME_KEY, DEFAULT_VOLUME);
        }

        public static float getMusicVolume()
        {
            return PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, DEFAULT_VOLUME);
        }

        public static float getVoiceVolume()
        {
            return PlayerPrefs.GetFloat(VOICE_VOLUME_KEY, DEFAULT_VOLUME);
        }

        public static float getSfxVolume()
        {
            return PlayerPrefs.GetFloat(SFX_VOLUME_KEY, DEFAULT_VOLUME);
        }

        public static bool getIsFullScreen()
        {
            return PlayerPrefs.GetInt(FULL_SCREEN_KEY) > 0;
        }

        public static void setMasterVolume(float newVolume)
        {
            PlayerPrefs.SetFloat(MASTER_VOLUME_KEY, newVolume);
        }

        public static void setMusicVolume(float newVolume)
        {
            PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, newVolume);
        }

        public static void setVoiceVolume(float newVolume)
        {
            PlayerPrefs.SetFloat(VOICE_VOLUME_KEY, newVolume);
        }

        public static void setSfxVolume(float newVolume)
        {
            PlayerPrefs.SetFloat(SFX_VOLUME_KEY, newVolume);
        }

        public static void setIsFullScreen(bool _isFullScreen)
        {
            PlayerPrefs.SetInt(FULL_SCREEN_KEY, _isFullScreen ? 1 : 0);
        }

    }

}
