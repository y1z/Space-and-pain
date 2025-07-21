
using UnityEngine;

namespace Managers
{
    [DefaultExecutionOrder(-30)]
    public sealed class SingletonManager : MonoBehaviour
    {
        public static SingletonManager inst
        {
            get; private set;
        }

        [field: SerializeField] public GameManager gameManager { get; private set; }
        [field: SerializeField] public InputManager inputManager { get; private set; }
        [field: SerializeField] public EnemyManager enemyManager { get; private set; }
        [field: SerializeField] public SoundManager soundManager { get; private set; }
        [field: SerializeField] public ScoreManager scoreManager { get; private set; }
        [field: SerializeField] public GfxManager gfxManager { get; private set; }

        private void Awake()
        {
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

            if (soundManager == null)
            {
                soundManager = GetComponentInChildren<SoundManager>();
                EDebug.Assert(soundManager != null, $"{nameof(soundManager)} needs a {typeof(SoundManager)} to work.", this);
            }

            if (scoreManager == null)
            {
                scoreManager = GetComponentInChildren<ScoreManager>();
                EDebug.Assert(scoreManager != null, $"{nameof(scoreManager)} needs a {typeof(ScoreManager)} to work.", this);
            }

            if (gfxManager == null)
            {
                gfxManager = GetComponentInChildren<GfxManager>();
                EDebug.Assert(gfxManager != null, $"{nameof(gfxManager)} needs a {typeof(GfxManager)} to work.", this);
            }

        }
    }
}

