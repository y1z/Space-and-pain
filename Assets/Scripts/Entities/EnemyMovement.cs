using interfaces;
using Managers;
using UnityEngine;
using Util;


namespace Entities
{
    internal enum EnemyMovementState
    {
        SINGLE,
        GROUP,
    };

    [RequireComponent(typeof(Enemy))]
    [RequireComponent(typeof(CharacterController))]
    public sealed class EnemyMovement : MonoBehaviour, ISaveGameData, ILoadGameData
    {

        [SerializeField] CharacterController cc;
        [Header("Movement Limits")]
        [Range(0.0f, 10.0f)]
        public float horizontalMax = 8.35f;
        [Range(-10.0f, 0.0f)]
        public float horizontalMin = -8.35f;

        EnemyMovementState state;
        [Header("Movement controls")]
        [SerializeField] Vector2 direction = Vector2.right;
        [field: SerializeField] public float teleportDistance { get; private set; } = .05f;
        [SerializeField] float currentTeleportDistance = 0.05f;
        // Controls how much the enemy moves when going down 
        [SerializeField] float teleportDistanceDown;

        [Header("Controls how the enemy moves when they are the only one left")]
        [SerializeField] float currentTimeUntilNextTeleport = 0.0f;
        [SerializeField] float timeUntilNextTeleport = 0.2f;

        [SerializeField] Enemy referenceToEnemy;

        void Start()
        {
            if (referenceToEnemy == null)
            {
                referenceToEnemy = GetComponent<Enemy>();
                EDebug.Assert(referenceToEnemy != null, $"This script needs a instance of {typeof(Enemy)}", this);
            }

            if (cc == null)
            {
                cc = GetComponent<CharacterController>();
                EDebug.Assert(cc != null, $"This script needs a instance of {typeof(CharacterController)}", this);
            }


            currentTeleportDistance = teleportDistance;
            calculateTeleportDown();
            determineIfEnemyIsInGroup();
        }

        private void Update()
        {
            if (referenceToEnemy.gameStates != GameStates.PLAYING) { return; }
            if (state != EnemyMovementState.SINGLE) { return; }

            if (currentTeleportDistance > teleportDistance || currentTeleportDistance < teleportDistance)
            {
                currentTeleportDistance = teleportDistance;
                calculateTeleportDown();
            }

            switch (state)
            {
                case EnemyMovementState.SINGLE:
                    currentTimeUntilNextTeleport += Time.deltaTime;
                    if (currentTimeUntilNextTeleport > timeUntilNextTeleport)
                    {
                        teleport();
                        currentTimeUntilNextTeleport = 0.0f;
                    }

                    break;
                case EnemyMovementState.GROUP:
                    break;
            }

        }

        #region newTeleport
        public bool nTeleport(Vector2 direction, float distance)
        {
            cc.enabled = false;

            Vector3 delta = new Vector3(direction.x * distance, direction.y * distance, 0.0f);
            transform.position = transform.position + delta;

            if (transform.position.x > horizontalMax)
            {
                cc.enabled = true;
                return false;
            }
            if (transform.position.x < horizontalMin)
            {
                cc.enabled = true;
                return false;
            }

            cc.enabled = true;

            return true;
        }
        #endregion

        public void teleport()
        {
            cc.enabled = false;
            bool isGoingRight = Vector2.Dot(Vector2.right, direction) > 0.5f;
            if (isGoingRight)
            {
                teleportRight(teleportDistance);
                cc.enabled = true;
                return;
            }

            teleportLeft(teleportDistance);

            cc.enabled = true;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="distance"></param>
        /// <returns></returns>
        public bool teleportRight(float distance)
        {
            bool canTeleportRighSide = true;
            // if 16 equal 100% then 9 equals 56.25
            float teleportDownAmount = MathU.RuleOfThreeQuantity(100.0f, distance, 56.25f);

            bool isOutOfBounds = transform.position.x > horizontalMax;

            cc.enabled = false;

            if (isOutOfBounds)
            {
                canTeleportRighSide = false;
                Vector2 finalPos = new Vector2(horizontalMax, transform.position.y);

                finalPos += (Vector2.down * teleportDownAmount);

                direction = Vector2.left;

                transform.position = finalPos;
            }
            else
            {
                Vector2 delta = direction * distance;
                transform.position += new Vector3(delta.x, delta.y, 0.0f);
            }

            cc.enabled = true;
            return canTeleportRighSide;
        }

        public bool teleportLeft(float distance)
        {
            cc.enabled = false;
            bool result = true;
            float teleportDownAmount = MathU.RuleOfThreeQuantity(100.0f, distance, 56.25f);

            if (transform.position.x < horizontalMin)
            {
                result = false;
                Vector2 finalPos = new Vector2(horizontalMin, transform.position.y);

                finalPos += (Vector2.down * teleportDistanceDown);

                direction = Vector2.right;

                transform.position = finalPos;
            }
            else
            {
                Vector2 delta = Vector2.left * distance;
                transform.position += new Vector3(delta.x, delta.y, 0.0f);
            }
            cc.enabled = true;
            return result;
        }

        // TODO : for testing reasons this function will always assume the enemy is going alone for now
        private void determineIfEnemyIsInGroup()
        {
            if (SingletonManager.inst.enemyManager.areInAGroup)
            {
                state = EnemyMovementState.GROUP;
            }
            else
            {
                state = EnemyMovementState.SINGLE;
            }
        }

        private void calculateTeleportDown()
        {
            // rule of 3
            // 100% - 16 (teleportDistance)
            // 56.25% - ? 
            // if 100% is 16 56.25% is 9
            teleportDistanceDown = 56.25f * teleportDistance * 0.01f;
        }

        public void setMovementStateToGroup()
        {
            state = EnemyMovementState.GROUP;
        }

        #region InterfaceImpl

        public string getSaveData()
        {
            return JsonUtility.ToJson(this);
        }

        public string getMetaData()
        {
            return JsonUtility.ToJson(new Util.MetaData(nameof(EnemyMovement)));
        }

        public void loadData(string data)
        {
            JsonUtility.FromJsonOverwrite(data, this);
        }

        public void loadData(StandardEntitySaveData data)
        {
            teleportDistance = data.speed.x;
            calculateTeleportDown();
        }

        #endregion
    }
}
