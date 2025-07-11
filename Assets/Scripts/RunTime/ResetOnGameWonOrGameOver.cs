using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Managers;

public sealed class ResetOnGameWonOrGameOver : MonoBehaviour
{
    public GameStates gameState { get; private set; }
    public float delay = 1.0f;

    private void FixedUpdate()
    {
        if (gameState != GameStates.GAME_OVER || gameState != GameStates.WON) { return; }
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

    private void setState(GameStates newState)
    {
        gameState = newState;

        switch (gameState)
        {
            case GameStates.GAME_OVER:
                StartCoroutine(reload());
                break;

            case GameStates.WON:
                StartCoroutine(reload());
                break;
        }
    }

    #endregion


    #region Coroutine

    private IEnumerator reload()
    {
        EDebug.Log($"<color=magenta>{nameof(reload)} function called</color>", this);
        yield return new WaitForSeconds(delay);
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex);
    }

    #endregion

}
