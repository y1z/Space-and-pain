using System.Text;
using System.Collections.Generic;
using UnityEngine;

using Saving;
using System;
using Util;


namespace Managers
{

    public sealed class GfxManager : MonoBehaviour
    {
        [field: SerializeField]
        public Resolution[] resolutionsAndRefreshRates { get; private set; }

        private Dictionary<string, Resolution> availableStringToResolution = new();
        private Dictionary<Resolution, string> availableResolutionToString = new();

        [field: SerializeField]
        public bool isFullScreen { get; private set; } = false;


        [field: SerializeField]
        public Resolution currentResolution { get; private set; }

        public Action<Resolution> resolutionChangeEvent;


        private void Start()
        {
            resolutionsAndRefreshRates = Screen.resolutions;
            currentResolution = Screen.currentResolution;
            isFullScreen = Settings.getIsFullScreen();

            List<string> resolutionAsString = new();

            for (int i = 0; i < resolutionsAndRefreshRates.Length; ++i)
            {
                string currentResolutionAsString = $"{resolutionsAndRefreshRates[i].width}x{+resolutionsAndRefreshRates[i].height}";

                if (!resolutionAsString.Contains(currentResolutionAsString))
                {
                    resolutionAsString.Add(currentResolutionAsString);

                    availableStringToResolution.Add(currentResolutionAsString, resolutionsAndRefreshRates[i]);
                    availableResolutionToString.Add(resolutionsAndRefreshRates[i], currentResolutionAsString);
                }
            }
        }

        public void setFullScreen(bool _isFullScreen)
        {
            if (isFullScreen != _isFullScreen)
            {
                Settings.setIsFullScreen(_isFullScreen);
            }
            isFullScreen = _isFullScreen;
            Resolution res = Screen.currentResolution;
            Screen.SetResolution(res.width, res.height, isFullScreen);
            Screen.fullScreen = isFullScreen;
        }

        public void setResolution(Resolution res)
        {
            if (!availableResolutionToString.ContainsKey(res))
            {
                DDebug.LogError($"Can't use currentResolution = |{res}|", this);
                return;
            }

            currentResolution = res;
            Screen.SetResolution(res.width, res.height, isFullScreen);

        }

        public void OnDestroy()
        {
            Settings.setIsFullScreen(isFullScreen);
        }


    }

    public static partial class SaveStringifyer
    {

        public static string Stringify(GfxManager gfxMan)
        {
            StringBuilder sb = new();

            sb.Append(SavingConstants.GFX_MANAGER_ID);
            sb.Append(SavingConstants.DIVIDER);

            sb.Append(gfxMan.isFullScreen ? 1 : 0);
            sb.Append(SavingConstants.DIVIDER);

            sb.Append(gfxMan.currentResolution.width);
            sb.Append(SavingConstants.DIVIDER);

            sb.Append(gfxMan.currentResolution.height);
            sb.Append(SavingConstants.DIVIDER);


            sb.Append(SavingConstants.SEGMENT_DIVIDER);
            return sb.ToString();
        }
    }


}
