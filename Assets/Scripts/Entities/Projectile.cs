using System.Collections;
using Managers;
using Unity.Profiling;
using UnityEngine;

namespace Entities
{
    [RequireComponent(typeof(CharacterController))]
    public sealed class Projectile : MonoBehaviour
    {

        static readonly ProfilerMarker marker = new ProfilerMarker("MARKER.PROJECTILE");

        [SerializeField] CharacterController cc;
        GameStates gameStates;

        public float speed = 1.0f;
        [SerializeField] private float distanceTraveled = 0.0f;
        [Header("Calculate Speed")]
        [SerializeField] private float maxDistanced = 10.0f;
        [SerializeField, Range(0.01f, 5.0f)] private float timeToReachMaxDistance = 1.0f;

        [Header("Direction to move")]
        public Vector2 direction = Vector2.up;

        /// <summary>
        /// Used to know who's shooting who 
        /// </summary>
        public bool isPlayerProjectile = false;


        private void Awake()
        {
            setUp();
        }

        private void Update()
        {
            if (gameStates != GameStates.PLAYING) { return; }
            using var _ = marker.Auto();

            Vector3 starting = new Vector3(transform.position.x, transform.position.y, transform.position.z);

            cc.Move((speed * Time.deltaTime) * direction);

            Vector3 ending = new Vector3(transform.position.x, transform.position.y, transform.position.z);

            distanceTraveled += (starting - ending).magnitude;

            if (distanceTraveled > maxDistanced)
            {
                distanceTraveled = 0.0f;
                gameObject.SetActive(false);
            }
        }

        void OnControllerColliderHit(ControllerColliderHit hit)
        {
            EDebug.Log($"{nameof(OnControllerColliderHit)}");


            if (isPlayerProjectile && hit.gameObject.TryGetComponent(out Enemy _enemy))
            {
                _enemy.dies();
                setUp();
                gameObject.SetActive(false);
            }

            else if (!isPlayerProjectile && hit.gameObject.TryGetComponent(out Player _player))
            {
                hit.gameObject.GetComponent<Player>().dies();
                setUp();
                gameObject.SetActive(false);
            }

            else if (hit.gameObject.CompareTag("Boundary"))
            {
                setUp();
                gameObject.SetActive(false);
            }

            else if (hit.gameObject.TryGetComponent(out Projectile _projectile))
            {
                StartCoroutine(deathAnimation());
            }

            else if (hit.gameObject.TryGetComponent(out BunkerBlock _bunkerBlock))
            {
                _bunkerBlock.hitBlock();
                gameObject.SetActive(false);
            }
        }

        public void setUp()
        {
            if (cc == null)
            {
                cc = GetComponent<CharacterController>();
                EDebug.Assert(cc != null, $"This script needs and instance of {typeof(CharacterController)} to work", this);
            }
            distanceTraveled = 0.0f;

            speed = maxDistanced / timeToReachMaxDistance;

        }

        public void teleport(Transform _transform)
        {
            teleport(_transform.position);
        }

        public void teleport(Vector3 vector3)
        {
            cc.enabled = false;
            transform.position = vector3;
            cc.enabled = true;
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
