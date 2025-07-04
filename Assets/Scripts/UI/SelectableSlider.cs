using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

namespace UI
{
    public sealed class SelectableSlider : SelectableBase
    {
        [Tooltip("Sends a signal every-time the slider changes")]
        public UnityEvent<SliderEventData> sliderUnitChange;

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

            SliderEventData eventData;
            switch (action)
            {
                case UiAction.MAIN_ACTION:
                    turnOnCount += 1;
                    if (turnOnCount > sliderUnits.Length)
                    {
                        turnOnCount = sliderUnits.Length;
                    }
                    drawSliderUnits();

                    eventData = SliderEventData.create(turnOnCount, sliderUnits.Length, getValue());
                    sliderUnitChange?.Invoke(eventData);

                    break;
                case UiAction.ALT_ACTION:
                    turnOnCount -= 1;
                    if (turnOnCount < 0)
                    {
                        turnOnCount = 0;
                    }
                    drawSliderUnits();

                    eventData = SliderEventData.create(turnOnCount, sliderUnits.Length, getValue());
                    sliderUnitChange?.Invoke(eventData);
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

    [Serializable]
    public struct SliderEventData
    {
        public SliderEventData(int _turnOnUnits, int _totalUnits, float _percentOfTurnOnUnits)
        {
            turnOnUnits = _turnOnUnits;
            totalUnits = _totalUnits;
            percentOfTurnOnUnits = _percentOfTurnOnUnits;
        }

        static public SliderEventData create(int _turnOnUnits, int _totalUnits, float _percentOfTurnOnUnits)
        {
            SliderEventData eventData;
            eventData.totalUnits = _totalUnits;
            eventData.turnOnUnits = _turnOnUnits;
            eventData.percentOfTurnOnUnits = _percentOfTurnOnUnits;
            return eventData;
        }

        public int turnOnUnits;
        public int totalUnits;
        public float percentOfTurnOnUnits;
    }
}
