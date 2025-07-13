using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class StartScreenLogic : MonoBehaviour
{
    public MenuScript menu;
    public MenuScript settings;

    public void newGame()
    {
        SceneManager.LoadSceneAsync("Scenes/Game/Main", LoadSceneMode.Single);
    }

    public void loadTutorial()
    {
        SceneManager.LoadSceneAsync("Scenes/Game/Tutorial", LoadSceneMode.Single);
    }

    public void quit()
    {
        Application.Quit();
    }

    public void changeToSettings()
    {
        menu.isOn = false;
        menu.gameObject.SetActive(false);
        settings.isOn = true;
        settings.gameObject.SetActive(true);
    }

    public void changeToMenu()
    {
        menu.isOn = true;
        menu.gameObject.SetActive(true);
        settings.isOn = false;
        settings.gameObject.SetActive(false);

    }
}
