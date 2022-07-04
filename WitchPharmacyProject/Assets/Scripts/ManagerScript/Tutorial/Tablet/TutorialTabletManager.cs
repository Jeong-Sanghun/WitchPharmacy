using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialTabletManager : TutorialManagerParent
{

    [SerializeField]
    TutorialTreeterManager tabletTreeterManager;
    [SerializeField]
    GameObject tabletCanvasParent;
    [SerializeField]
    GameObject treeterCanvas;

    [SerializeField]
    GameObject minimizeParent;
    [SerializeField]
    Text minimizeTalkText;

    [SerializeField]
    GameObject maximizeParent;
    [SerializeField]
    GameObject[] maximizeButtonObjectArray;
    [SerializeField]
    Text[] maximizeButtonTextArray;
    [SerializeField]
    Text maximizeTalkText;

    [SerializeField]
    Image tabletButtonGlow;
    [SerializeField]
    Transform tabletButtonGlowParent;

    [SerializeField]
    Image cariButtonGlow;
    [SerializeField]
    Transform cariButtonGlowParent;

    [SerializeField]
    Image treeterButtonGlow;
    [SerializeField]
    Transform treeterButtonGlowParent;

    [SerializeField]
    Image treeterPostButtonGlow;
    [SerializeField]
    Transform treeterPostButtonGlowParent;

    [SerializeField]
    Image tabletExitButtonGlow;
    [SerializeField]
    Transform tabletExitButtonGlowParent;


    // Start is called before the first frame update


    protected override void Start()
    {
        base.Start();
        dialogWrapper = jsonManager.ResourceDataLoad<TutorialDialogWrapper>("Tutorial/Dialog/Tablet");
        dialogWrapper.Parse();
        StartCoroutine(InvokerCoroutine(1, NextDialog));

    }

    public void OnTreeterButton(bool open)
    {
        if (isGlowing[(int)ActionKeyword.TreeterButtonGlow] == false)
        {
            return;
        }
        treeterCanvas.SetActive(open);
    }
    public void OnTabletButton(bool open)
    {
        if (open)
        {
            if (isGlowing[(int)ActionKeyword.TabletButtonGlow] == false)
            {
                return;
            }
        }
        else
        {
            if (isGlowing[(int)ActionKeyword.TabletExitButtonGlow] == false)
            {
                return;
            }
        }

        tabletCanvasParent.SetActive(open);
    }



    protected override void SetDialogText()
    {
        switch (nowType)
        {
            case DialogType.Dialog:
                TextFrameToggle(true);
                screenTouchCanvas.SetActive(true);
                nowTextComponent = dialogText;
                characterImage.sprite = characterIndexToName.GetSprite(nowCharacter, nowFeeling);
                dialogNameText.text = characterIndexToName.NameTranslator(nowCharacter, languagePack);
                characterImage.SetNativeSize();
                break;
            case DialogType.CariDialog:
                TextFrameToggle(false);
                screenTouchCanvas.SetActive(false);
                SetHomeCariDialog();

                break;
            default:
                //일단 암것도 하지말아봐.
                break;
        }


    }


    protected override void OverrideAction()
    {
        base.OverrideAction();
        switch (nowAction.action)
        {
            case ActionKeyword.TabletButtonGlow:
                TabletButtonGlow();
                break;
            case ActionKeyword.CariButtonGlow:
                CariButtonGlow();
                break;
            case ActionKeyword.TreeterButtonGlow:
                TreeterButtonGlow();
                break;
            case ActionKeyword.TreeterPostButtonGlow:
                TreeterPostButtonGlow();
                break;
            case ActionKeyword.TabletExitButtonGlow:
                TabletExitButtonGlow();
                break;

        }
    }

    public void MaximizeButton()
    {
        if (isGlowing[(int)ActionKeyword.CariButtonGlow] == false)
        {
            return;
        }
        minimizeParent.SetActive(false);
        maximizeParent.SetActive(true);
    }

    public void MinimizeButton()
    {
        minimizeParent.SetActive(true);
        maximizeParent.SetActive(false);
    }

    void SetHomeCariDialog()
    {
        maximizeTalkText.text = dialogWrapper.dialogArray[nowDialogIndex].dialog;

        for (int i = 0; i < maximizeButtonObjectArray.Length; i++)
        {
            maximizeButtonObjectArray[i].SetActive(false);
        }
        for (int i = 0; i < dialogWrapper.dialogArray[nowDialogIndex].routeList.Count; i++)
        {
            maximizeButtonObjectArray[i].SetActive(true);
            maximizeButtonTextArray[i].text = dialogWrapper.dialogArray[nowDialogIndex].routeList[i].routeString;
        }


    }

    public void SetTreeterCariDialog()
    {
        maximizeTalkText.text = tabletTreeterManager.tutorialTreeterData.cariComment;

        for (int i = 0; i < maximizeButtonObjectArray.Length; i++)
        {
            maximizeButtonObjectArray[i].SetActive(false);
        }
    }


    public void CariNextDialogButton()
    {
        MinimizeButton();
        screenTouchCanvas.SetActive(true);
        NextDialog();
    }

    void TabletButtonGlow()
    {
        TextFrameToggle(false);
        Glow(tabletButtonGlow,tabletButtonGlowParent, (int)nowAction.parameterFloat);
        nowGlow = ActionKeyword.TabletButtonGlow;
        isGlowing[(int)ActionKeyword.TabletButtonGlow] = true;
    }

    void CariButtonGlow()
    {
        TextFrameToggle(false);
        screenTouchCanvas.SetActive(false);
        Glow(cariButtonGlow,cariButtonGlowParent, (int)nowAction.parameterFloat);
        nowGlow = ActionKeyword.CariButtonGlow;
        isGlowing[(int)ActionKeyword.CariButtonGlow] = true;
    }
    void TreeterButtonGlow()
    {
        TextFrameToggle(false);
        SetTreeterCariDialog();
        screenTouchCanvas.SetActive(false);
        Glow(treeterButtonGlow,treeterButtonGlowParent, (int)nowAction.parameterFloat);
        nowGlow = ActionKeyword.TreeterButtonGlow;
        isGlowing[(int)ActionKeyword.TreeterButtonGlow] = true;
    }
    void TreeterPostButtonGlow()
    {
        TextFrameToggle(false);
        screenTouchCanvas.SetActive(false);
        Glow(treeterPostButtonGlow,treeterPostButtonGlowParent, (int)nowAction.parameterFloat);
        nowGlow = ActionKeyword.TreeterPostButtonGlow;
        isGlowing[(int)ActionKeyword.TreeterPostButtonGlow] = true;
    }
    void TabletExitButtonGlow()
    {
        TextFrameToggle(false);
        screenTouchCanvas.SetActive(false);
        Glow(tabletExitButtonGlow,tabletExitButtonGlowParent, (int)nowAction.parameterFloat);
        nowGlow = ActionKeyword.TabletExitButtonGlow;
        isGlowing[(int)ActionKeyword.TabletExitButtonGlow] = true;
    }




}
