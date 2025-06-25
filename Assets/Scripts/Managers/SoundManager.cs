using Scriptable_Objects;
using UnityEngine;

namespace Managers
{
    public sealed class SoundManager : MonoBehaviour
    {
        const string SFX_FOLDER = "Scriptable Objects/Audio/SFX";

        [Range(0.0f, 1.0f)] public float sfxVolume = 0.5f;
        [Range(0.0f, 1.0f)] public float musicVolume = 0.5f;

        [Header("Audio sources")]
        [SerializeField] private AudioSource sfxSource;
        [SerializeField] private AudioSource musicSource;

        [Header("Game Audios")]
        [SerializeField] GameAudio[] sfxAudio;
        [SerializeField] GameAudio[] musicAudio;
        [SerializeField] GameAudio lastUsedAudio;

        private void Start()
        {
            if (sfxSource == null)
            {
                sfxSource = GetComponentInChildren<AudioSource>();
                EDebug.Assert(sfxSource != null, $"{nameof(sfxSource)} needs a {typeof(AudioSource)} to work.", this);
            }

            if (musicSource == null)
            {
                musicSource = GetComponentInChildren<AudioSource>();
                EDebug.Assert(musicSource != null, $"{nameof(musicSource)} needs a {typeof(AudioSource)} to work.", this);
            }

            EDebug.Assert(sfxAudio != null, "sfxAudio is null, fix that", this);
            EDebug.Assert(musicAudio != null, "musicAudio is null, fix that", this);

        }


        public void playAudio(GameAudioType audioType, string _audioName)
        {
            bool shouldPlaySFX = audioType == GameAudioType.SFX;
            bool shouldPlayLastUsedAudio = (lastUsedAudio.audioType == audioType && lastUsedAudio.audioName == _audioName);

            sfxSource.volume = sfxVolume;
            musicSource.volume = musicVolume;

            if (shouldPlayLastUsedAudio)
            {
                if (shouldPlaySFX)
                {
                    sfxSource.Play();
                    return;
                }
                musicSource.Play();
                return;
            }

            GameAudio audioToPlay = null;
            audioToPlay = findGameAudio(audioType, _audioName);
            if (audioToPlay == null)
            {
                DDebug.LogError($"Could not find |{_audioName}| of type |{audioType}|", this);
                return;
            }

            switch (audioType)
            {
                case GameAudioType.SFX:
                    sfxSource.clip = audioToPlay.audioClip;
                    sfxSource.Play();
                    break;
                case GameAudioType.MUSIC:
                    musicSource.clip = audioToPlay.audioClip;
                    musicSource.Play();
                    break;
            }


        }

        private void loadAllSFX()
        {
            sfxAudio = Resources.LoadAll<GameAudio>(SFX_FOLDER);
            DDebug.Assert(sfxAudio != null, $"Could not load SFX from folder {SFX_FOLDER} fix that", this);
            
        }

        #region FindFunctions

        private GameAudio findSfxAudio(string _sfxName)
        {
            return findGameAudio(GameAudioType.SFX, _sfxName);
        }

        private GameAudio findMusicAudio(string _musicName)
        {
            return findGameAudio(GameAudioType.MUSIC, _musicName);
        }

        private GameAudio findGameAudio(GameAudioType audioType, string _audioName)
        {
            GameAudio result = null;
            switch (audioType)
            {
                case GameAudioType.SFX:
                    result = findGameAudioSearch(sfxAudio, _audioName);
                    break;

                case GameAudioType.MUSIC:
                    result = findGameAudioSearch(musicAudio, _audioName);
                    break;
            }


            return result;
        }

        private GameAudio findGameAudioSearch(GameAudio[] gameAudios, string _audioName)
        {
            GameAudio result = null;
            for (int i = 0; i < gameAudios.Length; ++i)
            {
                if (gameAudios[i].audioName == _audioName)
                {
                    result = gameAudios[i];
                    break;
                }
            }

            return result;
        }

        #endregion
    }
}
