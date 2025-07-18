using System;
using System.Collections;
using System.Text;
using interfaces;
using Managers;
using UnityEngine;
using Saving;

namespace Entities
{
    public enum EnemyPointsAmount : int
    {
        MINIMUM_POINTS = 50,
        MIDDLE_ENEMIE_POINTS = MINIMUM_POINTS * 2,
        TOP_ENEMIE_POINTS = MINIMUM_POINTS * 4,

        UFO_SHIP_LOWEST = MINIMUM_POINTS * 5,
        UFO_SHIP_MAXIMUM = UFO_SHIP_LOWEST * 3,
    }

    public sealed class Enemy : MonoBehaviour, ISaveGameData, ILoadGameData
    {
        public const int DEFAULT_ID = -1337;

        public int id = DEFAULT_ID;
        /// <summary>
        /// keeps track of how many enemies are in the scene
        /// </summary>

        public GameStates gameStates;

        public EnemyMovement enemyMovement;

        public EnemyShoot enemyShoot;

        public SpriteRenderer enemySprite;

        public Action<int> onDies;

        public EnemyPointsAmount enemyPointsAmount = EnemyPointsAmount.MINIMUM_POINTS;

        public StandardEntitySaveData standardEntitySaveData;

        [Tooltip("How low can you go before it is game over")]
        public float lowestPointToReach = -3.0f;

        #region GameManagerBoilerPlate

        private void OnEnable()
        {
            SingletonManager.inst.gameManager.subscribe(onStateChange);
            onStateChange(SingletonManager.inst.gameManager.gameState);
        }

        private void OnDisable()
        {
            SingletonManager.inst.gameManager.unSubscribe(onStateChange);
        }

        private void onStateChange(GameStates newState)
        {
            gameStates = newState;
        }

        #endregion

        /// <summary>
        /// TODO : PLAY DEATH ANIMATION WITH THIS FUNCTION
        /// </summary>
        /// <returns></returns>
        public void dies()
        {
            SingletonManager.inst.soundManager.playAudio(Scriptable_Objects.GameAudioType.SFX, "boom");
            StartCoroutine(deathAnmation());
            onDies?.Invoke(id);
        }

        private IEnumerator deathAnmation()
        {
            gameObject.SetActive(false);
            yield return null;
        }

        /// <summary>
        /// 
        /// </summary>
        public void selfAssignComponents()
        {

            if (enemyMovement is null)
            {
                enemyMovement = GetComponent<EnemyMovement>();
                DDebug.Assert(enemyMovement is not null, $"Could not find {typeof(EnemyMovement)} script in {nameof(Enemy)}", this);
            }

            if (enemyShoot is null)
            {
                enemyShoot = GetComponent<EnemyShoot>();
                DDebug.Assert(enemyShoot is not null, $"Could not find {typeof(EnemyShoot)} script in {nameof(Enemy)}", this);
            }

        }

        #region InterfacesImpl

        string ISaveGameData.getSaveData()
        {
            standardEntitySaveData = StandardEntitySaveData.create(transform.position,
               new Vector2(enemyMovement.teleportDistance, 0.0f),
               Vector2.zero,
               transform.gameObject.activeInHierarchy,
               "Enemy");
            return SaveStringifyer.Stringify(this);
        }

        public string getMetaData()
        {
            return JsonUtility.ToJson(new Util.MetaData(nameof(Enemy)));
        }

        public void loadSaveData(string data)
        {
            string[] variables = data.Split(Saving.SavingConstants.DIVIDER);

            int index = 2;
            standardEntitySaveData = StandardEntitySaveData.loadData(variables, ref index);

            transform.gameObject.SetActive(standardEntitySaveData.isActive);
            transform.position = standardEntitySaveData.position;
            enemyMovement.setTeleportDistance(standardEntitySaveData.speed.x);

            gameStates = (GameStates) int.Parse(variables[index]);
            ++index;

            this.enemyMovement.horizontalMin = float.Parse(variables[index]);
            ++index;

            this.enemyMovement.horizontalMax = float.Parse(variables[index]);
            ++index;

            enemyMovement.state = (EnemyMovementState) int.Parse(variables[index]);
            ++index;

            Vector3 spawnPointPos = Vector2.zero;
            spawnPointPos.x = float.Parse(variables[index]);
            ++index;

            spawnPointPos.y = float.Parse(variables[index]);
            ++index;

            spawnPointPos.z = float.Parse(variables[index]);
            ++index;

            enemyShoot.spawnPoint.position = spawnPointPos;

            enemyPointsAmount = (EnemyPointsAmount) int.Parse(variables[index]);

        }

        public void loadData(StandardEntitySaveData data)
        {
            transform.position = data.position;
            enemyMovement.loadData(data);
        }


        #endregion


        void OnControllerColliderHit(ControllerColliderHit hit)
        {
            EDebug.Log("<color=red>Enemy </color>");

        }

    }


    public static partial class SaveStringifyer
    {

        public static string Stringify(Enemy e)
        {
            StringBuilder sb = new();
            sb.Append(SavingConstants.ENEMY_ID);
            sb.Append(SavingConstants.DIVIDER);

            sb.Append(Saving.SaveStringifyer.StringifyEntitySaveData(e.standardEntitySaveData));

            sb.Append((int) e.gameStates);
            sb.Append(SavingConstants.DIVIDER);

            sb.Append(e.enemyMovement.horizontalMin);
            sb.Append(SavingConstants.DIVIDER);

            sb.Append(e.enemyMovement.horizontalMax);
            sb.Append(SavingConstants.DIVIDER);

            sb.Append((int) e.enemyMovement.state);
            sb.Append(SavingConstants.DIVIDER);

            sb.Append(e.enemyShoot.spawnPoint.position.x);
            sb.Append(SavingConstants.DIVIDER);

            sb.Append(e.enemyShoot.spawnPoint.position.y);
            sb.Append(SavingConstants.DIVIDER);

            sb.Append(e.enemyShoot.spawnPoint.position.z);
            sb.Append(SavingConstants.DIVIDER);

            sb.Append((int)e.enemyPointsAmount);
            sb.Append(SavingConstants.DIVIDER);

            sb.Append(SavingConstants.SEGMENT_DIVIDER);

            return sb.ToString();
        }
    }

}
