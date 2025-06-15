using UnityEngine;


namespace Managers
{
    using UnityEngine.InputSystem;
    /// <summary>
    /// </summary>
    public sealed class InputManager : MonoBehaviour
    {
        [SerializeField] private InputActionAsset asset;

        private InputAction moveAction;
        private InputAction shootAction;
        private InputAction pauseAction;

        public Vector2 movement { get; private set; } = Vector2.zero;
        public bool shootActionIsPressed { get; private set; } = false;
        public bool pauseActionIsPressed { get; private set; } = false;

        private void OnEnable()
        {
            asset.FindActionMap("Player").Enable();
        }

        private void OnDisable()
        {
            asset.FindActionMap("Player").Disable();
        }
        private void Awake()
        {
            moveAction = InputSystem.actions.FindAction("Move");
            shootAction = InputSystem.actions.FindAction("Shoot");
            pauseAction = InputSystem.actions.FindAction("Pause");

        }

        private void Update()
        {
            movement = moveAction.ReadValue<Vector2>();

            if (shootAction.WasPressedThisFrame())
            {
                shootActionIsPressed = true;
            }
            else
            {
                shootActionIsPressed = false;
            }

            if (pauseAction.WasPressedThisFrame())
            {
                pauseActionIsPressed = true;
            }
            else
            {
                pauseActionIsPressed = false;
            }

        }

    }

}
