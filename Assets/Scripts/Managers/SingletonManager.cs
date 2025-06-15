
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
                gameManager = GetComponent<GameManager>();
                EDebug.Assert(gameManager != null, $"Game Manager Not attached to {typeof(SingletonManager)}", this);
            }

        }
    }
}

