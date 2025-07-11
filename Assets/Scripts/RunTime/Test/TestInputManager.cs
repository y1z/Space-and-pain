using UnityEngine;

using TMPro;
namespace Test
{
    using Managers;
    public sealed class TestInputManager : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI movementInputText;
        [SerializeField] TextMeshProUGUI shootActionText;
        [SerializeField] TextMeshProUGUI pauseActionText;
        [SerializeField] TextMeshProUGUI directionInputed;
        [SerializeField] TextMeshProUGUI isShootBeingPressed;
        [SerializeField] TextMeshProUGUI isPauseBeingPressed;

        void Update()
        {
            InputManager im = SingletonManager.inst.inputManager;
            movementInputText.text = $"Movement = {im.movement}";
            shootActionText.text = $"is shoot Action Pressed this Frame= {im.isShootActionPressedThisFrame()}";
            pauseActionText.text = $"is pause Action Pressed this Frame= {im.isPauseActionPressedThisFrame()}";
            directionInputed.text = $"direction inputed = {im.getInputedDirection()}";
            isShootBeingPressed.text = $"is shoot being Pressed= {im.isShootBeingPressed()}";
            isPauseBeingPressed.text = $"is pause being Pressed= {im.isPauseBeingPressed()}";

        }
    }

}

