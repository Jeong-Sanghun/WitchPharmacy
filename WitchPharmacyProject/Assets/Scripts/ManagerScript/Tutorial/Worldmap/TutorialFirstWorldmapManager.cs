using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialFirstWorldmapManager : TutorialManagerParent
{
    [SerializeField]
    ExploreButtonManager buttonManager;
    [SerializeField]
    Image witchGraveMapiconGlow;

    protected override void Start()
    {
        base.Start();
        dialogWrapper = jsonManager.ResourceDataLoad<TutorialDialogWrapper>("Tutorial/Dialog/Worldmap1");
        dialogWrapper.Parse();
        StartCoroutine(InvokerCoroutine(1, NextDialog));

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
        }
    }

    protected override void OverrideAction()
    {
        base.OverrideAction();
        switch (nowAction.action)
        {
            case ActionKeyword.WitchGraveMapiconGlow:
                WitchGraveMapiconGlow();
                break;

        }
    }

    void WitchGraveMapiconGlow()
    {
        buttonManager.isTutorialGlowing = true;
        TextFrameToggle(false);
        screenTouchCanvas.SetActive(false);
        Glow(witchGraveMapiconGlow, 1);
    }

}
