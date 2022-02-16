using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialRoomCounterManager : TutorialManagerParent
{
    [SerializeField]
    Image visitorBallonImage;
    [SerializeField]
    Image ruelliaBallonImage;

    bool isButtonActive;

    [SerializeField]
    Text visitorNameText;
    [SerializeField]
    Text visitorText;
    [SerializeField]
    Text ruelliaText;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        dialogWrapper = jsonManager.ResourceDataLoad<TutorialDialogWrapper>("Tutorial/Dialog/RoomCounter");
        dialogWrapper.Parse();
        StartCoroutine(InvokerCoroutine(1, NextDialog));
        isButtonActive = false;
    }



    protected override void SetDialogText()
    {
        switch (nowType)
        {
            case DialogType.Dialog:
                TextFrameToggle(true);
                nowTextComponent = dialogText;
                characterImage.sprite = characterIndexToName.GetSprite(nowCharacter, nowFeeling);
                break;
            case DialogType.VisitorDialog:
                TextFrameToggle(false);
                StartCoroutine(InvokerCoroutine(1,BallonActiveTrue));

                if(nowCharacter == CharacterName.Ruellia)
                {
                    nowTextComponent = ruelliaText;
                    Debug.Log("왜안일어나");
                }
                else
                {
                    nowTextComponent = visitorText;
                    Debug.Log("왜안일");
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
        if (isButtonActive)
        {
            return;
        }
        isButtonActive = true;
        visitorText.gameObject.SetActive(true);
        ruelliaText.gameObject.SetActive(true);
        visitorBallonImage.color = new Color(1, 1, 1, 0);
        ruelliaBallonImage.color = new Color(1, 1, 1, 0);
        StartCoroutine(sceneManager.FadeModule_Image(ruelliaBallonImage.gameObject, 0, 1, 1));
        StartCoroutine(sceneManager.FadeModule_Image(visitorBallonImage.gameObject, 0, 1, 1));


    }
    void BallonActiveFalse()
    {
        if (!isButtonActive)
        {
            return;
        }
        isButtonActive = false;
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
                StartCoroutine(InvokerCoroutine(1, NextDialog));
                break;
        }
    }


}
