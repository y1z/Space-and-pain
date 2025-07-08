using System.Collections;
using interfaces;
using UnityEngine;

namespace Entities
{
    public sealed class EnemySpawner : MonoBehaviour, ISaveGameData, ILoadGameData
    {
        public Enemy prefab;

        public StandardEntitySaveData standardEntitySaveData;

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

        #region InterfacesImpl

        public string getMetaData()
        {
            return JsonUtility.ToJson(new Util.MetaData(nameof(EnemySpawner)));
        }

        public string getSaveData()
        {
            standardEntitySaveData.position = transform.position;
            return JsonUtility.ToJson(this);
        }

        public void loadData(string data)
        {
            JsonUtility.FromJsonOverwrite(data, this);
        }

        public void loadData(StandardEntitySaveData data)
        {
            throw new System.NotImplementedException();
        }

        #endregion

    }

}
