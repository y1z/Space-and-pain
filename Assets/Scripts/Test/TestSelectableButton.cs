using Managers;
using UnityEngine;

namespace Test
{
    public sealed class TestSelectableButton : MonoBehaviour
    {
        public void changeToPause()
        {
            SingletonManager.inst.gameManager.setState(GameStates.PAUSE);
        }

        public void randomNumberSqrt()
        {
            float randValue = UnityEngine.Random.Range(10.0f, 10000903.0f);

            EDebug.Log(Utility.StringUtil.addColorToString($"random value = {randValue}\n\t\tSquare Root ={Mathf.Sqrt(randValue)}",Color.green),this);
        }
    }

}
