using System;
using UnityEngine;

namespace UI
{
    /// <summary>
    /// use the 'confirmationEvent' Action to know which button was pressed (true = yes button pressed, false = no button pressed) 
    /// </summary>
    public sealed class ConfirmationMenu : MonoBehaviour
    {
        [SerializeField] MenuScript logic;
        [SerializeField] SelectableButton yesButton;
        [SerializeField] SelectableButton noButton;

        [field: SerializeField] public System.Action<bool> confirmationEvent;

        private void Awake()
        {
            DDebug.Assert(logic != null, "(logic == null) fix that", this);
            DDebug.Assert(yesButton != null, "(yesButton != null) fix that", this);
            DDebug.Assert(noButton != null, "(noButton != null) fix that", this);
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

    }

}
