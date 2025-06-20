using UnityEngine;
using System.Collections.Generic;
using Entities;
using System.Collections;

namespace Managers
{
    public sealed class EnemyManager : MonoBehaviour
    {
        public List<Enemy> enemies;
        private int currentId = 0;
        private GameStates gameStates;


        private void initalizeEnemies()
        {
            EnemySpawner[] spawners = GameObject.FindObjectsByType<EnemySpawner>(FindObjectsSortMode.None);
            for (int i = 0; i < spawners.Length; i++)
            {
                enemies.Add(spawners[i].Spawn());
            }

            sortEnemies();

            StartCoroutine(testMoveEnemeies());
        }

        #region GameManagerBoilerPlate

        private void OnEnable()
        {
            SingletonManager.inst.gameManager.subscribe(setState);
            setState(SingletonManager.inst.gameManager.gameState);
        }

        private void OnDisable()
        {
            SingletonManager.inst.gameManager.unSubscribe(setState);
        }

        private void setState(GameStates newStates)
        {
            gameStates = newStates;

            switch (gameStates)
            {
                case GameStates.INIT_ENEMYS:
                    initalizeEnemies();
                    break;
            }
        }

        #endregion

        #region Coroutines

        private IEnumerator testMoveEnemeies()
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                yield return new WaitForSeconds(0.5f);
                enemies[i].enemyMovement.teleportLeft(10.0f);
                enemies[i].enemySprite.color = Color.red;
            }

            yield return null;
        }

        #endregion

        #region EnemySort

        private void sortEnemies()
        {
            sortEnemiesVertically();
            sortEnemiesHorizontaly();
        }

        private void sortEnemiesHorizontaly()
        {
            int completelySorted = 0;
            int safetyVar = 10_000;
            float startingheight = enemies[0].transform.position.y;

            HorizontalEnemyCompare hec = new();

            int index = 0;
            while (completelySorted < enemies.Count)
            {
                for (index = completelySorted; index < enemies.Count; index++)
                {
                    if (enemies[index].transform.position.y > startingheight || enemies[index].transform.position.y < startingheight)
                    {
                        int difference = index - completelySorted;
                        enemies.Sort(completelySorted, difference, hec);
                        completelySorted = index;
                        startingheight = enemies[index].transform.position.y;
                        break;
                    }
                }

                // on the last row
                if (index >= enemies.Count)
                {
                    int difference = index - completelySorted;
                    enemies.Sort(completelySorted, difference, hec);
                    completelySorted = index;
                }


                safetyVar -= 1;
                if (safetyVar < 0)
                {
                    EDebug.LogError($"|{nameof(initalizeEnemies)}| has and infinite loop fix that please", this);
                    break;
                }
            }

        }

        private void sortEnemiesVertically()
        {
            enemies.Sort(new VerticalEnemyCompare());
        }

        #endregion

    }

    #region Comparers
    internal class HorizontalEnemyCompare : Comparer<Enemy>
    {
        public override int Compare(Enemy left, Enemy right)
        {
            bool isLesser = left.gameObject.transform.position.x < right.gameObject.transform.position.x;
            if (isLesser)
            {
                return -1;
            }
            else if (!isLesser)
            {
                return 1;
            }
            return 0;
        }
    }

    internal class VerticalEnemyCompare : Comparer<Enemy>
    {
        public override int Compare(Enemy left, Enemy right)
        {
            return left.gameObject.transform.position.y.CompareTo(right.gameObject.transform.position.y);
        }
    }
    #endregion
}
