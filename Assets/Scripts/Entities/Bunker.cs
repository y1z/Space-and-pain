using System.Text;
using interfaces;
using Managers;
using UnityEngine;

namespace Entities
{
    public sealed class Bunker : MonoBehaviour, ISaveGameData, ILoadGameData
    {

        [field: SerializeField] public BunkerBlock[] blocks { get; private set; }

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

            standardEntitySaveData = StandardEntitySaveData.create(transform.position,
                Vector2.zero,
                Vector2.zero,
               _isActive: transform.gameObject.activeInHierarchy,
               _prefabName: "Bunker");

            sb.Append(this.getMetaData());
            sb.AppendLine(SaveManager.DIVIDER);
            sb.Append(JsonUtility.ToJson(this));
            sb.AppendLine(SaveManager.DIVIDER);
            for (int i = 0; i < blocks.Length; i++)
            {
                sb.Append(blocks[i].getMetaData());
                sb.AppendLine(SaveManager.DIVIDER);
                sb.Append(blocks[i].getSaveData());
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
            JsonUtility.FromJsonOverwrite(data, this);
            transform.gameObject.SetActive(standardEntitySaveData.isActive);
            transform.position = standardEntitySaveData.position;
        }

        public void loadData(StandardEntitySaveData data)
        {
            transform.position = data.position;
        }

        #endregion


        public void loadDataForBlock(string data, int index)
        {
            DDebug.Assert(index >= 0 && index < blocks.Length, $"OutSide Index range index=|{index}| in method{nameof(loadDataForBlock)}", this);
            getBlocksIfThereAreNon();
            blocks[index].loadData(data);
        }

        private void getBlocksIfThereAreNon()
        {
            bool areElementsNull = false;
            for (int i = 0; i < blocks.Length; i++) 
            {
                if (blocks[i] is null)
                {
                    areElementsNull = true; 
                    break;
                }
            }

            if (areElementsNull)
            {
                blocks = GetComponentsInChildren<BunkerBlock>();
            }

        }

    }

}
