
using UnityEngine;

namespace Managers
{
    [DefaultExecutionOrder(-30)]
    public class SingletonManager : MonoBehaviour
    {
        public static SingletonManager inst
        {
            get; private set;
        }

        [field: SerializeField] public GameManager gameManager { get; private set; }
        [field: SerializeField] public InputManager inputManager { get; private set; }
        [field: SerializeField] public EnemyManager enemyManager { get; private set; }

        private void Awake()
        {
            EDebug.Log($"{typeof(SingletonManager)} has awaken", this);
            if (inst == null)
            {
                inst = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            if (gameManager == null)
            {
                gameManager = GetComponentInChildren<GameManager>();
                EDebug.Assert(gameManager != null, $"{typeof(GameManager)} Not attached to {typeof(SingletonManager)}", this);
            }

            if (inputManager == null)
            {
                inputManager = GetComponentInChildren<InputManager>();
                EDebug.Assert(inputManager != null, $"{typeof(InputManager)} Not attached to {typeof(SingletonManager)}", this);
            }

            if (enemyManager == null) 
            {
                enemyManager = GetComponentInChildren<EnemyManager>();
                EDebug.Assert(enemyManager != null, $"{typeof(EnemyManager)} Not attached to {typeof(SingletonManager)}", this);
            }

        }
    }
}

