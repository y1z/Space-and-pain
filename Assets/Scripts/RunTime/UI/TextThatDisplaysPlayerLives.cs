using TMPro;
using UnityEngine;
using Entities;

namespace UI
{
    public sealed class TextThatDisplaysPlayerLives : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI text;
        [SerializeField] Player player; 

        private void Start()
        {
            if (text == null)
            {
                text = GetComponent<TextMeshProUGUI>();
                EDebug.Assert(text != null, $"{nameof(text)} needs a {typeof(TextMeshProUGUI)} to work", this);
            }

            EDebug.Assert(player != null, $"This script needs an a {typeof(Player)} to work ", this);
            player.playerLiveSystem.onLivesChanged += OnLiveChange;
            text.text = "Lives = " + player.playerLiveSystem.lives.ToString();
        }

        private void OnDisable()
        {
            if(player.playerLiveSystem.onLivesChanged != null)
            {
                player.playerLiveSystem.onLivesChanged -= OnLiveChange;
                return;
            }
        }


        private void OnLiveChange(int newLives)
        {
            text.text = $"Lives ={newLives.ToString()}";
        }

    }

}
