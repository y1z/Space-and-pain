using UnityEngine;
using UnityEngine.SceneManagement;


public class TutorialLevelLogic : MonoBehaviour
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
        if (Managers.SingletonManager.inst.inputManager.isPauseActionPressedThisFrame())
        {
            loadStartScreen();
        }
    }
}
