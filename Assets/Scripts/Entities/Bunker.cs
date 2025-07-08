using System.Text;
using interfaces;
using Managers;
using UnityEngine;

namespace Entities
{
    public sealed class Bunker : MonoBehaviour, ISaveGameData, ILoadGameData
    {

        [field: SerializeField] BunkerBlock[] blocks;

        [Tooltip("The sprite used for when a block has been hit 0 times")]
        [field: SerializeField] private SpriteRenderer fullHealthSprite;

        [Tooltip("The Sprite used for when a block is at half health")]
        [field: SerializeField] private SpriteRenderer halfHealthSprite;

        public StandardEntitySaveData standardEntitySaveData;

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

            switch (_blockState)
            {
                case BlockState.FULL_HEALTH:
                    _sr.sprite = halfHealthSprite.sprite;
                    break;
                case BlockState.HALF_HEALTH:
                    Color finalColor = _sr.color;
                    finalColor.a = 0;
                    _sr.color = finalColor;
                    break;
            }

        }

        #region InterfacesImpl

        public string getSaveData()
        {
            StringBuilder sb = new();
            standardEntitySaveData.position = transform.position;

            sb.Append(JsonUtility.ToJson(this));
            sb.Append(SaveManager.DIVIDER);
            for (int i = 0; i < blocks.Length; i++)
            {
                sb.AppendLine(blocks[i].getSaveData());
                sb.AppendLine(SaveManager.DIVIDER);
            }

            return sb.ToString();
        }

        public string getMetaData()
        {
            return JsonUtility.ToJson(new Util.MetaData(nameof(Bunker)));
        }

        public void loadData(string data)
        {
            throw new System.NotImplementedException();
        }

        public void loadData(StandardEntitySaveData data)
        {
            transform.position = data.position;
        }

        #endregion
    }

}
