using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class StartScreenLogic : MonoBehaviour
{
    public MenuScript menu;
    public MenuScript settings;

    public void newGame()
    {
        SceneManager.LoadScene("Scenes/Game/Main");
    }

    public void loadTutorial()
    {
        SceneManager.LoadScene("Scenes/Game/Tutorial");
    }

    public void quit()
    {
        Application.Quit();
    }
}
