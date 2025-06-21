using UnityEngine;

namespace Entities
{
    [RequireComponent(typeof(Enemy))]
    public sealed class EnemyShoot : MonoBehaviour
    {
        public Enemy referenceToEnemy;
        [SerializeField] private Transform spawnPoint;

        private void Start()
        {
            if (referenceToEnemy == null)
            {
                gameObject.GetComponent<Enemy>();
                EDebug.Assert(referenceToEnemy != null, $"this script needs a {typeof(Enemy)}", this);
            }
        }
    }
}
