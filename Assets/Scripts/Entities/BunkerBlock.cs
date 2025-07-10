using System.Net.Http.Headers;
using interfaces;
using Managers;
using UnityEngine;

namespace Entities
{
    public enum BlockState
    {
        NONE = 0,
        FULL_HEALTH,
        HALF_HEALTH,
        DEAD,
    }

    [RequireComponent(typeof(CharacterController))]
    public sealed class BunkerBlock : MonoBehaviour, ISaveGameData, ILoadGameData
    {

        const int DEFAULT_BLOCK_HEALTH = 2;
        [Tooltip("The collider used for the individual bunker blocks")]
        [SerializeField] public CharacterController blockController;

        [Tooltip("the sprite used to represent the collider")]
        [SerializeField] public SpriteRenderer blockSprite;

        [SerializeField] private int blockHealth = DEFAULT_BLOCK_HEALTH;

        [Tooltip("Event: for when the block gets hit")]
        public System.Action<int, BlockState, SpriteRenderer> blockHit;

        public BlockState state = BlockState.FULL_HEALTH;

        public StandardEntitySaveData standardEntitySaveData;

        private void Start()
        {
            if (blockController == null)
            {
                blockController = GetComponent<CharacterController>();
                EDebug.Assert(blockController != null, $"{nameof(blockController)} needs a {typeof(CharacterController)} to work.", this);
            }

            if (blockSprite == null)
            {
                blockSprite = GetComponent<SpriteRenderer>();
                EDebug.Assert(blockSprite != null, $"{nameof(blockSprite)} needs a {typeof(SpriteRenderer)} to work.", this);
            }

        }

        public void hitBlock()
        {
            blockHealth = blockHealth - 1;
            blockHit?.Invoke(blockHealth, state, blockSprite);
            SingletonManager.inst.soundManager.playAudio(Scriptable_Objects.GameAudioType.SFX, "boom");
            if (blockHealth < 1)
            {
                gameObject.SetActive(false);
            }
        }

        #region InterfacesImpl

        public string getSaveData()
        {
            standardEntitySaveData = StandardEntitySaveData.create(_position: transform.position,
                _speed: Vector2.zero,
                _direction: Vector2.zero,
                _isActive: transform.gameObject.activeInHierarchy,
                _prefabName: "BunkerBlock");
            return JsonUtility.ToJson(this);
        }

        public string getMetaData()
        {
            return JsonUtility.ToJson(new Util.MetaData(nameof(BunkerBlock)));
        }

        public void loadData(string data)
        {
            JsonUtility.FromJsonOverwrite(data, this);
        }

        public void loadData(StandardEntitySaveData data)
        {
            transform.position = data.position;
        }

        #endregion
    }
}
