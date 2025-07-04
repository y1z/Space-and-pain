using Scriptable_Objects;
using UnityEngine;
using UnityEngine.Audio;

namespace Managers
{
    /// <summary>
    /// TODO : Change SoundManager to be able to play 2 sounds a the same time
    /// </summary>
    public sealed class SoundManager : MonoBehaviour
    {
        public const string MATER_VOLUME_KEY = "master_volume";
        public const string SFX_VOLUME_KEY = "sfx_volume";
        public const string MUSIC_VOLUME_KEY = "music_volume";
        public const string VOICE_VOLUME_KEY = "voice_volume";

        public const float MAX_VOLUME = 10.0f;
        public const float MIN_VOLUME = -80.0f;

        const string SFX_FOLDER = "Scriptable Objects/Audio/SFX";

        [Range(0.0f, 1.0f)] public float sfxVolume = 0.5f;
        [Range(0.0f, 1.0f)] public float musicVolume = 0.5f;
        [Range(0.0f, 1.0f)] public float voiceVolume = 0.5f;

        [Header("Audio mixer")]
        [SerializeField] AudioMixer mixer;

        [Header("Audio sources")]
        [SerializeField] private AudioSource[] sfxSources;
        [Tooltip("this exist so sounds don't cancel each other out")]
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource voiceSource;

        [Header("Game Audios")]
        [SerializeField] GameAudio[] sfxAudio;
        [SerializeField] GameAudio[] musicAudio;
        [SerializeField] GameAudio[] voiceAudio;
        [SerializeField] GameAudio lastUsedAudio;

        private void Start()
        {

            if (mixer == null)
            {
                mixer = GetComponent<AudioMixer>();
                EDebug.Assert(mixer != null, $"{nameof(mixer)} needs a {typeof(AudioMixer)} to work.", this);
            }

            if (sfxSources == null)
            {
                EDebug.Assert(sfxSources != null, $"{nameof(sfxSources)} needs a array of type {typeof(AudioSource)} to work.", this);
            }

            if (musicSource == null)
            {
                musicSource = GetComponentInChildren<AudioSource>();
                EDebug.Assert(musicSource != null, $"{nameof(musicSource)} needs a {typeof(AudioSource)} to work.", this);
            }

            if (voiceSource == null)
            {
                voiceSource = GetComponent<AudioSource>();
                EDebug.Assert(voiceSource != null, $"{nameof(voiceSource)} needs a {typeof(AudioSource)} to work.", this);
            }

            loadAllSFX();

            lastUsedAudio = sfxAudio[0];

            EDebug.Assert(sfxAudio != null, "sfxAudio is null, fix that", this);
            EDebug.Assert(musicAudio != null, "musicAudio is null, fix that", this);

        }


        #region PlayFunctions

        public void playAudio(GameAudioType audioType, string _audioName)
        {

            switch (audioType)
            {
                case GameAudioType.SFX:
                    playSFX(_audioName);
                    break;
                case GameAudioType.MUSIC:
                    playMusic(_audioName);
                    break;
                case GameAudioType.VOICE:
                    playVoice(_audioName);
                    break;

            }

        }

        public void playSFX(string _audioName)
        {
            bool shouldPlayLastUsedAudio = lastUsedAudio.isSameGameAudio(GameAudioType.SFX, _audioName);

            GameAudio audioToPlay = null;
            if (shouldPlayLastUsedAudio)
            {
                audioToPlay = lastUsedAudio;
            }
            else
            {
                audioToPlay = findGameAudio(GameAudioType.SFX, _audioName);
            }

            lastUsedAudio = audioToPlay;

            for (int i = 0; i < sfxSources.Length; ++i)
            {
                if (!sfxSources[i].isPlaying)
                {
                    sfxSources[i].clip = audioToPlay.audioClip;
                    sfxSources[i].Play();
                    //EDebug.Log($"<color=green> played sfx Source {i}  </color>", this);
                    break;
                }

            }

        }

        public void playMusic(string _audioName, bool looping = false)
        {
            GameAudio audioToPlay = findGameAudio(GameAudioType.MUSIC, _audioName);
            DDebug.Assert(audioToPlay != null, $"Could not find audio |{_audioName}| of type |{GameAudioType.MUSIC}|", this);

            musicSource.loop = looping;
            musicSource.clip = audioToPlay.audioClip;
            musicSource.Play();
        }

        public void playVoice(string _audioName)
        {
            GameAudio audioToPlay = findGameAudio(GameAudioType.VOICE, _audioName);
            DDebug.Assert(audioToPlay != null, $"Could not find audio |{_audioName}| of type |{GameAudioType.VOICE}|", this);

            voiceSource.clip = audioToPlay.audioClip;
            voiceSource.Play();
        }

        #endregion

        public void setMasterVolume(float newVolumePercent)
        {
            mixer.SetFloat(MATER_VOLUME_KEY, Mathf.Lerp(MIN_VOLUME, MAX_VOLUME, newVolumePercent));
        }

        public void setVolume(GameAudioType gameAudioType, float newVolumePercent)
        {
            switch (gameAudioType)
            {
                case GameAudioType.SFX:
                    mixer.SetFloat(SFX_VOLUME_KEY, Mathf.Lerp(MIN_VOLUME, MAX_VOLUME, newVolumePercent));
                    break;
                case GameAudioType.MUSIC:
                    mixer.SetFloat(MUSIC_VOLUME_KEY, Mathf.Lerp(MIN_VOLUME, MAX_VOLUME, newVolumePercent));
                    break;
                case GameAudioType.VOICE:
                    mixer.SetFloat(VOICE_VOLUME_KEY, Mathf.Lerp(MIN_VOLUME, MAX_VOLUME, newVolumePercent));
                    break;
                case GameAudioType.NONE:
                default:
                    EDebug.LogWarning($"un-handled case = {gameAudioType}", this);
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
                case GameAudioType.VOICE:
                    result = findGameAudioSearch(voiceAudio, _audioName);
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
