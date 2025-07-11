using Managers;
using UI;
using UnityEngine;

namespace Test
{
    public sealed class TestSelectableButton : MonoBehaviour
    {

        [SerializeField] private SelectableSlider slider;
        [SerializeField] private float valueToSetSliderTo = 1.0f;

        public void changeToPause()
        {
            SingletonManager.inst.gameManager.setState(GameStates.PAUSE);
        }

        public void randomNumberSqrt()
        {
            float randValue = UnityEngine.Random.Range(10.0f, 10000903.0f);

            EDebug.Log(Utility.StringUtil.addColorToString($"random value = {randValue}\n\t\tSquare Root ={Mathf.Sqrt(randValue)}", Color.green), this);
        }

        [ContextMenu("Set slider value")]
        public void setSliderValue()
        {
            slider.setValue(valueToSetSliderTo);
        }
    }

}
