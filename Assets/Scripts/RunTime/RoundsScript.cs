using UnityEngine;
using UnityEngine.Events;
using System.Collections;

using Managers;
using TMPro;

public sealed class RoundsScript : MonoBehaviour
{
    private GameStates gameStates;

    private EnemyManager enemyManagerReference;

    [SerializeField]
    private TextMeshProUGUI roundText;

    public UnityEvent onNextRoundLoad;

    private void Start()
    {
        if (enemyManagerReference is null)
        {
            enemyManagerReference = FindFirstObjectByType<EnemyManager>();
            EDebug.Assert(enemyManagerReference != null, $"{enemyManagerReference} needs a {typeof(EnemyManager)} to work", this);
        }
    }

    private IEnumerator prepareNextRound()
    {
        enemyManagerReference.prepareNextRound();
        yield return new WaitForEndOfFrame();

        onNextRoundLoad?.Invoke();
        SingletonManager.inst.gameManager.setState(GameStates.PLAYING);
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
        gameStates = newState;

        switch (gameStates)
        {
            case GameStates.WON:
                StartCoroutine(prepareNextRound());
                break;

        }
    }

    #endregion
}
