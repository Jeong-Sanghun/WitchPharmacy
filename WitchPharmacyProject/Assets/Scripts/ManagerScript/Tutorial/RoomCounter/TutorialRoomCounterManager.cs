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
    Transform bookGlowParent;

    [SerializeField]
    Image rightPageGlow;
    [SerializeField]
    Transform rightPageGlowParent;

    [SerializeField]
    Image effectIconGlow;
    [SerializeField]
    Transform effectIconGlowParent;

    [SerializeField]
    Image exitGlow;
    [SerializeField]
    Transform exitGlowParent;

    [SerializeField]
    Image symptomChartGlow;
    [SerializeField]
    Transform symptomChartGlowParent;

    [SerializeField]
    Image waterPlusGlow;
    [SerializeField]
    Transform waterPlusGlowParent;

    [SerializeField]
    Image counterChartExitButtonGlow;
    [SerializeField]
    Transform counterChartExitButtonGlowParent;

    [SerializeField]
    Image toRoomButtonGlow;
    [SerializeField]
    Transform toRoomButtonGlowParent;

    [SerializeField]
    Image roomSymptomChartGlow;
    [SerializeField]
    Transform roomSymptomChartGlowParent;

    [SerializeField]
    SpriteRenderer trayGlow;
    [SerializeField]
    Transform trayGlowParent;

    [HideInInspector]
    public Image frostItemGlow;
    [HideInInspector]
    public Transform frostItemGlowParent;
    [HideInInspector]
    public Image desireItemGlow;
    [HideInInspector]
    public Transform desireItemGlowParent;

    [SerializeField]
    Image fireIconGlow;
    [SerializeField]
    Transform fireIconGlowParent;

    [SerializeField]
    Image waterIconGlow;
    [SerializeField]
    Transform waterIconGlowParent;

    [SerializeField]
    Image potGlow;
    [SerializeField]
    Transform potGlowParent;

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
        Glow(visitorBallonGlow,visitorBallonGlowParent, (int)nowAction.parameterFloat);
        NextDialog();
    }
    void BookGlow()
    {
        TextFrameToggle(false);
        Glow(bookGlow,bookGlowParent, (int)nowAction.parameterFloat);
        nowGlow = ActionKeyword.BookGlow;
        isGlowing[(int)ActionKeyword.BookGlow] = true;
    }

    void RightPageGlow()
    {
        TextFrameToggle(false);
        Glow(rightPageGlow,rightPageGlowParent, (int)nowAction.parameterFloat);
        nowGlow = ActionKeyword.RightPageGlow;
        isGlowing[(int)ActionKeyword.RightPageGlow] = true;
    }
    void EffectIconGlow()
    {
        TextFrameToggle(false);
        Glow(effectIconGlow,effectIconGlowParent, (int)nowAction.parameterFloat);
        StartCoroutine(InvokerCoroutine(2, NextDialog));
    }

    public void SetEffectIconParent(Transform effectIconParent)
    {
        effectIconGlowParent = effectIconParent;
        effectIconGlow.transform.SetParent(effectIconParent);
    }

    void ExitGlow()
    {
        TextFrameToggle(false);
        Glow(exitGlow,exitGlowParent, (int)nowAction.parameterFloat);
        nowGlow = ActionKeyword.ExitGlow;
        isGlowing[(int)ActionKeyword.ExitGlow] = true;
    }
    void SymptomChartGlow()
    {
        TextFrameToggle(false);
        Glow(symptomChartGlow,symptomChartGlowParent, (int)nowAction.parameterFloat);
        nowGlow = ActionKeyword.CounterSymptomChartGlow;
        isGlowing[(int)ActionKeyword.CounterSymptomChartGlow] = true;
    }
    void WaterPlusGlow()
    {
        TextFrameToggle(false);
        Glow(waterPlusGlow,waterPlusGlowParent, (int)nowAction.parameterFloat);
        nowGlow = ActionKeyword.WaterPlusGlow;
        isGlowing[(int)ActionKeyword.WaterPlusGlow] = true;
    }
    void CounterChartExitButtonGlow()
    {
        TextFrameToggle(false);
        Glow(counterChartExitButtonGlow,counterChartExitButtonGlowParent, (int)nowAction.parameterFloat);
        nowGlow = ActionKeyword.CounterChartExitButtonGlow;
        isGlowing[(int)ActionKeyword.CounterChartExitButtonGlow] = true;
    }
    void ToRoomButtonGlow()
    {
        TextFrameToggle(false);
        Glow(toRoomButtonGlow,toRoomButtonGlowParent, (int)nowAction.parameterFloat);
        nowGlow = ActionKeyword.ToRoomButtonGlow;
        isGlowing[(int)ActionKeyword.ToRoomButtonGlow] = true;
        BallonActiveFalse();
    }
    void RoomSymptomChartGlow()
    {
        TextFrameToggle(false);
        Glow(roomSymptomChartGlow,roomSymptomChartGlowParent, (int)nowAction.parameterFloat,true);
        nowGlow = ActionKeyword.RoomSymptomChartGlow;
        isGlowing[(int)ActionKeyword.RoomSymptomChartGlow] = true;
    }
    void TrayGlow()
    {
        TextFrameToggle(false);
        Glow(trayGlow,trayGlowParent, (int)nowAction.parameterFloat);
        nowGlow = ActionKeyword.TrayGlow;
        isGlowing[(int)ActionKeyword.TrayGlow] = true;
    }

    void ItemForceChoose()
    {
        TextFrameToggle(false);
        Glow(desireItemGlow,desireItemGlowParent, (int)nowAction.parameterFloat,true);
        nowGlow = ActionKeyword.ItemForceChoose;
        isGlowing[(int)ActionKeyword.ItemForceChoose] = true;
    }

    void FireIconGlow()
    {
        TextFrameToggle(false);
        Glow(fireIconGlow,fireIconGlowParent, (int)nowAction.parameterFloat,true);
        nowGlow = ActionKeyword.FireIconGlow;
        isGlowing[(int)ActionKeyword.FireIconGlow] = true;
    }
    void WaterSubIconGlow()
    {
        TextFrameToggle(false);
        Glow(waterIconGlow,waterIconGlowParent, (int)nowAction.parameterFloat,true);
        nowGlow = ActionKeyword.WaterSubIconGlow;
        isGlowing[(int)ActionKeyword.WaterSubIconGlow] = true;
    }
    void AddDesireGlow()
    {
        TextFrameToggle(false);
        Glow(desireItemGlow,desireItemGlowParent, (int)nowAction.parameterFloat,true);
        nowGlow = ActionKeyword.AddDesireGlow;
        isGlowing[(int)ActionKeyword.AddDesireGlow] = true;
    }
    void AddFrostGlow()
    {
        TextFrameToggle(false);
        Glow(frostItemGlow,frostItemGlowParent, (int)nowAction.parameterFloat,true);
        nowGlow = ActionKeyword.AddFrostGlow;
        isGlowing[(int)ActionKeyword.AddFrostGlow] = true;
    }
    void CookButtonClick()
    {
        TextFrameToggle(false);
        Glow(potGlow,potGlowParent, (int)nowAction.parameterFloat,true);
        nowGlow = ActionKeyword.CookButtonClick;
        isGlowing[(int)ActionKeyword.CookButtonClick] = true;
    }
    void GetCoin()
    {
        counterManager.CoinGain((int)nowAction.parameterFloat);
    }

}
