using Entities;
using interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public sealed class ScoreManager : MonoBehaviour, ISaveGameData, ILoadGameData
    {
        [field: SerializeField]
        public int score { get; private set; } = 0;

        public System.Action<int> scoreChange;

        public void AddScore(EnemyPointsAmount points)
        {
            score += (int) points;
            scoreChange?.Invoke(score);
        }

        public void setScore(int newScore)
        {
            score = newScore;
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

        #region InterfacesImpl

        public string getSaveData()
        {
            return SaveStringifyer.Stringify(this);
        }

        public string getMetaData()
        {
            return JsonUtility.ToJson(new Util.MetaData(nameof(ScoreManager)));
        }

        public void loadSaveData(string data)
        {
            string[] variables = data.Split(Saving.SavingConstants.DIVIDER);

            int index = 1;
            int newScore = int.Parse(variables[index]);
            this.setScore(newScore);
            ++index;

        }

        public void loadData(StandardEntitySaveData data)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }

    public static partial class SaveStringifyer
    {

        public static string Stringify(ScoreManager sm)
        {
            System.Text.StringBuilder sb = new();
            sb.Append(Saving.SavingConstants.SCORE_MANAGER_ID);
            sb.Append(Saving.SavingConstants.DIVIDER);

            sb.Append(sm.score);
            sb.Append(Saving.SavingConstants.DIVIDER);
            sb.Append(Saving.SavingConstants.SEGMENT_DIVIDER);


            return sb.ToString();
        }
    }

}
