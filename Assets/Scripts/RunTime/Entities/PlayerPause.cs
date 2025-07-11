using Managers;
using UnityEngine;

namespace Entities
{

    [System.Serializable]
    public sealed class PlayerPause : MonoBehaviour
    {
        public Player referenceToPlayer;

        private void Update()
        {
            if (!SingletonManager.inst.inputManager.isPauseActionPressedThisFrame()) { return; }

            switch (referenceToPlayer.currentGameState)
            {
                case GameStates.PLAYING:
                    SingletonManager.inst.gameManager.setState(GameStates.PAUSE);
                    break;
                case GameStates.PAUSE:
                    SingletonManager.inst.gameManager.setState(GameStates.PLAYING);
                    break;
            }
        }

    }
}
