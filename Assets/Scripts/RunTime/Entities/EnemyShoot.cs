using Managers;
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

        public void shoot()
        {
            Projectile[] projectile = SingletonManager.inst.enemyManager.projectiles;

            for (int i = 0; i < projectile.Length; i++)
            {
                if (!projectile[i].gameObject.activeInHierarchy)
                {
                    projectile[i].gameObject.SetActive(true);
                    projectile[i].setUp();
                    projectile[i].teleport(spawnPoint);
                }
            }

        }
    }
}
