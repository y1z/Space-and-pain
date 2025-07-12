using System.Collections;
using System.Text;
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
            standardEntitySaveData = StandardEntitySaveData.create(_position: transform.position,
                _speed: Vector2.zero,
                _direction: Vector2.zero,
                _isActive: transform.gameObject.activeInHierarchy,
                _prefabName: "Enemy spawner");
            return SaveStringifyer.Stringify(this);
        }

        public void loadSaveData(string data)
        {
            string[] variables = data.Split(Saving.SavingConstants.DIVIDER);

            int index = 2;
            standardEntitySaveData = StandardEntitySaveData.loadData(variables, ref index);
            transform.gameObject.SetActive(standardEntitySaveData.isActive);
            transform.position = standardEntitySaveData.position;
        }

        public void loadData(StandardEntitySaveData data)
        {
            throw new System.NotImplementedException();
        }

        #endregion

    }

    public static partial class SaveStringifyer
    {

        public static string Stringify(EnemySpawner es)
        {
            StringBuilder sb = new();

            sb.Append(Saving.SavingConstants.ENEMY_SPAWNER_ID);
            sb.Append(Saving.SavingConstants.DIVIDER);

            sb.Append(Saving.SaveStringifyer.StringifyEntitySaveData(es.standardEntitySaveData));

            sb.Append(Saving.SavingConstants.SEGMENT_DIVIDER);

            return sb.ToString();
        }
    }

}
