using System.Collections;
using System.Text;
using Unity.Profiling;
using UnityEngine;

using interfaces;
using Managers;
using Saving;

namespace Entities
{
    [RequireComponent(typeof(CharacterController))]
    public sealed class Projectile : MonoBehaviour, ISaveGameData, ILoadGameData
    {

        static readonly ProfilerMarker marker = new ProfilerMarker("MARKER.PROJECTILE");

        [SerializeField]
        CharacterController cc;

        [SerializeField]
        public GameStates gameStates;

        [SerializeField]
        public float speed = 1.0f;

        [field:SerializeField]
        public float distanceTraveled { get; private set; } = 0.0f;

        [field:Header("Calculate Speed")]
        [field:SerializeField]
        public float maxDistanced { get; private set; } = 10.0f;

        [field:SerializeField, Range(0.01f, 5.0f)]
        public float timeToReachMaxDistance { get; private set; } = 1.0f;

        [Header("Direction to move")]
        public Vector2 direction = Vector2.up;

        /// <summary>
        /// Used to know who's shooting who 
        /// </summary>
        public bool isPlayerProjectile = false;

        public StandardEntitySaveData standardEntitySaveData;


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
                _player.dies();
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

        #region InterfacesImpl

        public string getSaveData()
        {
            standardEntitySaveData = StandardEntitySaveData.create(_position: transform.position,
                _speed: new Vector2(speed, 0.0f),
                _direction: direction,
                _isActive: transform.gameObject.activeInHierarchy,
                "Projectile");
            return SaveStringifyer.Stringify(this);
        }

        public string getMetaData()
        {
            return JsonUtility.ToJson(new Util.MetaData(nameof(Projectile)));
        }

        public void loadSaveData(string data)
        {
            throw new System.NotImplementedException();
        }

        public void loadData(StandardEntitySaveData data)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }


    public static partial class SaveStringifyer
    {

        public static string Stringify(Projectile p)
        {
            StringBuilder sb = new();
            sb.Append(SavingConstants.PROJECTILE_ID);
            sb.Append(SavingConstants.DIVIDER);

            Saving.SaveStringifyer.StringifyEntitySaveData(p.standardEntitySaveData);

            sb.Append((int) p.gameStates);
            sb.Append(SavingConstants.DIVIDER);


            sb.Append(p.speed);
            sb.Append(SavingConstants.DIVIDER);

            sb.Append(p.distanceTraveled);
            sb.Append(SavingConstants.DIVIDER);

            sb.Append(p.maxDistanced);
            sb.Append(SavingConstants.DIVIDER);


            sb.Append(p.timeToReachMaxDistance);
            sb.Append(SavingConstants.DIVIDER);

            sb.Append(p.direction);
            sb.Append(SavingConstants.DIVIDER);


            sb.Append(p.isPlayerProjectile);
            sb.Append(SavingConstants.DIVIDER);
            sb.Append(SavingConstants.SEGMENT_DIVIDER);


            return sb.ToString();
        }
    }
}

