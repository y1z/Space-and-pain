using Entities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public sealed class ScoreManager : MonoBehaviour
    {
        public int score { get; private set; } = 0;

        public System.Action<int> scoreChange;

        public void AddScore(EnemyPointsAmount points)
        {
            score += (int)points;
            scoreChange?.Invoke(score);
        }

        public void resetScore()
        {
            score = 0;
        }

        public string scoreString()
        {
            return $"Score x {score}";
        }

        #region SceneManagerBoilerPlate
        private void OnEnable()
        {
            SceneManager.activeSceneChanged += onActiveSceneChange;

        }

        private void OnDisable()
        {
            SceneManager.activeSceneChanged -= onActiveSceneChange;
            if (scoreChange != null)
            {
                var invokeList = scoreChange.GetInvocationList();
                int invokeListLenght = invokeList.Length;

                if (invokeListLenght < 1)
                {
                    return;
                }

                for (int i = invokeList.Length - 1; i >= 0; i--)
                {
                    System.Delegate.Remove(scoreChange, invokeList[i]);
                }
            }
        }

        private void onActiveSceneChange(Scene prev, Scene next)
        {
            resetScore();
        }
        #endregion
    }

}
