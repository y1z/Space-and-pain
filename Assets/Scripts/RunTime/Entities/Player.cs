// Ignore Spelling: Teleport

using System.Collections;
using UnityEngine;
using Managers;
using interfaces;
using System.Text;
using Saving;


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

        /// <summary>
        /// This function only exist because when loading a different scene and then coming back 
        /// the variables playerMovement, playerShoot, playerPause and playerLiveSystem
        /// are Null for some reason
        /// </summary>
        public void selfAssignComponents()
        {
            if (playerMovement is null)
            {
                playerMovement = GetComponent<PlayerMovement>();
                DDebug.Assert(playerMovement is not null, $"Could not find {typeof(PlayerMovement)} script in {nameof(Player)}", this);
            }

            if (playerShoot is null)
            {
                playerShoot = GetComponent<PlayerShoot>();
                DDebug.Assert(playerShoot is not null, $"Could not find {typeof(PlayerShoot)} script in {nameof(Player)}", this);

            }

            if (playerPause is null)
            {
                playerPause = GetComponent<PlayerPause>();
                DDebug.Assert(playerPause is not null, $"Could not find {typeof(PlayerPause)} script in {nameof(Player)}", this);
            }

            if (playerLiveSystem is null)
            {
                playerLiveSystem = GetComponent<PlayerLiveSystem>();
                DDebug.Assert(playerLiveSystem is not null, $"Could not find {typeof(PlayerLiveSystem)} script in {nameof(Player)}", this);
            }

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
            standardEntitySaveData = StandardEntitySaveData.create(_position: transform.position,
                _speed: new Vector2(playerMovement.speed, 0.0f),
                _direction: playerMovement.dir,
               _isActive: transform.gameObject.activeInHierarchy,
               _prefabName: "Player");
            return SaveStringifyer.Stringify(this); // JsonUtility.ToJson(this);
        }

        public string getMetaData()
        {
            return JsonUtility.ToJson(new Util.MetaData(nameof(Player)));
        }

        public void loadData(string data)
        {
            JsonUtility.FromJsonOverwrite(data, this);
            selfAssignComponents();
            playerMovement.speed = standardEntitySaveData.speed.x;
            playerMovement.setDir(standardEntitySaveData.direction);
        }

        public void loadData(StandardEntitySaveData data)
        {
            playerMovement.speed = data.speed.x;
            playerMovement.teleport(data.position);
        }

        #endregion

    }


    public static partial class SaveStringifyer
    {
        public static string Stringify(Player pl)
        {
            StringBuilder sb = new();
            sb.Append(SavingConstants.PLAYER_ID);
            sb.Append(SavingConstants.DIVIDER);

            sb.Append(Saving.SaveStringifyer.StringifyEntitySaveData(pl.standardEntitySaveData));

            sb.Append((int) pl.currentGameState);
            sb.Append(SavingConstants.DIVIDER);

            sb.Append(pl.startingPosition.x);
            sb.Append(SavingConstants.DIVIDER);

            sb.Append(pl.startingPosition.y);
            sb.Append(SavingConstants.DIVIDER);

            sb.Append((int) pl.playerState);
            sb.Append(SavingConstants.DIVIDER);

            sb.Append(pl.playerMovement.speed);
            sb.Append(SavingConstants.DIVIDER);

            sb.Append(pl.playerMovement.dir.x);
            sb.Append(SavingConstants.DIVIDER);

            sb.Append(pl.playerMovement.dir.y);
            sb.Append(SavingConstants.DIVIDER);

            sb.Append(pl.playerShoot.spawnPoint.transform.position.x);
            sb.Append(SavingConstants.DIVIDER);

            sb.Append(pl.playerShoot.spawnPoint.transform.position.y);
            sb.Append(SavingConstants.DIVIDER);

            sb.Append(pl.playerShoot.spawnPoint.transform.position.z);
            sb.Append(SavingConstants.DIVIDER);

            sb.Append(pl.playerShoot.maxShots);
            sb.Append(SavingConstants.DIVIDER);

            sb.Append(pl.playerLiveSystem.lives);
            sb.Append(SavingConstants.DIVIDER);

            sb.Append(pl.playerLiveSystem.timeUntilRespawn);
            sb.Append(SavingConstants.DIVIDER);

            sb.Append(SavingConstants.SEGMENT_DIVIDER);

            return sb.ToString();
        }

    }

}
