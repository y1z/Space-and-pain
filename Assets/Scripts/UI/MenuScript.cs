// Ignore Spelling: selectables

using System.Collections;
using Managers;
using Unity.VisualScripting;
using UnityEngine;

namespace UI
{

    public sealed class MenuScript : MonoBehaviour
    {
        /// <summary>
        ///  controls if the Menu script is doing anything
        /// </summary>

        [Header("Menu controls")]
        public bool isOn = true;
        [Tooltip("The delay between inputs")]
        public float inputDelay = 0.15f;

        public SelectableBase[] selectables = null;
        private GameStates currentGameState;

        [Header("Selector controls")]
        [SerializeField] private int selectorIndex;
        [SerializeField] private RectTransform selectorObject;
        [SerializeField] private Vector3 distanceFromSelectedObject = Vector3.left * 10.0f;

        private InputInfo blockedInputs;

        private void Start()
        {
            selectorIndex = 0;
            EDebug.Assert(selectorObject != null, "selectorObject == null <color=green> // Fix that please </color>", this);
            EDebug.Assert(selectables != null, "selectables == null <color=green> // Fix that please </color>", this);
        }

        void Update()
        {
            if (!isOn) { return; }
            handleInput();
            handleMenuInput();
            selectorPosition();
        }

        #region MathodsCalledByTheUpdateFunction

        void handleInput()
        {
            selectables[selectorIndex].handleInput();
        }

        void handleMenuInput()
        {
            InputManager im = SingletonManager.inst.inputManager;
            InputInfo inputedDirection = im.getInputedDirection();

            if ((inputedDirection & blockedInputs) > 0) { return; }

            if (im.isDirectionUpPressed())
            {
                changeSelectorUp();
                StartCoroutine(blockUpInput());
            }
            else if (im.isDirectionDownPressed())
            {
                changeSelectorDown();
                StartCoroutine(blockDownInput());
            }

        }

        void selectorPosition()
        {
            selectorObject.SetParent(selectables[selectorIndex].transform);
            selectorObject.localPosition = distanceFromSelectedObject;
        }

        #endregion

        #region ChangeSelectorToNextSelectable

        private void changeSelectorUp()
        {
            int nextIndex = selectorIndex - 1;
            if (nextIndex < 0)
            {
                nextIndex = selectables.Length - 1;
            }
            if (!selectables[nextIndex].gameObject.activeInHierarchy)
            {
                for (int i = nextIndex ; i > -1; --i)
                {
                    nextIndex -= 1;
                    if (selectables[nextIndex].gameObject.activeInHierarchy)
                    {
                        break;
                    }

                }

            }

            selectorIndex = nextIndex;
        }

        private void changeSelectorDown()
        {
            int nextIndex = selectorIndex + 1;
            nextIndex = nextIndex % selectables.Length;
            if (!selectables[nextIndex].gameObject.activeInHierarchy)
            {
                // If I can find a usable UI elements then the UI is wrong
                int safetyVar = 10_000;
                for (int i = 0; i < safetyVar; ++i)
                {
                    nextIndex += 1;
                    nextIndex = nextIndex % selectables.Length;
                    if (selectables[nextIndex].gameObject.activeInHierarchy)
                    {
                        break;
                    }
                }
            }


            selectorIndex = nextIndex;
        }


        #endregion

        #region GameManagerBoilerPlate

        private void OnEnable()
        {
            SingletonManager.inst.gameManager.subscribe(onStateChange);
            onStateChange(SingletonManager.inst.gameManager.gameState);
        }

        private void OnDisable()
        {
            SingletonManager.inst.gameManager.unSubscribe(onStateChange);
        }

        private void onStateChange(GameStates newState)
        {
            currentGameState = newState;
        }

        #endregion

        #region Coroutines

        IEnumerator blockUpInput()
        {
            blockedInputs |= InputInfo.UP;
            yield return new WaitForSeconds(inputDelay);
            blockedInputs = blockedInputs & ~InputInfo.UP;
        }

        IEnumerator blockDownInput()
        {
            blockedInputs |= InputInfo.DOWN;
            yield return new WaitForSeconds(inputDelay);
            blockedInputs = blockedInputs & ~InputInfo.DOWN;
        }

        #endregion

    }

}
