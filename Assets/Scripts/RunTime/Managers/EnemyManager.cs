using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Entities;
using Saving;
using interfaces;

namespace Managers
{
    public sealed class EnemyManager : MonoBehaviour, ISaveGameData, ILoadGameData
    {
        const string PATH_TO_PROJECTILE = "Prefabs/Entities/Enemy Projectile";
        const int MAX_PROJECTILES = 3;
        public enum EnemyManagerState
        {
            INIT_ENEMIES,
            MOVE_GROUP_HORIZONTALY,
            MOVE_GROUP_VERTICALLY,
            MOVE_NONE,
        }

        private int currentId = 0;

        [field: SerializeField]
        public GameStates gameStates { get; private set; }

        [field: SerializeField]
        public EnemyManagerState enemyManagerState { get; private set; }

        [Header("Data for shooting")]

        [SerializeField]
        Projectile projectileTemplate;
        [field: SerializeField]
        public List<Projectile> projectiles { get; private set; } = new List<Projectile>(MAX_PROJECTILES);

        [Header("Signals")]

        public Action onInitFinish;

        [Header("enemy data ")]
        public List<Enemy> enemies;
        [field: SerializeField]
        public EnemySpawner[] enemySpawners { get; private set; } = null;

        [field: SerializeField]
        public int aliveEnemiesCount { get; private set; } = 0;

        public bool areInAGroup { get; private set; } = false;

        [Header("enemy controls")]
        [field: SerializeField]
        public float teleportDistance { get; private set; } = 1.0f;

        public float howLongUntilNextMove = 0.1f;

        [field: SerializeField]
        public float currentHowLongUntilNextMove { get; private set; } = 0.0f;

        [field: SerializeField]
        [field: Range(0.0001f, 1.0f), Tooltip("Controls how fast the enemies go down when 1 hits the edge (lower number is faster)")]
        public float moveDownFactorSpeedUp { get; private set; } = 0.3f;

        [field: SerializeField]
        public int enemyToMoveIndex { get; private set; } = 0;

        [Header("Enemy next round controls")]
        [Tooltip("[Lower is faster]How much to speedMultipiler up enemy movement the next round ")]
        public float howLongUntilNextEnemyMovementSpeedUpRate = 0.95f;

        [Tooltip("[Lower is faster]How much to speedMultipiler up the enemy movement when going down next round")]
        public float moveDownFactorSpeedUpSpeedUpRate = 1.0f;

        [field: SerializeField]
        public bool moveEnemiesToTheRight { get; private set; } = false;

        [field: Header("Cool down")]

        [field: SerializeField, Range(0.001f, 1.0f)]
        public float minimumCoolDown { get; private set; } = 1.0f;

        [field: SerializeField, Range(1.0f, 10.0f)]
        public float maximumCoolDown { get; private set; } = 2.0f;

        Util.CoolDownInRange enemyShootCoolDown = null;

        private void Start()
        {
            enemyShootCoolDown = new Util.CoolDownInRange(minimumCoolDown, maximumCoolDown);
            enemyShootCoolDown.onCoolDownReach += randomEnemyShoot;

            projectileTemplate = Resources.Load<Projectile>(PATH_TO_PROJECTILE);

            EDebug.Assert(projectileTemplate != null, $"This scripts expected a prefab at the resources folder =|{PATH_TO_PROJECTILE}| fix that please", this);

            for (int i = 0; i < projectiles.Count; ++i)
            {
                projectiles[i] = projectileTemplate;
                projectiles[i].excludeLayers = 1 << LayerMask.NameToLayer("Enemy");
            }
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

        public void recountHowManyEnemiesAreAlive()
        {
            aliveEnemiesCount = 0;
            for (int i = 0; i < enemies.Count; ++i)
            {
                if (enemies[i].gameObject.activeInHierarchy)
                {
                    aliveEnemiesCount += 1;
                }
            }

        }


        #region ControlsEnemies

        private void moveEnemies()
        {
            if (enemyToMoveIndex < 0) { return; }
            if (enemyToMoveIndex > enemies.Count) { enemyToMoveIndex = 0; }

            bool shouldFindNewIndex = !enemies[enemyToMoveIndex].gameObject.activeInHierarchy;
            if (shouldFindNewIndex)
            {
                enemyToMoveIndex = findNextActiveEnemyIndex(enemyToMoveIndex);
            }

            if (enemyToMoveIndex < 0) { return; }


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
                currentHowLongUntilNextMove -= howLongUntilNextMove;
                moveEnemiesToTheRight = !moveEnemiesToTheRight;
                enemyToMoveIndex = 0;
                return;
            }


            enemyToMoveIndex = (enemyToMoveIndex + 1) % enemies.Count;
            currentHowLongUntilNextMove -= howLongUntilNextMove;
        }

        private void moveEnemiesDown()
        {
            bool shouldFindNewIndex = !enemies[enemyToMoveIndex].gameObject.activeInHierarchy;
            bool searchedAllEnemies = true;
            if (shouldFindNewIndex)
            {
                for (int i = enemyToMoveIndex; i < enemies.Count; ++i)
                {
                    if (enemies[i].gameObject.activeInHierarchy)
                    {
                        searchedAllEnemies = false;
                        enemyToMoveIndex = i;
                        break;
                    }
                }

                if (searchedAllEnemies)
                {
                    enemyManagerState = EnemyManagerState.MOVE_GROUP_HORIZONTALY;

                    enemyToMoveIndex = 0;
                    return;
                }

            }

            enemies[enemyToMoveIndex].enemyMovement.nTeleport(Vector2.down, teleportDistance);

            enemyToMoveIndex++;

            if (enemyToMoveIndex >= enemies.Count)
            {
                enemyManagerState = EnemyManagerState.MOVE_GROUP_HORIZONTALY;

                enemyToMoveIndex = 0;
                return;
            }


        }

        #endregion


        #region Initialize

        private void initalizeEnemies()
        {
            enemySpawners = GameObject.FindObjectsByType<EnemySpawner>(FindObjectsSortMode.None);

            areInAGroup = enemySpawners.Length > 1;

            enemies.Capacity = enemySpawners.Length;
            for (int i = 0; i < enemySpawners.Length; i++)
            {
                enemies.Add(enemySpawners[i].Spawn());
                aliveEnemiesCount++;
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
            for (int i = 0; i < this.projectiles.Count; ++i)
            {
                this.projectiles[i] = Instantiate<Projectile>(projectileTemplate);
                this.projectiles[i].gameObject.transform.position = defaultProjectilPos;
            }


        }



        #endregion

        private void onEnemyDies(int id)
        {
            aliveEnemiesCount--;
            EDebug.Log("Alive Enemies = " + aliveEnemiesCount, this);

            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].id == id)
                {
                    SingletonManager.inst.scoreManager.AddScore(enemies[i].enemyPointsAmount);
                }
            }

            if (aliveEnemiesCount < 1)
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
            SceneManager.activeSceneChanged -= onActiveSceneChanged;
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
            aliveEnemiesCount = 0;
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
            if (!enemies[index].gameObject.activeInHierarchy)
            {
                int activeEnemyIndex = findNextActiveEnemyIndex(index);
                index = activeEnemyIndex;
            }

            if (index < 0) { return; }

            enemies[index].enemyShoot.shoot();
            SingletonManager.inst.soundManager.playAudio(Scriptable_Objects.GameAudioType.SFX, "enemy shoot");
        }


        private int findNextActiveEnemyIndex(int startingIndex)
        {
            int result = -1;
            for (int i = 1; i < enemies.Count; i++)
            {
                int temp = (startingIndex + i) % enemies.Count;

                if (enemies[temp].gameObject.activeInHierarchy)
                {
                    result = temp;
                    break;
                }
            }
            return result;
        }

        public void prepareNextRound()
        {
            recylcleEnemies();
            enemyManagerState = EnemyManagerState.MOVE_GROUP_HORIZONTALY;
            howLongUntilNextMove *= howLongUntilNextEnemyMovementSpeedUpRate;
            moveDownFactorSpeedUp *= moveDownFactorSpeedUpSpeedUpRate;
        }

        private void recylcleEnemies()
        {
            aliveEnemiesCount = 0;
            for (int i = 0; i < enemySpawners.Length; ++i)
            {
                enemies[i].gameObject.SetActive(true);
                enemies[i].transform.position = enemySpawners[i].transform.position;
                aliveEnemiesCount++;
            }


            sortEnemies();

        }

        public void loadSpawnerData(string data, int index)
        {
            DDebug.Assert(index >= 0 && (index < enemySpawners.Length), $"Index outside range = {index}", this);
            enemySpawners[index].loadSaveData(data);
        }

        public void addProjectile(Projectile _projectile)
        {
            DDebug.Assert(!_projectile.isPlayerProjectile, "!_projectile.isPlayerProjectile;// FIX THIS ", this);
            _projectile = projectileTemplate;
            _projectile.direction = projectileTemplate.direction;
            projectiles.Add(_projectile);
        }

        #region interfacesImpl

        public string getSaveData()
        {
            return SaveStringifyer.Stringify(this);
        }

        public string getMetaData()
        {
            throw new NotImplementedException();
        }

        public void loadSaveData(string data)
        {
            int index = 1;
            string[] variables = data.Split(SavingConstants.DIVIDER);

            gameStates = (GameStates) int.Parse(variables[index]);
            ++index;

            enemyManagerState = (EnemyManagerState) int.Parse(variables[index]);
            ++index;

            aliveEnemiesCount = int.Parse(variables[index]);
            ++index;

            areInAGroup = int.Parse(variables[index]) > 0;
            ++index;

            teleportDistance = float.Parse(variables[index]);
            ++index;

            howLongUntilNextMove = float.Parse(variables[index]);
            ++index;

            currentHowLongUntilNextMove = float.Parse(variables[index]);
            ++index;

            moveDownFactorSpeedUp = float.Parse(variables[index]);
            ++index;

            enemyToMoveIndex = int.Parse(variables[index]);
            ++index;

            howLongUntilNextEnemyMovementSpeedUpRate = float.Parse(variables[index]);
            ++index;

            moveDownFactorSpeedUpSpeedUpRate = float.Parse(variables[index]);
            ++index;

            moveEnemiesToTheRight = int.Parse(variables[index]) > 0;
            ++index;

            minimumCoolDown = float.Parse(variables[index]);
            ++index;

            maximumCoolDown = float.Parse(variables[index]);
            ++index;

        }

        public void loadData(StandardEntitySaveData data)
        {
            throw new NotImplementedException();
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

    #region Stringifyer

    public static partial class SaveStringifyer
    {

        public static string Stringify(EnemyManager em)
        {
            StringBuilder sb = new();

            sb.Append(SavingConstants.ENEMY_MANAGER_ID);
            sb.Append(SavingConstants.DIVIDER);

            sb.Append((int) em.gameStates);
            sb.Append(SavingConstants.DIVIDER);

            sb.Append((int) em.enemyManagerState);
            sb.Append(SavingConstants.DIVIDER);

            sb.Append(em.aliveEnemiesCount);
            sb.Append(SavingConstants.DIVIDER);

            sb.Append(em.areInAGroup ? 1 : 0);
            sb.Append(SavingConstants.DIVIDER);

            sb.Append(em.teleportDistance);
            sb.Append(SavingConstants.DIVIDER);

            sb.Append(em.howLongUntilNextMove);
            sb.Append(SavingConstants.DIVIDER);

            sb.Append(em.currentHowLongUntilNextMove);
            sb.Append(SavingConstants.DIVIDER);

            sb.Append(em.moveDownFactorSpeedUp);
            sb.Append(SavingConstants.DIVIDER);

            sb.Append(em.enemyToMoveIndex);
            sb.Append(SavingConstants.DIVIDER);

            sb.Append(em.howLongUntilNextEnemyMovementSpeedUpRate);
            sb.Append(SavingConstants.DIVIDER);

            sb.Append(em.moveDownFactorSpeedUpSpeedUpRate);
            sb.Append(SavingConstants.DIVIDER);

            sb.Append(em.moveEnemiesToTheRight ? 1 : 0);
            sb.Append(SavingConstants.DIVIDER);

            sb.Append(em.minimumCoolDown);
            sb.Append(SavingConstants.DIVIDER);

            sb.Append(em.maximumCoolDown);
            sb.Append(SavingConstants.DIVIDER);
            sb.Append(SavingConstants.SEGMENT_DIVIDER);

            return sb.ToString();
        }
    }

    #endregion

}

