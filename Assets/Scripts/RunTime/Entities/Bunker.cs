using System.Text;
using interfaces;
using UnityEngine;
using Saving;

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

        private void spriteSwitch(BunkerBlock bb)
        {
            switch (bb.state)
            {
                case BlockState.FULL_HEALTH:
                    bb.blockSprite.sprite = fullHealthSprite.sprite;
                    break;
                case BlockState.HALF_HEALTH:
                    bb.blockSprite.sprite = halfHealthSprite.sprite;
                    break;
            }

        }

        #region InterfacesImpl

        public string getSaveData()
        {
            standardEntitySaveData = StandardEntitySaveData.create(transform.position,
                Vector2.zero,
                Vector2.zero,
               _isActive: transform.gameObject.activeInHierarchy,
               _prefabName: "Bunker");

            return SaveStringifyer.Stringify(this);
        }

        public string getMetaData()
        {
            return JsonUtility.ToJson(new Util.MetaData(nameof(Bunker)));
        }

        public void loadSaveData(string data)
        {
            string[] variables = data.Split(Saving.SavingConstants.DIVIDER);

            int index = 2;
            standardEntitySaveData = StandardEntitySaveData.loadData(variables, ref index);
            transform.gameObject.SetActive(standardEntitySaveData.isActive);
            transform.position = standardEntitySaveData.position;

            int len = int.Parse(variables[index]);
            ++index;

            for (int i = 0; i < len; ++i)
            {
                // skipping identifier
                ++index;

                blocks[i].standardEntitySaveData = StandardEntitySaveData.loadData(variables, ref index);

                blocks[i].setSelfEntitySaveData();

                blocks[i].setBlockHealth(int.Parse(variables[index]));
                ++index;

                blocks[i].state = (BlockState) int.Parse(variables[index]);
                ++index;

                this.spriteSwitch(blocks[i]);
            }

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
            //blocks[index].loadSaveData(data);
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

    public static partial class SaveStringifyer
    {
        public static string Stringify(Bunker b)
        {
            StringBuilder sb = new();

            sb.Append(SavingConstants.BUNKER_ID);
            sb.Append(SavingConstants.DIVIDER);

            sb.Append(Saving.SaveStringifyer.StringifyEntitySaveData(b.standardEntitySaveData));

            sb.Append(b.blocks.Length);
            sb.Append(SavingConstants.DIVIDER);

            for (int i = 0; i < b.blocks.Length; ++i)
            {
                b.blocks[i].updateEntitySaveData();
                sb.Append(Saving.SaveStringifyer.StringifyEntitySaveData(b.blocks[i].standardEntitySaveData));

                sb.Append(b.blocks[i].blockHealth);
                sb.Append(SavingConstants.DIVIDER);

                sb.Append((int) b.blocks[i].state);
                sb.Append(SavingConstants.DIVIDER);
            }


            sb.Append(SavingConstants.SEGMENT_DIVIDER);

            return sb.ToString();
        }
    }

}
