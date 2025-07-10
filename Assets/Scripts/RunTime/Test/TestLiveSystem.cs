using Entities;
using UnityEngine;

namespace Test
{
    public sealed class TestLiveSystem : MonoBehaviour
    {
        [SerializeField] private Player playerReference;
        public int howManyExtraLivesToGive = 10;
        public int liveToSet = 3;

        private void Start()
        {
            EDebug.Assert(playerReference != null, $"This scripts needs a reference to {typeof(Player)} to work", this);
        }

        [ContextMenu("Give extra life")]
        private void giveExtraLife()
        {
            playerReference.playerLiveSystem.giveExtraLive(howManyExtraLivesToGive);
        }

        [ContextMenu("Kill Player")]
        private void killPlayer()
        {
            playerReference.dies();
        }

        [ContextMenu("live To set")]
        private void setPlayerLives()
        {
            playerReference.playerLiveSystem.setLivesAmount(liveToSet);
        }

    }

}
