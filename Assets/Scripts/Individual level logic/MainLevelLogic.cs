using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class MainLevelLogic : MonoBehaviour
{
    /// <summary>
    /// TODO : REMOVE THIS FUNCTION
    /// </summary>
    public void loadStartScreen()
    {
        SceneManager.LoadScene("Scenes/Game/StartScreen");
    }

    /// <summary>
    /// TODO : REMOVE THIS UPDATE
    /// </summary>
    public void Update()
    {
        if (SingletonManager.inst.inputManager.isPauseActionPressedThisFrame())
        {
            loadStartScreen();
        }
    }

}
