using System;
using System.Collections;
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

    public sealed class Enemy : MonoBehaviour
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

        #region GameManagerBoilerPlate

        private void OnEnable()
        {
            SingletonManager.inst.gameManager.subscribe(onStateChange);
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
            onDies?.Invoke(id);
            StartCoroutine(deathAnmation());
        }

        private IEnumerator deathAnmation()
        {
            yield return null;
            gameObject.SetActive(false);
        }

    }

}
