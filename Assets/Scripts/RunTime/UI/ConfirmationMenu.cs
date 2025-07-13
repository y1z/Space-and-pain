using System;
using UnityEngine;

namespace UI
{
    /// <summary>
    /// use the 'confirmationEvent' Action to know which button was pressed (true = yes button pressed, false = no button pressed) 
    /// </summary>
    public sealed class ConfirmationMenu : MonoBehaviour
    {
        [SerializeField] MenuScript menuScript;
        [SerializeField] SelectableButton yesButton;
        [SerializeField] SelectableButton noButton;

        [field: SerializeField] public System.Action<bool> confirmationEvent = null;

        private void Awake()
        {
            DDebug.Assert(menuScript != null, "(menuScript is null) fix that", this);
            DDebug.Assert(yesButton != null, "(yesButton is null) fix that", this);
            DDebug.Assert(noButton != null, "(noButton is null) fix that", this);
        }

        public void OnEnable()
        {
            yesButton.buttonClickEvent.AddListener(onYesButtonPress);
            noButton.buttonClickEvent.AddListener(onNoButtonPress);
        }

        public void OnDisable()
        {
            yesButton.buttonClickEvent.RemoveListener(onYesButtonPress);
            noButton.buttonClickEvent.RemoveListener(onNoButtonPress);
        }

        public void turnOn()
        {
            menuScript.isOn = true;
        }

        public void turnOff()
        {
            menuScript.isOn = true;
        }

        public void subscribe(Action<bool> func)
        {
            confirmationEvent += func;
        }

        public void unSubscribe(Action<bool> func)
        {
            confirmationEvent -= func;
        }

        private void onYesButtonPress()
        {
            confirmationEvent?.Invoke(true);
        }

        private void onNoButtonPress()
        {
            confirmationEvent?.Invoke(false);
        }

        public void turnOffAndHide()
        {
            menuScript.turnOffAndHide();
        }

        public void turnOnAndShow()
        {
            menuScript.turnOnAndShow();
        }

    }

}
