using UnityEngine;
using TMPro;
using Entities;

namespace UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public sealed class PlayerLivesDisplay : MonoBehaviour
    {
        private bool hasInitBeenCalled = false;
        [field: SerializeField] public TextMeshProUGUI displayText;
        public RectTransform rectTrans;

        private void Start()
        {
            if (displayText == null)
            {
                displayText = GetComponent<TextMeshProUGUI>();
                DDebug.Assert(displayText != null, $"{displayText} needs a {typeof(TextMeshProUGUI)} to work", this);
            }

            if (!hasInitBeenCalled)
            {
                displayText.text = "Lives = ?";
            }
        }

        /// <summary>
        /// Call this to make the script work
        /// </summary>
        public void Init(Player _player)
        {
            hasInitBeenCalled = true;
            _player.playerLiveSystem.onLivesChanged += OnLivesChange;
            OnLivesChange(_player.playerLiveSystem.lives);
        }

        public void OnLivesChange(int newLivesAmount)
        {
            displayText.text = $"Lives = {newLivesAmount}";
        }

    }
}
