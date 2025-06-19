using System.Collections;
using Managers;
using UnityEngine;

namespace Entities
{
    [RequireComponent(typeof(CharacterController))]
    public class Projectile : MonoBehaviour
    {
        [SerializeField] CharacterController cc;
        GameStates gameStates;

        public float speed = 1.0f;
        public Vector2 direction = Vector2.up;

        /// <summary>
        /// Used to know who's shooting who 
        /// </summary>
        public bool isPlayerProjectile = false;


        private void Awake()
        {
            if (cc == null)
            {
                cc = GetComponent<CharacterController>();
                EDebug.Assert(cc != null, $"This script needs and instance of {typeof(CharacterController)} to work", this);
            }
        }

        private void Update()
        {
            if (gameStates != GameStates.PLAYING) { return; }

            cc.Move((speed * Time.deltaTime) * direction);
        }

        void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (hit.gameObject.CompareTag("Projectile"))
            {
                StartCoroutine(deathAnimation());
                return;
            }

            bool isEnemy = hit.gameObject.CompareTag("Enemy");
            if (isEnemy && isPlayerProjectile)
            {
                hit.gameObject.GetComponent<Enemy>().dies();
                return;
            }

            bool isPlayer = hit.gameObject.CompareTag("Player");
            if (isPlayer && !isPlayerProjectile)
            {
                hit.gameObject.GetComponent<Player>().dies();
                return;
            }

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
            gameStates = state;
        }

        #endregion 

        /// <summary>
        /// TODO : USE THIS FUNCTION TO PLAY A "DEATH" ANIMATION FOR THE PROJECTILE
        /// </summary>
        /// <returns></returns>
        private IEnumerator deathAnimation()
        {
            yield return null;
            gameObject.SetActive(false);
        }

    }
}
