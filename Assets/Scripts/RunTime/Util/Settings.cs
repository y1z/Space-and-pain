using UnityEngine;
using UnityEngine.Rendering;

namespace Util
{
    public static class Settings
    {
        public const string MASTER_VOLUME_KEY = "master_volume";
        public const string MUSIC_VOLUME_KEY = "music_volume";
        public const string VOICE_VOLUME_KEY = "voice_volume";
        public const string SFX_VOLUME_KEY = "sfx_volume";
        public const string FULL_SCREEN_KEY = "full_screen";
        public const string GFX_SETTINGS_KEY = "gfx_settings";
        public const string RESOLUTION_WIDTH_SETTINGS_KEY = "resolution_width";
        public const string RESOLUTION_HEIGHT_SETTINGS_KEY = "resolution_height";

        public const float DEFAULT_VOLUME = 0.5F;
        public const float DEFAULT_MASTER_VOLUME = 1.0f;

        public const int DEFAULT_RESOLUTION_WIDTH = 640;
        public const int DEFAULT_RESOLUTION_HEIGHT = 360;

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


            if (!PlayerPrefs.HasKey(RESOLUTION_HEIGHT_SETTINGS_KEY))
            {
                PlayerPrefs.GetInt(RESOLUTION_HEIGHT_SETTINGS_KEY, DEFAULT_RESOLUTION_HEIGHT);
            }

            if (!PlayerPrefs.HasKey(RESOLUTION_WIDTH_SETTINGS_KEY))
            {
                PlayerPrefs.GetInt(RESOLUTION_WIDTH_SETTINGS_KEY, DEFAULT_RESOLUTION_WIDTH);
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
            return PlayerPrefs.GetInt(FULL_SCREEN_KEY, 1) > 0;
        }

        public static Vector2Int getResolution()
        {
            Vector2Int result = Vector2Int.zero;
            result.y = PlayerPrefs.GetInt(RESOLUTION_HEIGHT_SETTINGS_KEY, 360);
            result.x = PlayerPrefs.GetInt(RESOLUTION_WIDTH_SETTINGS_KEY, 640);

            return result;
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

        public static void setResolution(Resolution res)
        {
            PlayerPrefs.SetInt(RESOLUTION_WIDTH_SETTINGS_KEY, res.width);
            PlayerPrefs.SetInt(RESOLUTION_HEIGHT_SETTINGS_KEY, res.height);
        }

    }

}
