using UnityEngine;

namespace Entities
{
    public sealed class Bunker : MonoBehaviour
    {

        [field: SerializeField] BunkerBlock[] blocks;

        [Tooltip("The sprite used for when a block has been hit 0 times")]
        [field: SerializeField] private SpriteRenderer fullHealthSprite;

        [Tooltip("The Sprite used for when a block is at half health")]
        [field: SerializeField] private SpriteRenderer halfHealthSprite;

        private void Awake()
        {
            EDebug.Assert(fullHealthSprite != null, "Script needs the full health sprite to work", this);
            EDebug.Assert(halfHealthSprite != null, "Script needs the halfHealthSprite to work", this);

            blocks = GetComponentsInChildren<BunkerBlock>();
            EDebug.Assert(blocks != null, $"This script requires that {typeof(BunkerBlock)} are the children of this script", this);
        }

        private void Start()
        {
            for (int i = 0; i < blocks.Length; i++)
            {
                blocks[i].blockHit += onBlockHit;
            }

        }

        private void setBlockToHalfHealthSprite(int index)
        {
            if (index > blocks.Length || index < 0)
            {
                EDebug.LogError($"index is out of range ={index}", this);
                return;
            }

            blocks[index].blockSprite = halfHealthSprite;
        }

        private void setBlockToFullHealthSprite(int index)
        {
            if (index > blocks.Length || index < 0)
            {
                EDebug.LogError($"index is out of range ={index}", this);
                return;
            }

            blocks[index].blockSprite = fullHealthSprite;
        }

        private void onBlockHit(int _health, BlockState _blockState, SpriteRenderer _sr)
        {

        }

    }

}
