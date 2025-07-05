using Managers;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scriptable_Objects;

public sealed class MainLevelLogic : MonoBehaviour
{
    private GameStates gameStates;

    [field: SerializeField, Tooltip("The pause menu")]
    MenuScript pauseMenu;

    [field: SerializeField, Tooltip("The settings menu")]
    MenuScript settingsMenu;

    [field: SerializeField, Tooltip("The menu used to confirm options")]
    ConfirmationMenu confirmationMenu;

    bool isConfirmationMenuOn = false;


    public void resumeGame()
    {
        SingletonManager.inst.gameManager.setState(GameStates.PLAYING);
    }

    public void save()
    {
        SingletonManager.inst.soundManager.playSFX("deny beep");
    }

    public void load()
    {
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
        SceneManager.LoadSceneAsync("Scenes/Game/StartScreen", LoadSceneMode.Single);
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
    /// </summary>
    /// <param name="checkThing"></param>
    public void onFullScreenChange(bool checkThing)
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

    #region GameManagerBoilerPlate

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

    private void OnEnable()
    {
        Managers.SingletonManager.inst.gameManager.subscribe(setState);
    }

    private void OnDisable()
    {
        Managers.SingletonManager.inst.gameManager.unSubscribe(setState);
    }

    #endregion

}
