using System;
using Managers;
using UnityEngine;
using UnityEngine.Events;

namespace UI
{
    /// <summary>
    /// Controls what the UI does when "Clicked ON"
    /// </summary>
    public enum UiAction
    {
        MAIN_ACTION,
        ALT_ACTION,
    }

    /// <summary>
    /// says if anything went wrong in the executeAction method
    /// </summary>
    public enum UiResult
    {
        ERROR = 0,
        SUCCESS,
    }

    /// <summary>
    /// Controls how to interact with the given UI element
    /// </summary>
    public enum InteractionType
    {
        CLICK,
        LEFT_RIGHT,
    }

    /// <summary>
    /// The base class for all UI used in the game
    /// </summary>
    public abstract class SelectableBase : MonoBehaviour
    {
        public RectTransform rectTransform;
        public InteractionType interactionType { get; protected set; } = InteractionType.CLICK;
        public UnityEvent<UiAction, UiResult> onActionExecute;
        public abstract UiResult executeAction(UiAction action);

        /// <summary>
        /// If the UI Element needs to be a certain way use this method for that
        /// </summary>
        public virtual void setup(Action<SelectableBase> setFunction) { }

        /// <summary>
        /// Default implementation of how UI will handle input in the game 
        /// if you don't like it you can override it
        /// </summary>
        public virtual void handleInput()
        {
            switch (interactionType)
            {
                case InteractionType.CLICK:
                    if (SingletonManager.inst.inputManager.isShootActionPressedThisFrame())
                    {
                        UiResult result = executeAction(UiAction.MAIN_ACTION);
                        onActionExecute?.Invoke(UiAction.MAIN_ACTION, result);
                    }
                    break;
                case InteractionType.LEFT_RIGHT:
                    InputInfo inputDir = SingletonManager.inst.inputManager.getInputedDirection();

                    if ((inputDir & InputInfo.RIGHT) > 0)
                    {
                        UiResult result = executeAction(UiAction.MAIN_ACTION);
                        onActionExecute?.Invoke(UiAction.MAIN_ACTION, result);
                    }
                    else if ((inputDir & InputInfo.LEFT) > 0)
                    {
                        UiResult result = executeAction(UiAction.ALT_ACTION);
                        onActionExecute?.Invoke(UiAction.ALT_ACTION, result);
                    }

                    break;

            }

        }
    }
}
