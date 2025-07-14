using Managers;

using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class StartScreenLogic : MonoBehaviour
{
    public MenuScript menu;
    public MenuScript settings;
    public ConfirmationMenu confirmationMenu;

    public void Awake()
    {
        DDebug.Assert(menu is not null, $"Attach a |{typeof(MenuScript)}| to the variable |{nameof(menu)}| for this to work", this);
        DDebug.Assert(settings is not null, $"Attach a |{typeof(MenuScript)}| to the variable |{nameof(settings)}| for this to work", this);

        if (confirmationMenu is null)
        {
            confirmationMenu = GetComponent<ConfirmationMenu>();
            DDebug.Assert(confirmationMenu is not null, $"Attach a |{typeof(ConfirmationMenu)}| to the variable |{nameof(confirmationMenu)}| for this to work", this);
        }
        confirmationMenu.turnOff();
        confirmationMenu.gameObject.SetActive(false);
    }

    public void Start()
    {
        SingletonManager.inst.soundManager.playMusic("track 2", true);
    }

    public void newGame()
    {
        SceneManager.LoadSceneAsync("Scenes/Game/Main", LoadSceneMode.Single);
    }

    public void loadTutorial()
    {
        SceneManager.LoadSceneAsync("Scenes/Game/Tutorial", LoadSceneMode.Single);
    }

    public void loadCredits()
    {
        SceneManager.LoadSceneAsync("Scenes/Game/Credits", LoadSceneMode.Single);
    }


    public void quit()
    {
        Application.Quit();
    }

    public void changeToSettings()
    {
        menu.turnOffAndHide();
        settings.turnOnAndShow();

        confirmationMenu.confirmationEvent += confirmationMenuEventShowMenu;
        confirmationMenu.turnOffAndHide();
    }

    public void changeToMenu()
    {
        menu.turnOnAndShow();
        settings.turnOffAndHide();

        confirmationMenu.turnOffAndHide();
        confirmationMenu.confirmationEvent -= confirmationMenuEventShowMenu;
    }

    public void confirmationMenuEventShowMenu(bool _isConfirmed)
    {
        showStartMenu();
    }


    public void showConfirmationMenu()
    {
        confirmationMenu.turnOnAndShow();

        settings.turnOffAndHide();
    }

    public void showStartMenu()
    {
        confirmationMenu.turnOffAndHide();

        settings.turnOffAndHide();

        menu.turnOnAndShow();
    }

}
