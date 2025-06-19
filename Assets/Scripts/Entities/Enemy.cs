using System.Collections;
using Managers;
using UnityEngine;

namespace Entities
{
    public sealed class Enemy : MonoBehaviour
    {
        public int id;
        /// <summary>
        /// keeps track of how many enemies are in the scene
        /// </summary>
        public int enemyCount;

        public GameStates gameStates;

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
            StartCoroutine(deathAnmation());
        }

        private IEnumerator deathAnmation()
        {
            yield return null;
            gameObject.SetActive(false);
        }

    }

}
