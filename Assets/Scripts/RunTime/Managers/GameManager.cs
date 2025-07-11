using System;
using System.Text;
using UnityEngine;

namespace Managers
{
    using interfaces;
    using Saving;

    /// <summary>
    /// Dictates the state of the game and let's other know what the current state is
    /// </summary>

    using Utility;
    [DefaultExecutionOrder(-1)]
    public sealed class GameManager : MonoBehaviour, ISaveGameData, ILoadGameData
    {
        [field: SerializeField]
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

        /// <summary>
        /// This function set the state  to all subscribers of the game manager
        /// </summary>
        /// <param name="state">The new state to be set</param>
        public void setState(GameStates state)
        {
            gameState = state;
            gameStateSignalSender?.Invoke(gameState);
        }

        #region ContextMenuFunctions

        [ContextMenu("Set state to IDLE")]
        public void setStateIdle()
        {
            setState(GameStates.IDLE);
        }

        [ContextMenu("Set state to Pause")]
        public void setStatePause()
        {
            setState(GameStates.PAUSE);
        }

        [ContextMenu("Set state to PLAYING")]
        public void setStatePlaying()
        {
            setState(GameStates.PLAYING);
        }

        [ContextMenu("Set state to GAME_OVER")]
        public void setStateGameOver()
        {
            setState(GameStates.GAME_OVER);
        }

        [ContextMenu("Set state to WON")]
        public void setStateWon()
        {
            setState(GameStates.WON);
        }

        #endregion


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

        #region InterfacesImpl

        public string getSaveData()
        {
            return SaveStringifyer.Stringify(this);
        }

        public string getMetaData()
        {
            return JsonUtility.ToJson(new Util.MetaData(nameof(GameManager)));
        }

        public void loadData(string data)
        {
            JsonUtility.FromJsonOverwrite(data, this);
        }

        public void loadData(StandardEntitySaveData data)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    [System.Serializable]
    public enum GameStates : byte
    {
        IDLE = 0,
        INIT_LEVEL,
        INIT_ENEMYS,
        UI,
        PAUSE,
        PLAYING,
        GAME_OVER,
        WON,
    }

    public static partial class SaveStringifyer
    {

        public static string Stringify(GameManager gm)
        {
            StringBuilder sb = new();
            sb.Append(SavingConstants.GAME_MANAGER_ID);
            sb.Append(SavingConstants.DIVIDER);

            sb.Append((int) gm.gameState);
            sb.Append(SavingConstants.DIVIDER);

            return sb.ToString();
        }
    }
}
