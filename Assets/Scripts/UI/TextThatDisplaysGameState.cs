using System;
using Managers;
using TMPro;
using UnityEngine;

namespace UI
{

    public sealed class TextThatDisplaysGameState : MonoBehaviour
    {
        [Tooltip("The text that will display the current game state")]
        public TextMeshProUGUI text;
        public GameStates gameState;
        private GameStates textGameState = GameStates.GAME_OVER;

        [Range(1.0f, 15.0f),Tooltip("How many times a seconds does this class update")]
        public float updatesPerSecond = 1.0f;


        private void Start()
        {
            if (text == null)
            {
                text = GetComponent<TextMeshProUGUI>();
                EDebug.Assert(text != null, $"This script need a type of {typeof(TextMeshProUGUI)} to work, FIX that.", this);
            }

            InvokeRepeating(nameof(LazyUpdate), updatesPerSecond, 1.0f);
        }

        private void LazyUpdate()
        {
            if (gameState == textGameState) { return; }
            text.text = Utility.StringUtil.addAlignCenterToString("Game State = " + gameState.ToString());
            textGameState = gameState;
        }

        #region GameManagerBoilerPlate
        private void OnEnable()
        {
            SingletonManager.inst.gameManager.subscribe(setState);
        }

        private void OnDisable()
        {
            SingletonManager.inst.gameManager.unSubscribe(setState);
        }

        private void setState(GameStates newState)
        {
            gameState = newState;
        }

        #endregion
    }

}
