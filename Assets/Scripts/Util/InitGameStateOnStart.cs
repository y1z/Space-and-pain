using UnityEngine;

namespace Util
{
    public sealed class InitGameStateOnStart : MonoBehaviour
    {
        [Tooltip("This values controls which state the scene will start in")]
        public Managers.GameStates gameStateToStartOn;

        private void Start()
        {
            Managers.SingletonManager.inst.gameManager.setState(gameStateToStartOn);
        }
    }

}
