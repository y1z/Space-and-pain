using System.Collections.Generic;
using System.Linq;
using UnityEngine;



namespace Saving
{
    using static SavingConstants;

    public static class SaveLoading
    {
        public static string[] loadSaveData()
        {
            var segment_div = SEGMENT_DIVIDER;

            List<string> 
                result = PlayerPrefs.GetString(SAVE_KEY + SavingStatics.getSavingIndex(), ERROR_NO_SAVE_DATA + segment_div).Split(segment_div).ToList();

            if (string.IsNullOrEmpty(result[result.Count - 1]))
            {
                result.RemoveAt(result.Count - 1);
            }

            return result.ToArray();
        }

    }
}
