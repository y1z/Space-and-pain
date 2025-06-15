using UnityEngine;

public class SingletonManager : MonoBehaviour
{
    public static SingletonManager inst { get; private set; }

    private void Awake()
    {
        EDebug.Log($"{typeof(SingletonManager)} has awaken", this);
        if (inst == null)
        {
            inst = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
