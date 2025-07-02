using System.Collections.Generic;
using UnityEngine;

namespace Generators
{
    using Entities;
    using Scriptable_Objects;

    public sealed class LevelGenerator : MonoBehaviour
    {
        const string PATH_TO_SPAWNER = "Prefabs/Entities/Enemy spawner";
        const string PATH_TO_BUNKER = "Prefabs/Entities/Bunker";
        const string PATH_TO_PLAYER= "Prefabs/Entities/Player";

        [SerializeField] Managers.GameStates gameStates;

        [SerializeField] LevelGeneratorData generatorData = null;

        [Tooltip("[optional] The offset from the coordinate for everything generated (0,0)")]
        [SerializeField] Transform optionalOffsetForGeneration;

        private void Start()
        {
            if (generatorData == null)
            {
                DDebug.LogError($"{nameof(generatorData)} is null and need a {typeof(LevelGeneratorData)} to work", this);
                return;
            }
        }

        #region InitLevel

        private void initLevel()
        {
            DDebug.Assert(generatorData != null, "generatorData == null; FIX THAT", this);
            Vector2 offset = Vector2.zero;
            Vector2 finalPos = generatorData.enemySpawnArea.position;
            if (optionalOffsetForGeneration != null)
            {
                offset = optionalOffsetForGeneration.position;
            }

            createEnemySpawners(offset);
            createBunkers(offset);
            createPlayer(offset);
        }

        private void createEnemySpawners(Vector2 _offset)
        {
            EnemySpawner enemySpawnerTemplate = Resources.Load<EnemySpawner>(PATH_TO_SPAWNER);
            Rect afterOffSet = generatorData.enemySpawnArea;
            afterOffSet.position += _offset;

            float top = afterOffSet.yMin;
            float left = afterOffSet.xMin;
            Vector2 topLeftPos = new Vector2(left, top);

            GameObject spawnersPivot = new GameObject("_Spawners");
            spawnersPivot.transform.position = new Vector3(top, left, 0);

            float horzontalDistance = afterOffSet.width / (float)generatorData.enemiesColumnCount;
            float verticalDistance = afterOffSet.height / (float)generatorData.enemiesRowsCount;


            for (int i = 0; i < generatorData.enemiesRowsCount; ++i)
            {
                for (int j = 0; j < generatorData.enemiesColumnCount; ++j)
                {
                    Vector2 finalPos = topLeftPos + Vector2.right * (horzontalDistance * j) + Vector2.down * (verticalDistance * i);
                    EnemySpawner spawner = Instantiate<EnemySpawner>(enemySpawnerTemplate);
                    spawner.transform.position = finalPos;
                    spawner.transform.SetParent(spawnersPivot.transform);
                }
            }

        }

        private void createBunkers(Vector2 _offset)
        {
            Bunker bunkerTemplate = Resources.Load<Bunker>(PATH_TO_BUNKER);
            Rect afterOffSet = generatorData.bunkerArea;
            afterOffSet.position += _offset;

            float distance_x = afterOffSet.width / (float)generatorData.bunkerAmount;
            Vector2 startPos = new Vector2(afterOffSet.xMin, afterOffSet.center.y);


            GameObject bunkersPivot = new GameObject("_Bunkers");

            for (int i = 0; i < generatorData.bunkerAmount; ++i)
            {
                Bunker bunker = Instantiate<Bunker>(bunkerTemplate);
                bunker.transform.position = startPos + (Vector2.right * (distance_x * i));
                bunker.transform.SetParent(bunkersPivot.transform);
            }

        }

        private void createPlayer(Vector2 _offset)
        {
            Vector3 finalPlayerPosition = generatorData.playerStartPosition + _offset;
            Player pl = Resources.Load<Player>(PATH_TO_PLAYER);
            Player finalPlayer = Instantiate(pl, finalPlayerPosition, Quaternion.identity);
            finalPlayer.playerLiveSystem.setLivesAmount(generatorData.playerLives);
            finalPlayer.playerShoot.setMaxShots(generatorData.playerMaxShots);
        }

        #endregion

        #region GameManagerBoilerPlate

        private void OnEnable()
        {
            Managers.SingletonManager.inst.gameManager.subscribe(setState);
            setState(Managers.SingletonManager.inst.gameManager.gameState);
        }

        private void OnDisable()
        {
            Managers.SingletonManager.inst.gameManager.unSubscribe(setState);
        }

        private void setState(Managers.GameStates newState)
        {
            gameStates = newState;
            switch (gameStates)
            {
                case Managers.GameStates.INIT_LEVEL:
                    initLevel();
                    break;
            }
        }

        #endregion
    }

}
