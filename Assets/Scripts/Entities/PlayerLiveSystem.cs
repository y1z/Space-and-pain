// Ignore Spelling: Respawn

using System;
using System.Collections;
using Managers;
using UnityEngine;

namespace Entities
{
    [RequireComponent(typeof(Player))]
    public sealed class PlayerLiveSystem : MonoBehaviour
    {
        public int lives = 3;
        public float timeUntilRespawn = 1.0f;
        public SpriteRenderer spriteRenderer;
        public CharacterController cc;
        public Action onRespawn;
        public Player referenceToPlayer;

        private void Start()
        {
            if (cc == null)
            {
                cc = GetComponent<CharacterController>();
                EDebug.Assert(cc != null, $"This script need a {typeof(CharacterController)} to work", this);
            }

            if (spriteRenderer == null)
            {
                spriteRenderer = GetComponent<SpriteRenderer>();
                EDebug.Assert(spriteRenderer != null, $"{nameof(spriteRenderer)} needs a {typeof(SpriteRenderer)} to work", this);
            }

            if (referenceToPlayer == null)
            {
                referenceToPlayer = GetComponent<Player>();
                EDebug.Assert(referenceToPlayer != null, $"{nameof(referenceToPlayer)} needs a {typeof(Player)} to work", this);
            }

        }

        public IEnumerator dies()
        {
            cc.enabled = false;
            spriteRenderer.gameObject.SetActive(false);
            lives -= 1;
            EDebug.Log($"Live=|{lives}|");
            if (lives < 0)
            {
                SingletonManager.inst.gameManager.setState(GameStates.GAME_OVER);
            }
            yield return new WaitForSeconds(timeUntilRespawn);
            cc.enabled = true;
            spriteRenderer.gameObject.SetActive(true);
            onRespawn?.Invoke();
        }


    }

}
