using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public sealed class SelectableSlider : SelectableBase
    {
        [Tooltip("This is the container of the slider Units (aka the objects need for the slider to work)")]
        [SerializeField] private HorizontalOrVerticalLayoutGroup sliderContainer;

        [Tooltip("The representation of the slider")]
        [SerializeField] private Image[] sliderUnits;

        [Tooltip("The color for the slider unit that is on")]
        [SerializeField] private Color onColor;
        [Tooltip("The color for the slider unit that is off")]
        [SerializeField] private Color offColor;
        public int turnOnCount { get; private set; } = 0;

        private void Start()
        {
            EDebug.Assert(sliderContainer != null, $"This script needs a type of {typeof(HorizontalOrVerticalLayoutGroup)}", this);
            sliderUnits = sliderContainer.GetComponentsInChildren<Image>();

            EDebug.Assert(sliderUnits.Length > 1, $"This scripts needs an array of {typeof(Image)}", this);
            turnOnCount = sliderUnits.Length / 2;
            base.interactionType = InteractionType.LEFT_RIGHT;
            drawSliderUnits();
        }

        public override UiResult executeAction(UiAction action)
        {

            switch (action)
            {
                case UiAction.MAIN_ACTION:
                    turnOnCount += 1;
                    if (turnOnCount > sliderUnits.Length)
                    {
                        turnOnCount = sliderUnits.Length;
                    }
                    drawSliderUnits();

                    break;
                case UiAction.ALT_ACTION:
                    turnOnCount -= 1;
                    if (turnOnCount < 0)
                    {
                        turnOnCount = 0;
                    }
                    drawSliderUnits();

                    break;
            }
            return UiResult.SUCCESS;
        }

        private void drawSliderUnits()
        {
            for (int i = 0; i < sliderUnits.Length; ++i)
            {
                if (i < turnOnCount)
                {
                    sliderUnits[i].color = onColor;
                    continue;
                }
                sliderUnits[i].color = offColor;
            }

        }
    }
}
