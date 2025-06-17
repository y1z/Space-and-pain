using Managers;
using UnityEngine;

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
        public InteractionType interactionType;
        public abstract void ExecuteAction(UiAction action);

        /// <summary>
        /// Default implementation of how UI will handle input in the game
        /// </summary>
        public virtual void HandleInput()
        {
            switch (interactionType)
            {
                case InteractionType.CLICK:
                    if (SingletonManager.inst.inputManager.isShootActionPressedThisFrame())
                    {
                        ExecuteAction(UiAction.MAIN_ACTION);
                    }
                    break;
                case InteractionType.LEFT_RIGHT:
                    InputInfo inputDir = SingletonManager.inst.inputManager.getInputedDirection();

                    if ((inputDir & InputInfo.RIGHT) > 0)
                    {
                        ExecuteAction(UiAction.MAIN_ACTION);
                    }
                    else if ((inputDir & InputInfo.LEFT) > 0)
                    {
                        ExecuteAction(UiAction.ALT_ACTION);
                    }

                    break;

            }

        }
    }
}
