using UnityEngine;
using UnityEngine.Events;

namespace UI
{
    using UnityEngine.UI;
    public sealed class SelectableCheckBox : SelectableBase
    {
        [Header("Events")]
        [Tooltip("Image displayed with the check box is ON")]
        public UnityEvent<bool> checkedEvent;
        [Header("Logic")]
        public bool isChecked = false;
        private bool privousIsChecked;

        [Header("Visuals")]
        [Tooltip("Image displayed with the check box is ON")]
        [SerializeField] private Image checkImage;
        [Tooltip("Image displayed with the check box is OFF")]
        [SerializeField] private Image unCheckImage;

        private void Start()
        {
            privousIsChecked = isChecked;
            EDebug.Assert(checkImage != null, $"This script needs a type of {typeof(Image)} to work ", this);
            EDebug.Assert(unCheckImage != null, $"This script needs a type of {typeof(Image)} to work ", this);
        }

        private void Update()
        {
            if (isChecked == privousIsChecked) { return; }
            privousIsChecked = isChecked;
            changeImage();
        }

        public override UiResult executeAction(UiAction action)
        {
            switch (action)
            {
                case UiAction.MAIN_ACTION:
                    if (Debug.isDebugBuild)
                    {
                        if (checkImage == null)
                        {
                            DDebug.LogError($"|{nameof(checkImage)}| is null please fix that", this);
                            return UiResult.ERROR;
                        }
                        if (unCheckImage == null)
                        {
                            DDebug.LogError($"|{nameof(unCheckImage)}| is null please fix that", this);
                            return UiResult.ERROR;
                        }
                    }
                    isChecked = !isChecked;
                    checkedEvent?.Invoke(isChecked);
                    break;
                case UiAction.ALT_ACTION:
                    EDebug.LogWarning($"Check box does not have an {UiAction.ALT_ACTION}", this);
                    break;

            }

            return UiResult.SUCCESS;
        }

        private void changeImage()
        {

            switch (isChecked)
            {
                case true:
                    checkImage.gameObject.SetActive(true);
                    unCheckImage.gameObject.SetActive(false);
                    break;

                case false:
                    checkImage.gameObject.SetActive(false);
                    unCheckImage.gameObject.SetActive(true);
                    break;
            }

        }
    }
}

