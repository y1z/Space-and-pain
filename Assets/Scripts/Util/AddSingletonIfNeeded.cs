using UnityEngine;

namespace Util
{
    using Managers;
    [DefaultExecutionOrder(-50)]
    public class AddSingletonIfNeeded : MonoBehaviour
    {

        public GameObject singletonPrefab;

        private void Awake()
        {
            SingletonManager sm = FindAnyObjectByType<SingletonManager>();
            if (sm == null)
            {
                Instantiate(singletonPrefab);
            }
        }
    }

}
