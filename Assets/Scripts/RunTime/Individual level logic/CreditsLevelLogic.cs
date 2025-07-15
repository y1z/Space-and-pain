using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

using Credits;
using Managers;
using System.Collections;

public sealed class CreditsLevelLogic : MonoBehaviour
{
    const string PATH_TO_CREDITS_FOLDER = "Credits/";

    const string PATH_TO_CREDIT_BOX_TEMPLATE = "Prefabs/UI/Credit Box";

    const float TIME_TO_REACH_TOP_SCRREN = 4.0f;

    const float INVERSE_TIME_TO_REACH_TOP_SCRREN = 1.0f / TIME_TO_REACH_TOP_SCRREN;


    // multiplier 
    public List<CreditsBox> credits;
    public CreditsBox creditsBoxTemplate = null;
    public float speedMultiplier = 1.0f;
    public Transform spawnPoint;


    public RectTransform topRight;
    public RectTransform bottomLeft;

    public float inputDelay = 0.15f;
    public InputInfo blockedInput = InputInfo.NONE;

    private void Awake()
    {
        creditsBoxTemplate = Resources.Load<CreditsBox>(PATH_TO_CREDIT_BOX_TEMPLATE);

        DDebug.Assert(creditsBoxTemplate is not null, this);

        DDebug.Assert(spawnPoint is not null, this);

        DDebug.Assert(topRight is not null, this);
        DDebug.Assert(bottomLeft is not null, this);
    }

    void Start()
    {
        TextAsset[] allCredits = Resources.LoadAll<TextAsset>(PATH_TO_CREDITS_FOLDER);


        for (int i = 0; i < allCredits.Length; ++i)
        {
            CreditsBox newCreditsBox = Instantiate<CreditsBox>(creditsBoxTemplate, spawnPoint);
            newCreditsBox.text.text = allCredits[i].text.ToString();

            credits.Add(newCreditsBox);
        }

        float backGroundHieght = Screen.height;

        for (int i = 0; i < credits.Count; ++i)
        {
            Vector3 pos = credits[i].transform.position;
            pos.y -= backGroundHieght * i;
            credits[i].transform.position = pos;
        }

        SingletonManager.inst.soundManager.playMusic("track 1", true);

    }

    private void Update()
    {
        InputManager im = SingletonManager.inst.inputManager;

        InputInfo currentFrameInput = im.currentInputInfo;

        if (im.isPauseActionPressedThisFrame())
        {
            SceneManager.LoadSceneAsync("Scenes/Game/StartScreen", LoadSceneMode.Single);
        }

        if ((currentFrameInput & blockedInput) > 0) { return; }

        if (im.isDirectionUpPressed())
        {
            speedMultiplier += 0.25f;
            StartCoroutine(blockUpInput());
        }

        if (im.isDirectionDownPressed())
        {
            speedMultiplier -= 0.25f;
            StartCoroutine(blockDownInput());
        }


    }


    private void LateUpdate()
    {
        float screenDelta = (topRight.transform.position.y - bottomLeft.transform.position.y);

        float finalSpeed = (screenDelta * INVERSE_TIME_TO_REACH_TOP_SCRREN * speedMultiplier) * Time.deltaTime;

        Vector2 spawnPos = spawnPoint.transform.position;
        spawnPoint.transform.Translate(Vector2.up * finalSpeed);
    }

    #region Coroutines

    private IEnumerator blockDownInput()
    {
        blockedInput |= InputInfo.DOWN;
        yield return new WaitForSeconds(inputDelay);
        blockedInput = blockedInput & ~InputInfo.DOWN;
    }

    private IEnumerator blockUpInput()
    {
        blockedInput |= InputInfo.UP;
        yield return new WaitForSeconds(inputDelay);
        blockedInput = blockedInput & ~InputInfo.UP;
    }


    #endregion
}
