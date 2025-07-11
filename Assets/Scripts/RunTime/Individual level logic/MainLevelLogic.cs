using Managers;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scriptable_Objects;
using Entities;
using System.Collections.Generic;

public sealed class MainLevelLogic : MonoBehaviour
{
    private GameStates gameStates;

    [field: SerializeField, Tooltip("The pause menu")]
    MenuScript pauseMenu;

    [field: SerializeField, Tooltip("The settings menu")]
    MenuScript settingsMenu;

    [field: SerializeField, Tooltip("The menu used to confirm options")]
    ConfirmationMenu confirmationMenu;

    Player playerReference = null;

    bool isConfirmationMenuOn = false;


    public void resumeGame()
    {
        SingletonManager.inst.gameManager.setState(GameStates.PLAYING);
    }

    public void save()
    {
        Saving.SaveBuilder sab = new();

        playerReference.selfAssignComponents();

        sab.addToBeSaved(playerReference);

        List<Enemy> enemies = SingletonManager.inst.enemyManager.enemies; //FindObjectsByType<Enemy>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        EDebug.Assert(enemies is not null, $"Could not find type of {typeof(Enemy)} in this scene", this);

        for (int i = 0; i < enemies.Count; i++)
        {
            sab.addToBeSaved(enemies[i]);
        }

        EnemySpawner[] spawners = SingletonManager.inst.enemyManager.enemySpawners;
        EDebug.Assert(spawners is not null, $"Could not find type of {typeof(EnemySpawner)} in this scene", this);

        foreach (EnemySpawner s in spawners)
        {
            sab.addToBeSaved(s);
        }


        Bunker[] bunkers = FindObjectsByType<Bunker>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        for (int i = 0; i < bunkers.Length; i++)
        {
            sab.addToBeSaved(bunkers[i]);
        }

        Projectile[] projectiles = FindObjectsByType<Projectile>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        for (int i = 0; i < projectiles.Length; i++)
        {
            sab.addToBeSaved(projectiles[i]);
        }

        sab.addToBeSaved(SingletonManager.inst.scoreManager);
        sab.addToBeSaved(SingletonManager.inst.gameManager);

        sab.printSaveDataDebug();

        sab.finalizeSave();

        SingletonManager.inst.soundManager.playSFX("deny beep");
    }

    public void load()
    {
        string[] loadingData = Saving.SaveLoading.loadSaveData();

        if (loadingData[0] == Saving.SavingConstants.ERROR_NO_SAVE_DATA)
        {
            EDebug.LogError(loadingData[0], this);
            SingletonManager.inst.soundManager.playSFX("deny beep");
            return;
        }

        int enemyIndex = 0;
        int enemySpanwerIndex = 0;
        int bunkerIndex = -1; // yes this starting at -1 is intentional 
        int blockIndex = 0;
        int projectile = 0;

        List<Enemy> enemies = SingletonManager.inst.enemyManager.enemies;

        EnemyManager enemyManager = SingletonManager.inst.enemyManager;

        Bunker[] bunkers = FindObjectsByType<Bunker>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        playerReference.selfAssignComponents();


        SingletonManager.inst.soundManager.playSFX("deny beep");
    }

    public void settings()
    {
        pauseMenu.gameObject.SetActive(false);
        settingsMenu.gameObject.SetActive(true);
        pauseMenu.isOn = false;
        settingsMenu.isOn = true;
    }

    public void goBackToMenu()
    {
        if (!isConfirmationMenuOn)
        {
            confirmationMenu.confirmationEvent += confirmGoBackToMenu;
            confirmationMenu.turnOn();
            settingsMenu.isOn = false;
            settingsMenu.gameObject.SetActive(false);
            isConfirmationMenuOn = true;
            showConfirmationMenu();
        }
    }

    private void confirmGoBackToMenu(bool confirm)
    {
        settingsMenu.isOn = true;
        confirmationMenu.turnOff();
        if (confirm)
        {
            pauseMenu.gameObject.SetActive(true);
            settingsMenu.gameObject.SetActive(false);
            pauseMenu.isOn = true;
            settingsMenu.isOn = false;
        }
        else
        {
            pauseMenu.gameObject.SetActive(false);
            settingsMenu.gameObject.SetActive(true);
            settingsMenu.isOn = true;

        }
        confirmationMenu.confirmationEvent -= confirmGoBackToMenu;
        hideConfdirmationMenu();
        isConfirmationMenuOn = false;
    }

    /// <summary>
    /// TODO: Add confirmation menu to this function
    /// </summary>
    public void quitToStartScreen()
    {
        SceneManager.LoadScene("Scenes/Game/StartScreen");
    }

    // volume 
    public void masterVolumeSlider(SliderEventData data)
    {
        EDebug.Log($"{nameof(masterVolumeSlider)} was called", this);
        EDebug.Log($"percent = %{data.percentOfTurnOnUnits} was called", this);

        SingletonManager.inst.soundManager.setMasterVolume(data.percentOfTurnOnUnits);
    }

    public void musicVolumeSlider(SliderEventData data)
    {
        EDebug.Log($"{nameof(musicVolumeSlider)} was called", this);
        EDebug.Log($"percent = %{data.percentOfTurnOnUnits} was called", this);
        SingletonManager.inst.soundManager.setVolume(GameAudioType.MUSIC, data.percentOfTurnOnUnits);
    }

    public void sfxVolumeSlider(SliderEventData data)
    {
        EDebug.Log($"{nameof(sfxVolumeSlider)} was called", this);
        EDebug.Log($"percent = %{data.percentOfTurnOnUnits} was called", this);
        SingletonManager.inst.soundManager.setVolume(GameAudioType.SFX, data.percentOfTurnOnUnits);
    }

    public void voiceVolumeSlider(SliderEventData data)
    {
        EDebug.Log($"{nameof(voiceVolumeSlider)} was called", this);
        EDebug.Log($"percent = %{data.percentOfTurnOnUnits} was called", this);
        SingletonManager.inst.soundManager.setVolume(GameAudioType.VOICE, data.percentOfTurnOnUnits);
    }

    /// <summary>
    /// TODO: Implement function for changing to full screen
    /// <para><param name="isChecked">true = go full screen, false = don't go full screen</param> </para>
    /// </summary>
    public void onFullScreenChange(bool isChecked)
    {
        EDebug.Log($"{nameof(onFullScreenChange)} was called", this);
    }

    private void showConfirmationMenu()
    {
        confirmationMenu.gameObject.SetActive(true);
        confirmationMenu.transform.localPosition = Vector3.zero;
        confirmationMenu.transform.SetAsLastSibling();
    }

    private void hideConfdirmationMenu()
    {
        confirmationMenu.transform.localPosition = new Vector3(1337, 1337);
        confirmationMenu.transform.SetAsFirstSibling();
        confirmationMenu.gameObject.SetActive(false);
    }

    #region GameManagerAndSceneManagerBoilerPlate

    private void OnEnable()
    {
        Managers.SingletonManager.inst.gameManager.subscribe(setState);
        SceneManager.activeSceneChanged += onActiveSceneChange;
    }

    private void OnDisable()
    {
        Managers.SingletonManager.inst.gameManager.unSubscribe(setState);

        SceneManager.activeSceneChanged -= onActiveSceneChange;
    }

    private void setState(GameStates _gameStates)
    {
        gameStates = _gameStates;
        switch (gameStates)
        {
            case GameStates.PAUSE:
                confirmationMenu.gameObject.SetActive(true);
                confirmationMenu.transform.localPosition = new Vector3(1337, 1337);
                confirmationMenu.turnOff();
                pauseMenu.gameObject.SetActive(true);
                settingsMenu.gameObject.SetActive(false);
                pauseMenu.isOn = true;
                settingsMenu.isOn = false;

                if (playerReference is null)
                {
                    playerReference = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
                    EDebug.Assert(playerReference != null, $"{nameof(playerReference)} needs a {typeof(Player)} to work.", this);
                }

                break;
            case GameStates.INIT_ENEMYS:

                GameObject playerGameObject = GameObject.FindGameObjectWithTag("Player");
                playerReference = playerGameObject.GetComponent<Player>();

                // only exist because when loading the scene back it's variables are null
                playerReference.selfAssignComponents();

                break;
            default:
                confirmationMenu.gameObject.SetActive(true);
                confirmationMenu.transform.localPosition = new Vector3(1337, 1337);
                confirmationMenu.turnOff();
                pauseMenu.gameObject.SetActive(false);
                settingsMenu.gameObject.SetActive(false);
                pauseMenu.isOn = false;
                settingsMenu.isOn = false;
                break;

        }
    }

    private void onActiveSceneChange(Scene prev, Scene next)
    {
        playerReference = null;
    }

    #endregion


}
