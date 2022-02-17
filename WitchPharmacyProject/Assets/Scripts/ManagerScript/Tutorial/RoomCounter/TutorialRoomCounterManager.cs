using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TutorialRoomCounterManager : TutorialManagerParent
{
    [SerializeField]
    Image visitorBallonImage;
    [SerializeField]
    Image ruelliaBallonImage;


    bool isBallonActive;



    [SerializeField]
    Text visitorNameText;
    [SerializeField]
    Text visitorText;
    [SerializeField]
    Text ruelliaText;
    [SerializeField]
    Text ruelliaNameText;

    [SerializeField]
    Image visitorBallonGlow;
    [SerializeField]
    Image bookGlow;
    [SerializeField]
    Image rightPageGlow;
    [SerializeField]
    Image effectIconGlow;
    [SerializeField]
    Image exitGlow;
    [SerializeField]
    Image symptomChartGlow;
    [SerializeField]
    Image waterPlusGlow;
    [SerializeField]
    Image counterChartExitButtonGlow;

    [SerializeField]
    GameObject visitorObject;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        dialogWrapper = jsonManager.ResourceDataLoad<TutorialDialogWrapper>("Tutorial/Dialog/RoomCounter");
        dialogWrapper.Parse();
        StartCoroutine(InvokerCoroutine(1, NextDialog));
        ruelliaNameText.text = characterIndexToName.NameTranslator(CharacterName.Ruellia, languagePack);
        isBallonActive = false;

    }



    protected override void SetDialogText()
    {
        switch (nowType)
        {
            case DialogType.Dialog:
                TextFrameToggle(true);
                nowTextComponent = dialogText;
                characterImage.sprite = characterIndexToName.GetSprite(nowCharacter, nowFeeling);
                dialogNameText.text = characterIndexToName.NameTranslator(nowCharacter, languagePack);
                characterImage.SetNativeSize();
                break;
            case DialogType.VisitorDialog:
                TextFrameToggle(false);
                BallonActiveTrue();
                if (nowCharacter == CharacterName.Ruellia)
                {
                    nowTextComponent = ruelliaText;
                }
                else
                {
                    nowTextComponent = visitorText;
                    visitorNameText.text = characterIndexToName.NameTranslator(nowCharacter, languagePack);
                }
                break;
            default:
                //일단 암것도 하지말아봐.
                break;
        }


    }
    void BallonActiveTrue()
    {
        if (isBallonActive)
        {
            return;
        }
        isBallonActive = true;
        visitorText.gameObject.SetActive(true);
        ruelliaText.gameObject.SetActive(true);
        visitorText.text = "";
        ruelliaText.text = "";
        visitorBallonImage.color = new Color(1, 1, 1, 0);
        ruelliaBallonImage.color = new Color(1, 1, 1, 0);
        StartCoroutine(sceneManager.FadeModule_Image(ruelliaBallonImage.gameObject, 0, 1, 1));
        StartCoroutine(sceneManager.FadeModule_Image(visitorBallonImage.gameObject, 0, 1, 1));


    }
    void BallonActiveFalse()
    {
        if (!isBallonActive)
        {
            return;
        }
        isBallonActive = false;
        visitorBallonImage.color = new Color(1, 1, 1, 1);
        ruelliaBallonImage.color = new Color(1, 1, 1, 1);
        StartCoroutine(sceneManager.FadeModule_Image(ruelliaBallonImage.gameObject, 1, 0, 1));
        StartCoroutine(sceneManager.FadeModule_Image(visitorBallonImage.gameObject, 1, 0, 1));


        //visitorText.gameObject.SetActive(false);
        //ruelliaText.gameObject.SetActive(false);
    }

    protected override void OverrideAction()
    {
        base.OverrideAction();
        switch (nowAction.action)
        {
            case ActionKeyword.VisitorUp:
                VisitorUp();
                break;
            case ActionKeyword.VisitorBallonGlow:
                VisitorBallonGlow();
                break;
            case ActionKeyword.BookGlow:
                BookGlow();
                break;
            case ActionKeyword.RightPageGlow:
                RightPageGlow();
                break;
            case ActionKeyword.EffectIconGlow:
                EffectIconGlow();
                break;
            case ActionKeyword.ExitGlow:
                ExitGlow();
                break;
            case ActionKeyword.CounterSymptomChartGlow:
                SymptomChartGlow();
                break;
            case ActionKeyword.WaterPlusGlow:
                WaterPlusGlow();
                break;
            case ActionKeyword.CounterChartExitButtonGlow:
                CounterChartExitButtonGlow();
                break;
        }
    }
    

    void VisitorUp()
    {
        StartCoroutine(sceneManager.MoveModule_Linear(visitorObject, 
            new Vector3(visitorObject.transform.position.x, 0, 0), 0.7f));
        StartCoroutine(InvokerCoroutine(1, NextDialog));
    }

    void VisitorBallonGlow()
    {
        Glow(visitorBallonGlow, (int)nowAction.parameter);
        NextDialog();
    }
    void BookGlow()
    {
        TextFrameToggle(false);
        Glow(bookGlow, (int)nowAction.parameter);
        nowGlow = ActionKeyword.BookGlow;
        isGlowing[(int)ActionKeyword.BookGlow] = true;
    }

    void RightPageGlow()
    {
        TextFrameToggle(false);
        Glow(rightPageGlow, (int)nowAction.parameter);
        nowGlow = ActionKeyword.RightPageGlow;
        isGlowing[(int)ActionKeyword.RightPageGlow] = true;
    }
    void EffectIconGlow()
    {
        TextFrameToggle(false);
        Glow(effectIconGlow, (int)nowAction.parameter);
        StartCoroutine(InvokerCoroutine(2, NextDialog));
    }
    void ExitGlow()
    {
        TextFrameToggle(false);
        Glow(exitGlow, (int)nowAction.parameter);
        nowGlow = ActionKeyword.ExitGlow;
        isGlowing[(int)ActionKeyword.ExitGlow] = true;
    }
    void SymptomChartGlow()
    {
        TextFrameToggle(false);
        Glow(symptomChartGlow, (int)nowAction.parameter);
        nowGlow = ActionKeyword.CounterSymptomChartGlow;
        isGlowing[(int)ActionKeyword.CounterSymptomChartGlow] = true;
    }
    void WaterPlusGlow()
    {
        TextFrameToggle(false);
        Glow(waterPlusGlow, (int)nowAction.parameter);
        nowGlow = ActionKeyword.WaterPlusGlow;
        isGlowing[(int)ActionKeyword.WaterPlusGlow] = true;
    }
    void CounterChartExitButtonGlow()
    {
        TextFrameToggle(false);
        Glow(counterChartExitButtonGlow, (int)nowAction.parameter);
        nowGlow = ActionKeyword.CounterChartExitButtonGlow;
        isGlowing[(int)ActionKeyword.CounterChartExitButtonGlow] = true;
    }


}
