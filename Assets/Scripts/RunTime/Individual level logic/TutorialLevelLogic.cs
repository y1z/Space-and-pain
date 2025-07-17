using Entities;
using Managers;


using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

using System.Collections.Generic;
using System.Linq;
using System.Collections;

namespace IndividualLevelLogic
{

    public sealed class TutorialLevelLogic : MonoBehaviour
    {
        public enum TutorialStates
        {
            NONE,
            Start,
            TeachMovement,
            TeachShoot,
            TeachPlayerLives,
            TeachEnemyShoot,
            TeachBunkerEnemyShoot,
            TeachBunker,
            TeachPlayerShootBunkerShoot,
            END,
        }


        public Enemy tutorialEnemy;
        public Bunker tutorialBunker;
        public Player tutorialPlayer;
        public Projectile tutorialEnemyProjectile;

        public RectTransform ScoreDisplay;
        public RectTransform LivesDisplay;
        public RectTransform Arrow;

        public RectTransform positionForTextBoxes;

        public TextMeshProUGUI uiText;

        private TutorialStates tutorialState = TutorialStates.NONE;

        public List<TutorialTextBox> textBoxesInScene = new();
        public List<TutorialTextBox> textBoxesInUse = new();
        public Queue<InputInfo> queueOfExpectInputs = new();


        public bool waitForInput { get; set; } = false;
        public int currentTextBoxIndex = 0;


        public void loadStartScreen()
        {
            SceneManager.LoadScene("Scenes/Game/StartScreen");
        }

        public void Start()
        {
            tutorialState = TutorialStates.Start;

            textBoxesInScene = FindObjectsByType<TutorialTextBox>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
            textBoxesInScene.Sort(TutorialTextBoxCompare.compareObj);
            waitForInput = false;
            SingletonManager.inst.soundManager.playMusic("track 2", looping: true);
            SingletonManager.inst.gameManager.setState(GameStates.PAUSE);
            ScoreDisplay.gameObject.SetActive(false);
            LivesDisplay.gameObject.SetActive(false);

            Vector3 offScreenPos = Vector3.one * 42069.0f;

            Projectile template = Resources.Load<Projectile>(EnemyManager.PATH_TO_PROJECTILE);
            tutorialEnemyProjectile = Instantiate<Projectile>(template, offScreenPos, Quaternion.identity);

        }

        public void Update()
        {
            if (Managers.SingletonManager.inst.inputManager.isPauseActionPressedThisFrame())
            {
                loadStartScreen();
            }

            if (waitForInput)
            {
                getInput();
                moveTextBox();
                hideTextBoxes();
                return;
            }


            switch (tutorialState)
            {
                case TutorialStates.Start:
                    findTextBoxesOfState(tutorialState);
                    queueOfExpectInputs.Enqueue(InputInfo.SHOOT_ACTION_PRESSED_THIS_FRAME);
                    queueOfExpectInputs.Enqueue(InputInfo.SHOOT_ACTION_PRESSED_THIS_FRAME);
                    waitForInput = true;
                    break;

                case TutorialStates.TeachMovement:
                    queueOfExpectInputs.Clear();
                    queueOfExpectInputs.Enqueue(InputInfo.LEFT);
                    queueOfExpectInputs.Enqueue(InputInfo.RIGHT);


                    hideCurrentTextBoxes();
                    findTextBoxesOfState(tutorialState);
                    waitForInput = true;
                    break;
                case TutorialStates.TeachShoot:
                    queueOfExpectInputs.Clear();
                    SingletonManager.inst.gameManager.setState(GameStates.PLAYING);
                    tutorialEnemy.transform.position = Vector3.one * 420691337.0f;

                    queueOfExpectInputs.Enqueue(InputInfo.SHOOT_ACTION_PRESSED_THIS_FRAME);
                    hideCurrentTextBoxes();

                    findTextBoxesOfState(tutorialState);
                    waitForInput = true;
                    break;
                case TutorialStates.TeachPlayerLives:
                    queueOfExpectInputs.Clear();
                    SingletonManager.inst.gameManager.setState(GameStates.PLAYING);

                    queueOfExpectInputs.Enqueue(InputInfo.SHOOT_ACTION_PRESSED_THIS_FRAME);
                    queueOfExpectInputs.Enqueue(InputInfo.SHOOT_ACTION_PRESSED_THIS_FRAME);

                    Arrow.gameObject.SetActive(true);
                    ScoreDisplay.gameObject.SetActive(true);
                    LivesDisplay.gameObject.SetActive(true);
                    hideCurrentTextBoxes();
                    findTextBoxesOfState(tutorialState);

                    waitForInput = true;
                    break;
                case TutorialStates.TeachEnemyShoot:

                    queueOfExpectInputs.Enqueue(InputInfo.SHOOT_ACTION_PRESSED_THIS_FRAME);
                    queueOfExpectInputs.Enqueue(InputInfo.SHOOT_ACTION_PRESSED_THIS_FRAME);

                    positionForTextBoxes.localPosition = Vector3.down * 400;
                    hideCurrentTextBoxes();
                    findTextBoxesOfState(tutorialState);

                    tutorialEnemy.transform.position = new Vector3(0.0f, 3.0f,0.0f);
                    tutorialEnemyProjectile.transform.position = new(0.0f, 2.0f, 0.0f);
                    tutorialEnemyProjectile.teleport(new Vector3(0.0f, 2.0f, 0.0f));
                    tutorialEnemyProjectile.gameObject.SetActive(true);
                    waitForInput = true;
                    StartCoroutine(shotAndDoThing());

                    break;

                case TutorialStates.TeachBunker:

                    queueOfExpectInputs.Enqueue(InputInfo.SHOOT_ACTION_PRESSED_THIS_FRAME);
                    queueOfExpectInputs.Enqueue(InputInfo.SHOOT_ACTION_PRESSED_THIS_FRAME);

                    positionForTextBoxes.localPosition = Vector3.down * 400;
                    hideCurrentTextBoxes();
                    findTextBoxesOfState(tutorialState);
                    break;
                default:
                    DDebug.LogError($"Un-handled case = |{tutorialState}|", this);
                    break;
            }
        }

        public void findTextBoxesOfState(TutorialStates state)
        {
            this.textBoxesInUse.Clear();

            for (int i = 0; i < textBoxesInScene.Count; i++)
            {
                if (textBoxesInScene[i].tutorialState == state)
                {
                    textBoxesInUse.Add(textBoxesInScene[i]);
                }
            }
        }

        public void hideCurrentTextBoxes()
        {
            for (int i = 0; i < textBoxesInUse.Count; i++)
            {
                textBoxesInUse[i].background.rectTransform.localPosition = Vector3.one * 420691337.0f;
                textBoxesInUse[i].gameObject.SetActive(false);
            }

        }

        public void hideAllTextBoxesBeforeIndex()
        {
            for (int i = currentTextBoxIndex - 1; i >= 0; --i)
            {
                textBoxesInUse[i].background.rectTransform.localPosition = Vector3.one * 420691337.0f;
                textBoxesInUse[i].gameObject.SetActive(false);
            }

        }

        public void hideTextBoxes()
        {
            if (currentTextBoxIndex < 1) { return; }
            hideAllTextBoxesBeforeIndex();
        }

        public void getInput()
        {
            if (!wasExpectedInputPressed()) { return; }

            int nextIndex = currentTextBoxIndex + 1;
            queueOfExpectInputs.Dequeue();

            if (nextIndex >= textBoxesInUse.Count)
            {
                currentTextBoxIndex = 0;
                waitForInput = false;
                this.tutorialState = this.tutorialState + 1;
                return;
            }

            currentTextBoxIndex = nextIndex;

        }

        public void moveTextBox()
        {
            textBoxesInUse[currentTextBoxIndex].background.rectTransform.localPosition = positionForTextBoxes.localPosition;
            if (currentTextBoxIndex > 0)
            {
                hideAllTextBoxesBeforeIndex();
            }
        }

        public bool wasExpectedInputPressed()
        {
            if (queueOfExpectInputs.Count < 1) { return false; }

            InputInfo inputThisFrame = SingletonManager.inst.inputManager.currentInputInfo;
            InputInfo expectedInput = queueOfExpectInputs.Peek();

            return (inputThisFrame & expectedInput) > 0;
        }

        #region Coroutines

        private IEnumerator shotAndDoThing()
        {
            yield return new WaitForEndOfFrame();

            SingletonManager.inst.gameManager.setState(GameStates.PAUSE);
            yield return new WaitForEndOfFrame();

            yield break;
        }

        #endregion
    }
}
