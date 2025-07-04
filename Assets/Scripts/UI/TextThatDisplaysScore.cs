using System.Collections;
using Managers;
using TMPro;
using UnityEngine;

namespace UI
{
    public sealed class TextThatDisplaysScore : MonoBehaviour
    {
        [SerializeField] public TextMeshProUGUI scoreText;
        public RectTransform rectTrans;

        private IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();
            if (scoreText == null)
            {
                scoreText = GetComponent<TextMeshProUGUI>();
                EDebug.Assert(scoreText != null, $"{nameof(scoreText)} needs a {typeof(TextMeshProUGUI)} to work.", this);
            }

            if (rectTrans == null)
            {
                rectTrans = GetComponent<RectTransform>();
                EDebug.Assert(rectTrans != null,$"{rectTrans} needs a {typeof(RectTransform)} to work",this);
            }

            scoreText.text = SingletonManager.inst.scoreManager.scoreString();
            yield break;
        }

        private void OnEnable()
        {
            SingletonManager.inst.scoreManager.scoreChange += OnScoreChange;
        }

        private void OnDisable()
        {
            SingletonManager.inst.scoreManager.scoreChange -= OnScoreChange;
        }

        private void OnScoreChange(int new_score)
        {
            scoreText.text = SingletonManager.inst.scoreManager.scoreString();
        }

    }

}
