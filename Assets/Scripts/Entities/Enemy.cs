using System;
using System.Collections;
using interfaces;
using Managers;
using UnityEngine;

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

        public SpriteRenderer enemySprite;

        public EnemyShoot enemyShoot;

        public Action<int> onDies;

        public EnemyPointsAmount enemyPointsAmount = EnemyPointsAmount.MINIMUM_POINTS;

        public StandardEntitySaveData standardEntitySaveData;

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

        #region InterfacesImpl

        string ISaveGameData.getSaveData()
        {
            standardEntitySaveData.position = transform.position;
            return $"{{" +
                $"\"{nameof(StandardEntitySaveData)}\" : {JsonUtility.ToJson(standardEntitySaveData)}" +
                $"\"{nameof(EnemyMovement)}\" : {enemyMovement.getSaveData()}" +
                $"}}";
        }

        public string getMetaData()
        {
            return JsonUtility.ToJson(new Util.MetaData(nameof(Enemy)));
        }

        void ILoadGameData.loadData(string data)
        {
            JsonUtility.FromJsonOverwrite(data, this);
        }

        public void loadData(StandardEntitySaveData data)
        {
            transform.position = data.position;
            enemyMovement.loadData(data);
        }


        #endregion

    }

}
