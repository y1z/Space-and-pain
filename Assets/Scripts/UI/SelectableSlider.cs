using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Analytics;
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
        [ColorUsage(true, true)]
        [SerializeField] private Color onColor;

        [Tooltip("The color for the slider unit that is off")]
        [ColorUsage(true, true)]
        [SerializeField] private Color offColor;

        public int turnOnCount { get; private set; } = 0;

        public float separationBetweenSliderUnits = 0.2f;

        private void Start()
        {
            EDebug.Assert(sliderContainer != null, $"This script needs a type of {typeof(HorizontalOrVerticalLayoutGroup)}", this);
            sliderUnits = sliderContainer.GetComponentsInChildren<Image>();

            EDebug.Assert(sliderUnits.Length > 1, $"This scripts needs an array of {typeof(Image)}", this);
            turnOnCount = sliderUnits.Length / 2;
            base.interactionType = InteractionType.LEFT_RIGHT;
            drawSliderUnits();
        }

        private void Update()
        {
            sliderContainer.spacing = separationBetweenSliderUnits;
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

        public override float getValue()
        {
            return ((float)sliderUnits.Length / (float)turnOnCount);
        }

        public override void setValue(float newValue)
        {
            turnOnCount = Mathf.FloorToInt(sliderUnits.Length * Mathf.Clamp01(newValue));
            drawSliderUnits();
        }

        [ContextMenu("get all the blocks")]
        private void getAllTheBlocks()
        {
            sliderUnits = sliderContainer.GetComponentsInChildren<Image>();
        }

        [ContextMenu("remove all the block")]
        private void removeAllTheBlocks()
        {
            sliderUnits = null;
        }

        [ContextMenu("Color the blocks")]
        private void devColorTheBlocks()
        {
            if (sliderUnits == null)
            {
                getAllTheBlocks();
            }
            int halfSliders = sliderUnits.Length / 2;

            for (int i = 0; i < sliderUnits.Length; ++i)
            {
                if (i < halfSliders)
                {
                    sliderUnits[i].color = onColor;
                }
                else
                {
                    sliderUnits[i].color = offColor;
                }

            }

            removeAllTheBlocks();
        }

        [ContextMenu("reset color")]
        private void devResetColor()
        {
            if (sliderUnits == null)
            {
                getAllTheBlocks();
            }

            for (int i = 0; i < sliderUnits.Length; ++i)
            {
                sliderUnits[i].color = Color.white;
            }

            removeAllTheBlocks();
        }

    }
}
