using System.Net.Http.Headers;
using System.Text;
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
    public sealed class BunkerBlock : MonoBehaviour
    {

        const int DEFAULT_BLOCK_HEALTH = 2;
        [Tooltip("The collider used for the individual bunker blocks")]
        [SerializeField]
        public CharacterController blockController;

        [Tooltip("the sprite used to represent the collider")]
        [SerializeField]
        public SpriteRenderer blockSprite;

        [field:SerializeField] 
        public int blockHealth { get; private set; } = DEFAULT_BLOCK_HEALTH;

        public BlockState state = BlockState.FULL_HEALTH;

        public StandardEntitySaveData standardEntitySaveData;

        [Tooltip("Event: for when the block gets hit")]
        public System.Action<int, BlockState, SpriteRenderer> blockHit;


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

    }

}
