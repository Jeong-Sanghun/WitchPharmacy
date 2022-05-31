using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TutorialRoomCounterManager : TutorialManagerParent
{
    [SerializeField]
    TutorialCounterManager counterManager;
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
    Transform visitorBallonGlowParent;
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
    Image toRoomButtonGlow;
    [SerializeField]
    Image roomSymptomChartGlow;
    [SerializeField]
    SpriteRenderer trayGlow;
    [HideInInspector]
    public Image frostItemGlow;
    [HideInInspector]
    public Image desireItemGlow;

    [SerializeField]
    Image fireIconGlow;
    [SerializeField]
    Image waterIconGlow;
    [SerializeField]
    Image potGlow;

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

        visitorText.gameObject.SetActive(false);
        ruelliaText.gameObject.SetActive(false);
        visitorNameText.gameObject.SetActive(false);
        ruelliaNameText.gameObject.SetActive(false);
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
                if (nowCharacter != CharacterName.Ruellia)
                {
                    visitorNameText.text = characterIndexToName.NameTranslator(nowCharacter, languagePack);
                }
                if (nowCharacter == CharacterName.Ruellia)
                {
                    nowTextComponent = ruelliaText;
                }
                else
                {
                    nowTextComponent = visitorText;
                }
                if (isBallonActive == false)
                {
                    BallonActiveTrue();
                    isDialogStopping = true;
                    StartCoroutine(InvokerCoroutine(1, NextDialog));
                    return;
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
        visitorNameText.gameObject.SetActive(true);
        ruelliaNameText.gameObject.SetActive(true);
        StartCoroutine(sceneManager.FadeModule_Text(visitorText, 0, 1, 1));
        StartCoroutine(sceneManager.FadeModule_Text(ruelliaText, 0, 1, 1));
        StartCoroutine(sceneManager.FadeModule_Text(visitorNameText, 0, 1, 1));
        StartCoroutine(sceneManager.FadeModule_Text(ruelliaNameText, 0, 1, 1));
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
        StartCoroutine(sceneManager.FadeModule_Text(visitorText, 1, 0, 1));
        StartCoroutine(sceneManager.FadeModule_Text(ruelliaText, 1, 0, 1));
        StartCoroutine(sceneManager.FadeModule_Text(visitorNameText, 1, 0, 1));
        StartCoroutine(sceneManager.FadeModule_Text(ruelliaNameText, 1, 0, 1));

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
            case ActionKeyword.ToRoomButtonGlow:
                ToRoomButtonGlow();
                break;
            case ActionKeyword.RoomSymptomChartGlow:
                RoomSymptomChartGlow();
                break;
            case ActionKeyword.TrayGlow:
                TrayGlow();
                break;
            case ActionKeyword.ItemForceChoose:
                ItemForceChoose();
                break;
            case ActionKeyword.FireIconGlow:
                FireIconGlow();
                break;
            case ActionKeyword.WaterSubIconGlow:
                WaterSubIconGlow();
                break;
            case ActionKeyword.AddDesireGlow:
                AddDesireGlow();
                break;
            case ActionKeyword.AddFrostGlow:
                AddFrostGlow();
                break;
            case ActionKeyword.CookButtonClick:
                CookButtonClick();
                break;
            case ActionKeyword.GetCoin:
                GetCoin();
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
        Glow(visitorBallonGlow,visitorBallonGlowParent, (int)nowAction.parameter);
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
    void ToRoomButtonGlow()
    {
        TextFrameToggle(false);
        Glow(toRoomButtonGlow, (int)nowAction.parameter);
        nowGlow = ActionKeyword.ToRoomButtonGlow;
        isGlowing[(int)ActionKeyword.ToRoomButtonGlow] = true;
        BallonActiveFalse();
    }
    void RoomSymptomChartGlow()
    {
        TextFrameToggle(false);
        Glow(roomSymptomChartGlow, (int)nowAction.parameter);
        nowGlow = ActionKeyword.RoomSymptomChartGlow;
        isGlowing[(int)ActionKeyword.RoomSymptomChartGlow] = true;
    }
    void TrayGlow()
    {
        TextFrameToggle(false);
        Glow(trayGlow, (int)nowAction.parameter);
        nowGlow = ActionKeyword.TrayGlow;
        isGlowing[(int)ActionKeyword.TrayGlow] = true;
    }

    void ItemForceChoose()
    {
        TextFrameToggle(false);
        Glow(desireItemGlow, (int)nowAction.parameter);
        nowGlow = ActionKeyword.ItemForceChoose;
        isGlowing[(int)ActionKeyword.ItemForceChoose] = true;
    }

    void FireIconGlow()
    {
        TextFrameToggle(false);
        Glow(fireIconGlow, (int)nowAction.parameter);
        nowGlow = ActionKeyword.FireIconGlow;
        isGlowing[(int)ActionKeyword.FireIconGlow] = true;
    }
    void WaterSubIconGlow()
    {
        TextFrameToggle(false);
        Glow(waterIconGlow, (int)nowAction.parameter);
        nowGlow = ActionKeyword.WaterSubIconGlow;
        isGlowing[(int)ActionKeyword.WaterSubIconGlow] = true;
    }
    void AddDesireGlow()
    {
        TextFrameToggle(false);
        Glow(desireItemGlow, (int)nowAction.parameter);
        nowGlow = ActionKeyword.AddDesireGlow;
        isGlowing[(int)ActionKeyword.AddDesireGlow] = true;
    }
    void AddFrostGlow()
    {
        TextFrameToggle(false);
        Glow(desireItemGlow, (int)nowAction.parameter);
        nowGlow = ActionKeyword.AddFrostGlow;
        isGlowing[(int)ActionKeyword.AddFrostGlow] = true;
    }
    void CookButtonClick()
    {
        TextFrameToggle(false);
        Glow(potGlow, (int)nowAction.parameter);
        nowGlow = ActionKeyword.CookButtonClick;
        isGlowing[(int)ActionKeyword.CookButtonClick] = true;
    }
    void GetCoin()
    {
        counterManager.CoinGain((int)nowAction.parameter);
    }

}
