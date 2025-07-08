using System.Collections;
using UnityEngine;
using Managers;
using interfaces;


namespace Entities
{

    [System.Serializable]
    [SelectionBase]
    public sealed class Player : MonoBehaviour, ISaveGameData, ILoadGameData
    {

        [System.Serializable]
        public enum PlayerState
        {
            ALIVE,
            DEAD,
        }
        public Vector2 startingPosition;

        public PlayerState playerState;

        public PlayerMovement playerMovement;

        public PlayerShoot playerShoot;

        public PlayerPause playerPause;

        public PlayerLiveSystem playerLiveSystem;

        public GameStates currentGameState { get; private set; }

        public StandardEntitySaveData standardEntitySaveData;

        private void Start()
        {
            startingPosition = transform.position;
            playerState = PlayerState.ALIVE;
        }

        public void dies()
        {
            SingletonManager.inst.soundManager.playAudio(Scriptable_Objects.GameAudioType.SFX, "boom");
            StartCoroutine(playerLiveSystem.dies());
        }

        /// <summary>
        /// TODO : PLAY DEATH ANIMATION WITH THIS FUNCTION
        /// </summary>
        private IEnumerator deathAnmation()
        {
            yield return null;
            //gameObject.SetActive(false);
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
            currentGameState = state;
        }

        #endregion


        #region IntefacesImpl

        public string getSaveData()
        {
            return $"{{ " +
                $"\"{nameof(StandardEntitySaveData)}\" : {JsonUtility.ToJson(standardEntitySaveData)}, " +
                $"\"{nameof(PlayerState)}\" : {JsonUtility.ToJson(playerState)}, " +
                $"\"{nameof(GameStates)}\" : {JsonUtility.ToJson(currentGameState)}, " +
                $"\"{nameof(PlayerMovement)}\" : {playerMovement.getSaveData()}, " +
                $"\"{nameof(PlayerShoot)}\" : {playerShoot.getSaveData()}, " +
                $"\"{nameof(PlayerLiveSystem)}\" : {playerLiveSystem.getSaveData()}, " +
                $"}}";
        }

        public string getMetaData()
        {
            return JsonUtility.ToJson(new Util.MetaData(nameof(Player)));
        }

        public void loadData(string data)
        {
            JsonUtility.FromJsonOverwrite(data, this);
        }

        public void loadData(StandardEntitySaveData data)
        {
            playerMovement.speed = data.speed.x;
        }

        #endregion

    }

}
