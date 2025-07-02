using System.Collections.Generic;
using Managers;
using UnityEngine;

namespace Entities
{

    public sealed class PlayerShoot : MonoBehaviour
    {
        private List<Projectile> projectiles = new();

        [SerializeField] private Player referenceToPlayer;

        [Tooltip("The Projectile instance that all projectiles will be base off")]
        [SerializeField] private Projectile templateInstance;

        [Tooltip("This controls were the projectile will spawn at")]
        [SerializeField] private Transform spawnPoint;

        const string pathToProjectileResource = "Prefabs/Entities/Player Projectile";

        [Tooltip("The max amount of shots the player can have active in the scene")]
        [field: SerializeField] public int maxShots { get; private set; } = 1;


        private void Start()
        {
            templateInstance = Resources.Load<Projectile>(pathToProjectileResource);
            EDebug.Assert(templateInstance != null, $"This script requires that a projectile instance be in the path '{pathToProjectileResource}'", this);
            if (referenceToPlayer == null)
            {
                referenceToPlayer = gameObject.GetComponent<Player>();
                EDebug.Assert(referenceToPlayer != null, $"This script needs a reference to the {typeof(Player)} script", this);
            }

        }

        private void Update()
        {
            if (referenceToPlayer.currentGameState != GameStates.PLAYING) { return; }
            if (referenceToPlayer.playerState != Player.PlayerState.ALIVE) { return; }

            if (SingletonManager.inst.inputManager.isShootActionPressedThisFrame())
            {
                shoot();
            }
        }

        private void shoot()
        {
            int activeProjectile = 0;
            for (int i = 0; i < projectiles.Count; i++)
            {
                activeProjectile = projectiles[i].gameObject.activeInHierarchy ? activeProjectile + 1 : activeProjectile;
            }
            EDebug.Log("Active Projectiles =" + activeProjectile);
            if (activeProjectile >= maxShots) { return; }

            Projectile futureProjectile = recycle();

            if (futureProjectile != null)
            {
                futureProjectile.setUp();
                futureProjectile.teleport(spawnPoint);
                SingletonManager.inst.soundManager.playAudio(Scriptable_Objects.GameAudioType.SFX, "beep");
                return;
            }

            futureProjectile = create();
            futureProjectile.teleport(spawnPoint);
            SingletonManager.inst.soundManager.playAudio(Scriptable_Objects.GameAudioType.SFX, "beep");
            projectiles.Add(futureProjectile);
        }

        private Projectile recycle()
        {
            for (int i = 0; i < projectiles.Count; ++i)
            {
                if (!projectiles[i].gameObject.activeInHierarchy)
                {
                    projectiles[i].gameObject.SetActive(true);
                    return projectiles[i];
                }
            }
            return null;
        }


        private Projectile create()
        {
            Projectile result = Instantiate(templateInstance);

            return result;
        }

        public void setMaxShots(int _maxShots)
        {
            maxShots = Mathf.Clamp(_maxShots, 0, 1337);
        }

    }
}
