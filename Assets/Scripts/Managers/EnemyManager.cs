using UnityEngine;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using Entities;
using System.Collections;
using UnityEngine.Rendering;

namespace Managers
{
    public sealed class EnemyManager : MonoBehaviour
    {
        private int currentId = 0;
        private GameStates gameStates;
        [Header("enemy data ")]
        public List<Enemy> enemies;
        [field: SerializeField] public int aliveEnemies { get; private set; } = 0;
        public bool areInAGroup { get; private set; } = false;

        [Header("enemy controls")]
        [SerializeField] float teleportDistance = 1.0f;
        public float howLongUntilNextMove = 0.1f;
        private float currentHowLongUntilNextMove = 0.0f;
        private int enemyToMoveIndex = 0;
        [SerializeField] private bool moveEnemiesToTheRight = true;

        private void Update()
        {
            if (gameStates != GameStates.PLAYING) { return; }

            if (enemyToMoveIndex > enemies.Count)
            {
                enemyToMoveIndex = enemyToMoveIndex % enemies.Count;
            }

            if (currentHowLongUntilNextMove > howLongUntilNextMove)
            {

                bool shouldFindNewIndex = !enemies[enemyToMoveIndex].gameObject.activeInHierarchy;
                if (shouldFindNewIndex)
                {
                    for (int i = 0; i < enemies.Count; i++)
                    {
                        int possibleNewIndex = (enemyToMoveIndex + i) % enemies.Count;

                        if (enemies[possibleNewIndex].gameObject.activeInHierarchy)
                        {
                            enemyToMoveIndex = possibleNewIndex;
                        }
                    }
                }

                if (moveEnemiesToTheRight)
                {
                    enemies[enemyToMoveIndex].enemyMovement.teleportRight(teleportDistance);
                }
                else
                {
                    enemies[enemyToMoveIndex].enemyMovement.teleportLeft(teleportDistance);
                }

                enemyToMoveIndex = (enemyToMoveIndex + 1) % enemies.Count;
                currentHowLongUntilNextMove = 0.0f;
            }

            currentHowLongUntilNextMove += Time.deltaTime;
        }

        private void initalizeEnemies()
        {
            EnemySpawner[] spawners = GameObject.FindObjectsByType<EnemySpawner>(FindObjectsSortMode.None);

            areInAGroup = spawners.Length > 1;

            enemies.Capacity = spawners.Length;
            for (int i = 0; i < spawners.Length; i++)
            {
                enemies.Add(spawners[i].Spawn());
                aliveEnemies++;
            }

            sortEnemies();

            for (int i = 0; i < enemies.Count; ++i)
            {
                enemies[i].onDies += onEnemyDies;
                enemies[i].id = currentId++;
                enemies[i].enemyMovement.setMovementStateToGroup();
            }

            //StartCoroutine(testMoveEnemeies());
        }

        private void onEnemyDies(int id)
        {
            EDebug.Log($"enemy |{id}| dies");
            int index = enemies.FindIndex(e => e.id == id);
            enemies.RemoveAt(index);
            aliveEnemies--;
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
                // sort then in groups based on how high the enemies are
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
                if (index >= enemies.Count && completelySorted < enemies.Count)
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

    #region ComparerClases
    internal sealed class HorizontalEnemyCompare : Comparer<Enemy>
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

    internal sealed class VerticalEnemyCompare : Comparer<Enemy>
    {
        public override int Compare(Enemy left, Enemy right)
        {
            return left.gameObject.transform.position.y.CompareTo(right.gameObject.transform.position.y);
        }
    }
    #endregion
}
