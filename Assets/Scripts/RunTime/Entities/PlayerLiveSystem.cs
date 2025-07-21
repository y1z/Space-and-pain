// Ignore Spelling: Respawn
// Ignore Spelling: Respawns

using System;
using System.Collections;
using interfaces;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Entities
{
    [Serializable]
    [RequireComponent(typeof(Player))]
    public sealed class PlayerLiveSystem : MonoBehaviour, ISaveGameData, ILoadGameData
    {
        [field: SerializeField] public int lives { get; private set; } = 3;

        public float timeUntilRespawn = 1.0f;

        public SpriteRenderer spriteRenderer;

        public CharacterController cc;

        [Tooltip("Gets called when the player Respawns ")]
        public Action onRespawn;
        [Tooltip("gets called everything the amount of live the player has changes")]
        public Action<int> onLivesChanged;
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

        public void giveExtraLive(int amount = 1)
        {
            lives += amount;
            onLivesChanged?.Invoke(lives);
        }

        public void setLivesAmount(int amount)
        {
            lives = amount;
            onLivesChanged?.Invoke(lives);
        }

        #region Coroutines
        public IEnumerator dies()
        {
            referenceToPlayer.playerState = Player.PlayerState.DEAD;

            cc.enabled = false;
            spriteRenderer.gameObject.SetActive(false);
            lives -= 1;
            onLivesChanged?.Invoke(lives);

            if (lives < 0)
            {
                SingletonManager.inst.soundManager.playVoice("game over");
                SingletonManager.inst.gameManager.printSubscribers();
                SingletonManager.inst.gameManager.setState(GameStates.GAME_OVER);
                //SceneManager.LoadScene("Scenes/Game/StartScreen");
            }

            yield return new WaitForSeconds(timeUntilRespawn);
            cc.enabled = true;
            spriteRenderer.gameObject.SetActive(true);
            onRespawn?.Invoke();

            referenceToPlayer.playerState = Player.PlayerState.ALIVE;
        }

        #endregion


        #region InterfaceImpl

        public string getSaveData()
        {
            return JsonUtility.ToJson(this);
        }

        public string getMetaData()
        {
            return JsonUtility.ToJson(new Util.MetaData(nameof(PlayerLiveSystem)));
        }

        public void loadSaveData(string data)
        {
            JsonUtility.ToJson(data);
        }

        public void loadData(StandardEntitySaveData data)
        {
            throw new NotImplementedException();
        }

        #endregion

    }

}
