using Scriptable_Objects;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;

namespace Managers
{
    public sealed class SoundManager : MonoBehaviour
    {
        public const string MATER_VOLUME_KEY = "master_volume";
        public const string SFX_VOLUME_KEY = "sfx_volume";
        public const string MUSIC_VOLUME_KEY = "music_volume";
        public const string VOICE_VOLUME_KEY = "voice_volume";

        const string SFX_FOLDER = "Scriptable Objects/Audio/SFX";

        [Range(0.0f, 1.0f)] public float sfxVolume = 0.5f;
        [Range(0.0f, 1.0f)] public float musicVolume = 0.5f;
        [Range(0.0f, 1.0f)] public float voiceVolume = 0.5f;

        [Header("Audio mixer")]
        [SerializeField] AudioMixer mixer;

        [Header("Audio sources")]
        [SerializeField] private AudioSource sfxSource;
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


        public void playAudio(GameAudioType audioType, string _audioName)
        {
            bool shouldPlayLastUsedAudio = lastUsedAudio.isSameGameAudio(audioType, _audioName);

            if (shouldPlayLastUsedAudio)
            {

                switch (lastUsedAudio.audioType)
                {
                    case GameAudioType.SFX:
                        sfxSource.clip = lastUsedAudio.audioClip;
                        sfxSource.Play();
                        break;
                    case GameAudioType.MUSIC:
                        musicSource.clip = lastUsedAudio.audioClip;
                        musicSource.Play();
                        break;
                    case GameAudioType.VOICE:
                        voiceSource.clip = lastUsedAudio.audioClip;
                        voiceSource.Play();
                        break;
                }
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
                case GameAudioType.VOICE:
                    voiceSource.clip = audioToPlay.audioClip;
                    voiceSource.Play();
                    break;
                case GameAudioType.NONE:
                default:
                    DDebug.LogWarning($"NOT handled case ={audioType}", this);
                    break;
            }


        }

        public void setMasterVolume(float newVolume)
        {
            mixer.SetFloat(MATER_VOLUME_KEY, newVolume);
        }

        public void setVolume(GameAudioType gameAudioType, float newVolume)
        {
            switch (gameAudioType)
            {
                case GameAudioType.SFX:
                    mixer.SetFloat(SFX_VOLUME_KEY, newVolume);
                    break;
                case GameAudioType.MUSIC:
                    mixer.SetFloat(MUSIC_VOLUME_KEY, newVolume);
                    break;
                case GameAudioType.VOICE:
                    mixer.SetFloat(VOICE_VOLUME_KEY, newVolume);
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
