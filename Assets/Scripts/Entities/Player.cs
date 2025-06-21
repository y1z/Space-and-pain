using System.Collections;
using UnityEngine;
using Managers;


namespace Entities
{

    [SelectionBase]
    public sealed class Player : MonoBehaviour
    {
        public int live { get; set; } = 3;
        public int maxShots { get; set; } = 2;

        public Vector2 startingPosition;

        public PlayerMovement PlayerMovement;

        public PlayerShoot playerShoot;

        public GameStates currentGameState { get; private set; }

        private void Start()
        {
            startingPosition = transform.position;
        }

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

        private void setState(GameStates state)
        {
            currentGameState = state;
        }

        #endregion 

    }

}
