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

    public sealed class BunkerBlock : MonoBehaviour
    {
        const int DEFAULT_BLOCK_HEALTH = 2;
        [Tooltip("The collider used for the individual bunker blocks")]
        [SerializeField] public CharacterController blockController;

        public Collider2D col;

        [Tooltip("the sprite used to represent the collider")]
        [SerializeField] public SpriteRenderer blockSprite;

        [SerializeField] private int blockHealth = DEFAULT_BLOCK_HEALTH;

        [Tooltip("Event: for when the block gets hit")]
        public System.Action<int, BlockState, SpriteRenderer> blockHit;

        public BlockState state = BlockState.FULL_HEALTH;

        private void Start()
        {
            /*
            if (blockController == null)
            {
                blockController = GetComponent<CharacterController>();
                EDebug.Assert(blockController != null, $"{blockController} needs an array of type {typeof(CharacterController)} to work", this);
            }*/
            col = GetComponent<Collider2D>();
            EDebug.Assert(col != null, $"{nameof(col)} needs a {typeof(Collider2D)} to work.", this);
            if (blockSprite == null)
            {
                blockSprite = GetComponent<SpriteRenderer>();
                EDebug.Assert(blockSprite != null, $"{blockSprite} needs an array of type {typeof(SpriteRenderer)} to work", this);
            }

        }

        void OnControllerColliderHit(ControllerColliderHit hit)
        {
            EDebug.Log($"{hit}", this);
            if (hit.gameObject.CompareTag("Projectile") || hit.gameObject.CompareTag("Enemy Projectile"))
            {
                blockHealth = blockHealth - 1;
                blockHit?.Invoke(blockHealth, state, blockSprite);
            }

            if (blockHealth < 1)
            {
                gameObject.SetActive(false);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            EDebug.Log($"{nameof(OnCollisionEnter)} entered");

        }

        private void OnCollisionEnter2D(Collision2D collision)
        {

            EDebug.Log($"{nameof(OnCollisionEnter2D)} entered");
        }

    }

}
