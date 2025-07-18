using UnityEngine;
using System.Text;

using interfaces;

namespace Saving
{

    using static SavingConstants;

    /// <summary>
    /// <para> Use 'addToBeSaved' for all the objects you want to save then use 'finalizeSave' to save that data </para>
    /// <para> Use 'loadSaveData()' to get all the save data in string form</para>
    /// </summary>
    public sealed class SaveBuilder 
    {
        static readonly Color DEFAULT_COLOR = Color.white;

        [field: SerializeField]
        public StringBuilder saveData { get; private set; } = new StringBuilder();

        public void addToBeSaved(ISaveGameData thingToBeSaved, bool shouldAddMetaData = false)
        {
            if (shouldAddMetaData)
            {
                saveData.Append(thingToBeSaved.getMetaData());
            }
            saveData.Append(thingToBeSaved.getSaveData());
        }

        public void finalizeSave()
        {
            PlayerPrefs.SetString(SAVE_KEY + SavingStatics.getSavingIndex(), saveData.ToString());
            PlayerPrefs.Save();
            DDebug.Log("<color=green> SAVE WAS FINALIZED </color>");
        }

        public void finalizeCheckPoint()
        {
            PlayerPrefs.SetString(SAVE_KEY + SavingStatics.getSavingIndex() + SavingConstants.checkpoint , saveData.ToString());
            PlayerPrefs.Save();
            DDebug.Log("<color=cyan>CHECK POINT FINALIZED</color>");
        }

        public void clear()
        {
            saveData.Clear();
        }

        public void printSaveDataDebug(Color color = default)
        {
            if (Debug.isDebugBuild)
            {
                printSaveData(color);
            }

        }

        public void printSaveData(Color color = default)
        {
            if (color == default)
            {
                color = DEFAULT_COLOR;
            }

            string[] dataToPrint = saveData.ToString().Split(SEGMENT_DIVIDER);

            foreach (string data in dataToPrint)
            {
                EDebug.Log(Utility.StringUtil.addColorToString(data, color));
            }

        }
    }

}
