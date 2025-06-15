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

        void Update()
        {
            movementInputText.text = $"Movement = {SingletonManager.inst.inputManager.movement}";
            shootActionText.text = $"is shoot Action Pressed ={SingletonManager.inst.inputManager.isShootActionPressedThisFrame()}";
            pauseActionText.text = $"is pause Action Pressed ={SingletonManager.inst.inputManager.isPauseActionPressedThisFrame()}";
        }
    }

}

