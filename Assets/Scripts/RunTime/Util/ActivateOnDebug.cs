using UnityEngine;


namespace Util
{
    /**
     * Used for debugging purposes
     */
    public sealed class ActivateOnDebug : MonoBehaviour
    {
        [SerializeField] GameObject objectToActive;

        private void Start()
        {
            objectToActive.SetActive(Debug.isDebugBuild);
        }
    }

}
