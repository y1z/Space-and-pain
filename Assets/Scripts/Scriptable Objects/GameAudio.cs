// Ignore Spelling: Scriptable

using UnityEngine;

namespace Scriptable_Objects
{

    public enum GameAudioType
    {
        NONE = 0,
        SFX,
        MUSIC
    }

    [CreateAssetMenu(fileName = "GameAudio", menuName = "Scriptable Objects/GameAudio")]
    public sealed class GameAudio : ScriptableObject
    {
        [field: SerializeField] public GameAudioType audioType { get; private set; }
        [field: SerializeField] public AudioClip audioClip { get; private set; }
        [field: SerializeField] public string audioName { get; private set; }

        public bool isSameAudioType(GameAudioType _audioType) => audioType == _audioType;
        public bool isSameAudioName(string name) => audioName == name;

        public bool isSameAudioClipName(string name) => audioClip.name == name;

    }

}