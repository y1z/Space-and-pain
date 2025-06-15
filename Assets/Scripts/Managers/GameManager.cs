using System;
using System.Text;
using UnityEngine;

namespace Managers
{
    /// <summary>
    /// Dictates the state of the game and let's other know what the current state is
    /// </summary>

    using Utility;
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

        public void printSubscribers()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var invocator in gameStateSignalSender.GetInvocationList())
            {
                sb.Append("Subscriber=\'");
                sb.Append(invocator.Target);
                sb.Append("\'");
                sb.Append("\n");
                sb.Append("Method name=\'");
                sb.Append(invocator.Method.Name);
                sb.Append("\'");
                sb.Append("\n");
                sb.Append("\t");
            }

            string[] result = sb.ToString().Split('\t');

            for (int i = 0; i < result.Length; i++)
            {
                EDebug.Log(result[i]);
            }

        }

        public void printState()
        {
            Debug.Log(StringUtil.addColorToString($"current State='{gameState}'", Color.green));
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
