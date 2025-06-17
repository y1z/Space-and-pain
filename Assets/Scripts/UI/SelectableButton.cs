using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace UI
{
    public sealed class SelectableButton : SelectableBase
    {
        public UnityEvent onButtonPress;
        public TextMeshProUGUI text;
        public Image image;

        public override UiResult executeAction(UiAction action)
        {
            if (onButtonPress == null)
            {
                return UiResult.ERROR;
            }

            onButtonPress.Invoke();
            return UiResult.SUCCESS;
        }
    }

}
