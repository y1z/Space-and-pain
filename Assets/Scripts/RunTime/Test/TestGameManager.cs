using Managers;
using UnityEngine;

namespace Test
{
    public class TestGameManager : MonoBehaviour
    {
        GameStates gameState;
        GameStates[] allGameState = { GameStates.IDLE, GameStates.PAUSE, GameStates.PLAYING, GameStates.GAME_OVER, GameStates.WON };
        int index;

        void Start()
        {
            index = 0;
        }

        #region GameManagerBoilerPlate

        private void OnEnable()
        {
            SingletonManager.inst.gameManager.subscribe(onStateChange);
        }

        private void OnDisable()
        {
            SingletonManager.inst.gameManager.unSubscribe(onStateChange);
        }

        private void onStateChange(GameStates state)
        {
            gameState = state;
        }

        #endregion


        void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                SingletonManager.inst.gameManager.printState();
                SingletonManager.inst.gameManager.setState(allGameState[index]);
                index = (index + 1) % allGameState.Length;
            }

            if (Input.GetKeyDown(KeyCode.O))
            {
                SingletonManager.inst.gameManager.printSubscribers();
            }

        }
    }

}
