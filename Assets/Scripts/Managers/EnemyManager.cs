using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Entities;
using UnityEngine.SceneManagement;

namespace Managers
{
    public sealed class EnemyManager : MonoBehaviour
    {
        const string PATH_TO_PROJECTILE = "Prefabs/Entities/Enemy Projectile";
        const int MAX_PROJECTILES = 3;
        internal enum EnemyManagerState
        {
            INIT_ENEMIES,
            MOVE_GROUP_HORIZONTALY,
            MOVE_GROUP_VERTICALLY,
            MOVE_NONE,
        }

        private int currentId = 0;

        private GameStates gameStates;

        private EnemyManagerState enemyManagerState;

        [Header("Data for shooting")]
        [SerializeField] Projectile projectileTemplate;
        [field: SerializeField] public Projectile[] projectiles { get; private set; } = new Projectile[MAX_PROJECTILES];

        [Header("Signals")]
        public Action onInitFinish;

        [Header("enemy data ")]
        public List<Enemy> enemies;

        [field: SerializeField] public int aliveEnemies { get; private set; } = 0;

        public bool areInAGroup { get; private set; } = false;

        [Header("enemy controls")]
        [SerializeField] float teleportDistance = 1.0f;

        public float howLongUntilNextMove = 0.1f;

        private float currentHowLongUntilNextMove = 0.0f;

        [Range(0.0001f, 1.0f), Tooltip("Controls how fast the enemies go down when 1 hits the edge (lower number is faster)")]
        private float moveDownFactorSpeedUp = 0.3f;

        private int enemyToMoveIndex = 0;

        private int enemyToMoveDownIndex = 0;

        [SerializeField] private bool moveEnemiesToTheRight = true;

        [Header("Cool down")]
        [SerializeField, Range(0.001f, 1.0f)] private float minimumCoolDown = 1.0f;
        [SerializeField, Range(1.0f, 10.0f)] private float maximumCoolDown = 2.0f;

        Util.CoolDownInRange enemyShootCoolDown = null;

        private void Start()
        {
            enemyShootCoolDown = new Util.CoolDownInRange(minimumCoolDown, maximumCoolDown);
            enemyShootCoolDown.onCoolDownReach += randomEnemyShoot;

            projectileTemplate = Resources.Load<Projectile>(PATH_TO_PROJECTILE);
            EDebug.Assert(projectileTemplate != null, $"This scripts expected a prefab at the resources folder =|{PATH_TO_PROJECTILE}| fix that please", this);
        }

        private void Update()
        {
            if (gameStates != GameStates.PLAYING) { return; }

            if (enemies.Count < 1) { return; }

            currentHowLongUntilNextMove += Time.deltaTime;

            if (enemyManagerState != EnemyManagerState.MOVE_NONE)
            {
                enemyShootCoolDown.Update(Time.deltaTime);
            }

            if (enemyToMoveIndex > enemies.Count)
            {
                enemyToMoveIndex = enemyToMoveIndex % enemies.Count;
            }

            switch (enemyManagerState)
            {
                case EnemyManagerState.MOVE_GROUP_HORIZONTALY:
                    if (!(currentHowLongUntilNextMove > howLongUntilNextMove)) { return; }
                    moveEnemies();
                    break;
                case EnemyManagerState.MOVE_GROUP_VERTICALLY:
                    if (!(currentHowLongUntilNextMove > (howLongUntilNextMove * moveDownFactorSpeedUp))) { return; }
                    moveEnemiesDown();
                    break;
            }

        }


        private void OnDestroy()
        {
            if (enemyShootCoolDown.onCoolDownReach != null)
            {
                enemyShootCoolDown.onCoolDownReach -= randomEnemyShoot;
            }
        }


        #region ControlsEnemies

        private void moveEnemies()
        {
            enemyToMoveIndex = enemyToMoveIndex % enemies.Count;
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

            bool canTeleport = false;
            if (moveEnemiesToTheRight)
            {
                canTeleport = enemies[enemyToMoveIndex].enemyMovement.nTeleport(Vector2.right, teleportDistance);
            }
            else
            {
                canTeleport = enemies[enemyToMoveIndex].enemyMovement.nTeleport(Vector2.left, teleportDistance);
            }

            if (!canTeleport)
            {
                enemyManagerState = EnemyManagerState.MOVE_GROUP_VERTICALLY;
                enemyToMoveIndex = 0;
                currentHowLongUntilNextMove = 0.0f;
                moveEnemiesToTheRight = !moveEnemiesToTheRight;
                enemyToMoveDownIndex = 0;
                return;
            }


            enemyToMoveIndex = (enemyToMoveIndex + 1) % enemies.Count;
            currentHowLongUntilNextMove = 0.0f;
        }

        private void moveEnemiesDown()
        {
            enemyToMoveIndex = enemyToMoveIndex % enemies.Count;
            currentHowLongUntilNextMove = 0.0f;
            bool shouldFindNewIndex = !enemies[enemyToMoveDownIndex].gameObject.activeInHierarchy;
            if (shouldFindNewIndex)
            {
                for (int i = 0; i < enemies.Count; i++)
                {
                    int possibleNewIndex = (enemyToMoveDownIndex + i) % enemies.Count;

                    if (enemies[possibleNewIndex].gameObject.activeInHierarchy)
                    {
                        enemyToMoveDownIndex = possibleNewIndex;
                    }
                }
            }

            enemies[enemyToMoveDownIndex].enemyMovement.nTeleport(Vector2.down, teleportDistance);

            if (enemyToMoveDownIndex + 1 >= enemies.Count)
            {
                enemyManagerState = EnemyManagerState.MOVE_GROUP_HORIZONTALY;
                return;
            }

            enemyToMoveDownIndex = (enemyToMoveDownIndex + 1) % enemies.Count;
        }

        #endregion


        #region Initialize
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

            enemyManagerState = EnemyManagerState.MOVE_GROUP_HORIZONTALY;

            initalizeProjectile();

            onInitFinish?.Invoke();
            //StartCoroutine(testMoveEnemeies());
        }

        private void initalizeProjectile()
        {
            Vector3 defaultProjectilPos = new Vector3(-1337.0f, -1337.0f);
            for (int i = 0; i < this.projectiles.Length; ++i)
            {
                this.projectiles[i] = Instantiate<Projectile>(projectileTemplate);
                this.projectiles[i].gameObject.transform.position = defaultProjectilPos;
            }


        }

        #endregion

        private void onEnemyDies(int id)
        {
            EDebug.Log($"enemy |{id}| dies");
            int index = enemies.FindIndex(e => e.id == id);
            enemies.RemoveAt(index);
            aliveEnemies--;
            if (aliveEnemies < 1)
            {
                enemyManagerState = EnemyManagerState.MOVE_NONE;
                SingletonManager.inst.gameManager.setState(GameStates.WON);
            }
        }

        #region GameManagerAndSceneManagerBoilerPlate

        private void OnEnable()
        {
            SingletonManager.inst.gameManager.subscribe(setState);
            setState(SingletonManager.inst.gameManager.gameState);
            SceneManager.activeSceneChanged += onActiveSceneChanged;
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
                    enemyManagerState = EnemyManagerState.INIT_ENEMIES;
                    initalizeEnemies();
                    break;
            }
        }

        private void onActiveSceneChanged(Scene current, Scene next)
        {
            enemies.Clear();
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


        private void randomEnemyShoot()
        {
            if (enemies.Count < 1) { return; }

            int index = UnityEngine.Random.Range(0, enemies.Count);
            enemies[index].enemyShoot.shoot();
        }
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
