using UnityEngine;

namespace Entities
{


    public sealed class PlayerShoot : MonoBehaviour
    {
        [SerializeField] private Projectile[] projectiles;
        /// <summary>
        /// The Projectile instance that all projectiles will be base off 
        /// </summary>
        [SerializeField] private Projectile templateInace;

        [SerializeField] private Player referenceToPlayer;

        const string pathToProjectileResource = "Prefabs/Entities/Player Projectile";

        private void Start()
        {
            templateInace = Resources.Load<Projectile>(pathToProjectileResource);
            EDebug.Assert(templateInace != null, $"This script requires that a projectile instance be in the path '{pathToProjectileResource}'", this);
            if (referenceToPlayer == null)
            {
                referenceToPlayer = gameObject.GetComponent<Player>();
                EDebug.Assert(referenceToPlayer != null, $"This script needs a reference to the {typeof(Player)} script", this);
            }

        }

        private void Update()
        {
            if (referenceToPlayer.currentGameState != Managers.GameStates.PLAYING) { return; }

        }
    }
}
