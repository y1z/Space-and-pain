using Managers;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class MainLevelLogic : MonoBehaviour
{
    private GameStates gameStates;
    [field: SerializeField, Tooltip("The pause menu")] MenuScript pauseMenu;
    [field: SerializeField, Tooltip("The settings menu")] MenuScript settingsMenu;


    public void resumeGame()
    {
        SingletonManager.inst.gameManager.setState(GameStates.PLAYING);
    }


    #region GameManagerBoilerPlate

    private void setState(GameStates _gameStates)
    {
        gameStates = _gameStates;
        switch (gameStates)
        {
            case GameStates.PAUSE:
                pauseMenu.gameObject.SetActive(true);
                settingsMenu.gameObject.SetActive(false);
                pauseMenu.isOn = true;
                settingsMenu.isOn = false;
                break;
            default:
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
