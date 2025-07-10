// Ignore Spelling: Util

using Managers;
using UnityEngine;

namespace Util
{
    public sealed class ChangeGameState : MonoBehaviour
    {
        private GameStates currentGameState;
        [Tooltip("Change this to change the game state ")]
        public GameStates desiredGameState;

        [Tooltip("✅ = change state to  desired Game State.\n 𐄂 = do nothing ")]
        public bool isON = false;

        private void Update()
        {
            if (!isON) { return; }
            if (currentGameState == desiredGameState) { return; }

            SingletonManager.inst.gameManager.setState(desiredGameState);
        }


        #region GameManagerBoilerPlate

        private void OnEnable()
        {
            SingletonManager.inst.gameManager.subscribe(setState);
            setState(SingletonManager.inst.gameManager.gameState);
        }

        private void OnDisable()
        {
            SingletonManager.inst.gameManager.unSubscribe(setState);
        }

        private void setState(GameStates newState)
        {
            currentGameState = newState;
        }

        #endregion
    }

}

