using System.Text;
using UnityEngine;
using interfaces;

namespace Managers
{
    /// <summary>
    /// <para> Use 'addToBeSaved' for all the objects you want to save then use 'finalizeSave' to save that data </para>
    /// <para> Use 'loadSaveData()' to get all the save data in string form</para>
    /// </summary>
    public sealed class SaveManager : MonoBehaviour
    {
        public const string DIVIDER = "<*>";
        const string SAVE_KEY = "save_key";
        public int saveIndex = 0;

        [field: SerializeField]
        public StringBuilder saveData { get; private set; } = new StringBuilder();

        public void addToBeSaved(ISaveGameData thingToBeSaved)
        {
            saveData.Append(thingToBeSaved.getSaveData());
            saveData.Append(DIVIDER);
        }

        public void addToBeSaved(ISaveGameData[] thingToBeSaved)
        {
            for (int i = 0; i < thingToBeSaved.Length; i++)
            {
                addToBeSaved(thingToBeSaved[i]);
            }
        }

        public void addToBeSavedRaw(ISaveGameData thingToBeSaved)
        {
            saveData.Append(thingToBeSaved.getSaveData());
        }

        public string[] loadSaveData()
        {
            string[] result = PlayerPrefs.GetString(SAVE_KEY + saveIndex, "ERROR : NO SAVE DATA FOUND" + DIVIDER).Split(DIVIDER);

            return result;
        }


        public void finalizeSave()
        {
            PlayerPrefs.SetString(SAVE_KEY + saveIndex, saveData.ToString());
            PlayerPrefs.Save();
        }

        public void clear()
        {
            saveData.Clear();
        }

        public void printSaveData()
        {
            string[] dataToPrint = saveData.ToString().Split(DIVIDER);

            foreach (string data in dataToPrint)
            {
                EDebug.Log(data);
            }

        }
    }

}
