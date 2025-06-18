using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace UI
{
    public sealed class SelectableButton : SelectableBase
    {
        public UnityEvent buttonClickEvent;
        public TextMeshProUGUI text;
        public Image image;

        public void Start()
        {
            if (buttonClickEvent.GetPersistentEventCount() < 1)
            {
                buttonClickEvent.AddListener(defaultButtonFunction);
            }
        }

        public override UiResult executeAction(UiAction action)
        {
            buttonClickEvent?.Invoke();
            return UiResult.SUCCESS;
        }

        public void defaultButtonFunction()
        {
            EDebug.Log($"Default Button Function Please reassign the function on the variable {nameof(buttonClickEvent)} if this was not intended", this);
        }
    }

}
