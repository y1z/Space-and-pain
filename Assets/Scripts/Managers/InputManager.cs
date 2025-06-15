using UnityEngine;


namespace Managers
{
    using UnityEngine.InputSystem;

    [System.Flags]
    public enum InputInfo : ushort
    {

        NONE = 0,
        SHOOT_ACTION_PRESSED_THIS_FRAME = 0b0000_0000_0000_0001,
        PAUSE_ACTION_PRESSED_THIS_FRAME = 0b0000_0000_0000_0010,
        ANY_ACTION_PRESSED_THIS_FRAME = SHOOT_ACTION_PRESSED_THIS_FRAME | SHOOT_ACTION_PRESSED_THIS_FRAME,

        SHOOT_ACTION_PRESSED = 0b0000_0000_0001_0000,
        PAUSE_ACTION_PRESSED = 0b0000_0000_0010_0000,
        ANY_ACTION_PRESSED = SHOOT_ACTION_PRESSED | PAUSE_ACTION_PRESSED,


        UP = 0b0001_0000_0000_0000,
        DOWN = 0b0010_0000_0000_0000,
        LEFT = 0b0100_0000_0000_0000,
        RIGHT = 0b1000_0000_0000_0000,
        VERTICAL = UP | DOWN,
        HORIZONTAL = LEFT | RIGHT,
        ANY_DIRECTION = VERTICAL | HORIZONTAL,
    }

    /// <summary>
    /// Controls input for the current projec5
    /// </summary>
    public sealed class InputManager : MonoBehaviour
    {
        [SerializeField] private InputActionAsset asset;

        private InputAction moveAction;
        private InputAction shootAction;
        private InputAction pauseAction;

        public Vector2 movement { get; private set; } = Vector2.zero;

        public InputInfo currentInputInfo { get; private set; } = InputInfo.NONE;

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
            currentInputInfo = InputInfo.NONE;
            checkMovementDirection();
            checkIsBeingPressed();
            checkIsBeingPressedThisFrame();
        }

        #region READ_INPUT

        private void checkIsBeingPressedThisFrame()
        {

            if (shootAction.WasPressedThisFrame())
            {
                currentInputInfo |= InputInfo.SHOOT_ACTION_PRESSED_THIS_FRAME;
            }
            else
            {
                currentInputInfo = currentInputInfo & ~InputInfo.SHOOT_ACTION_PRESSED_THIS_FRAME;
            }

            if (pauseAction.WasPressedThisFrame())
            {
                currentInputInfo |= InputInfo.PAUSE_ACTION_PRESSED_THIS_FRAME;
            }
            else
            {
                currentInputInfo = currentInputInfo & ~InputInfo.PAUSE_ACTION_PRESSED_THIS_FRAME;
            }

        }

        private void checkIsBeingPressed()
        {
            if (shootAction.IsPressed())
            {
                currentInputInfo |= InputInfo.SHOOT_ACTION_PRESSED;
            }
            else
            {
                currentInputInfo = currentInputInfo & ~InputInfo.SHOOT_ACTION_PRESSED;
            }

            if (pauseAction.IsPressed())
            {
                currentInputInfo |= InputInfo.PAUSE_ACTION_PRESSED;
            }
            else
            {
                currentInputInfo = currentInputInfo & ~InputInfo.PAUSE_ACTION_PRESSED;
            }

        }

        private void checkMovementDirection()
        {
            movement = moveAction.ReadValue<Vector2>();
            if (movement.y > 0.1f)
            {
                currentInputInfo |= InputInfo.UP;
            }

            if (movement.y < -0.1f)
            {
                currentInputInfo |= InputInfo.DOWN;
            }

            if (movement.x > 0.1f)
            {
                currentInputInfo |= InputInfo.RIGHT;
            }

            if (movement.x < -0.1f)
            {
                currentInputInfo |= InputInfo.LEFT;
            }
        }

        #endregion

        #region IS_INPUT_SECTION

        public bool isShootActionPressedThisFrame()
        {
            return (currentInputInfo & InputInfo.SHOOT_ACTION_PRESSED_THIS_FRAME) > 0;
        }

        public bool isPauseActionPressedThisFrame()
        {
            return (currentInputInfo & InputInfo.PAUSE_ACTION_PRESSED_THIS_FRAME) > 0;
        }

        public bool isShootBeingPressed()
        {
            return (currentInputInfo & InputInfo.SHOOT_ACTION_PRESSED) > 0;
        }

        public bool isPauseBeingPressed()
        {
            return (currentInputInfo & InputInfo.PAUSE_ACTION_PRESSED) > 0;
        }

        public bool isDirectionBeingInputed(InputInfo desired_direction)
        {
            return (currentInputInfo & desired_direction) > 0;
        }

        #endregion

        #region GET_INPUT

        public InputInfo getInputedDirection()
        {
            InputInfo result = InputInfo.NONE;

            result = currentInputInfo & InputInfo.ANY_DIRECTION;

            return result;
        }

        #endregion


    }

}
