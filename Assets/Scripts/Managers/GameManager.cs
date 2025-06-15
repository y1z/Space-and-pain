using System;
using UnityEngine;

namespace Managers
{
    /// <summary>
    /// Dictates the state of the game and let's other know what the current state is
    /// </summary>
    public sealed class GameManager : MonoBehaviour
    {
        public GameStates gameState { get; private set; } = GameStates.IDLE;

        private Action<GameStates> gameStateSignalSender;

        public void subscribe(Action<GameStates> func)
        {
            gameStateSignalSender += func;
        }

        public void unSubscribe(Action<GameStates> func)
        {
            gameStateSignalSender -= func;
        }

        public void setState(GameStates state)
        {
            gameState = state;
            gameStateSignalSender?.Invoke(gameState);
        }




    }

    public enum GameStates : byte
    {
        IDLE = 0,
        PAUSE,
        PLAYING,
        GAME_OVER,
        WON,
    }
}
