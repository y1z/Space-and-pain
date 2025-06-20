using System.Collections;
using UnityEngine;

namespace Entities
{
    public sealed class EnemySpawner : MonoBehaviour
    {
        public Enemy prefab;

        public Enemy Spawn()
        {
            Enemy result = Instantiate(prefab, transform.position, transform.rotation);
            StartCoroutine(turnOff());
            return result;
        }

        public IEnumerator turnOff()
        {
            yield return new WaitForEndOfFrame();
            gameObject.SetActive(false);
        }

    }

}
