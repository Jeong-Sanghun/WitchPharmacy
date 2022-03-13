using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialTabletManager : TutorialManagerParent
{

    [SerializeField]
    TabletTreeterManager tabletTreeterManager;
    [SerializeField]
    TabletCariManager tabletCariManager;
    [SerializeField]
    GameObject tabletCanvasParent;
    [SerializeField]
    GameObject wholeTabletParent;
    [SerializeField]
    GameObject homeButton;
    [SerializeField]
    GameObject[] buttonHighlightObject;
    [SerializeField]
    GameObject tabletHighlightObject;

    [SerializeField]
    Image tabletButtonGlow;
    [SerializeField]
    Image cariButtonGlow;
    [SerializeField]
    Image treeterButtonGlow;
    [SerializeField]
    Image treeterPostButtonGlow;
    [SerializeField]
    Image tabletExitButtonGlow;
    

    // Start is called before the first frame update


    protected override void Start()
    {
        base.Start();
        dialogWrapper = jsonManager.ResourceDataLoad<TutorialDialogWrapper>("Tutorial/Dialog/Tablet");
        dialogWrapper.Parse();
        StartCoroutine(InvokerCoroutine(1, NextDialog));
        wholeTabletParent.SetActive(false);

    }

    public void OnTabletButton(bool open)
    {
        tabletCanvasParent.SetActive(open);
    }
    

    public void ButtonHighlightActive(bool active)
    {
        tabletHighlightObject.SetActive(active);
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

    void TabletButtonGlow()
    {
        TextFrameToggle(false);
        Glow(tabletButtonGlow, (int)nowAction.parameter);
        nowGlow = ActionKeyword.TabletButtonGlow;
        isGlowing[(int)ActionKeyword.TabletButtonGlow] = true;
    }

    void CariButtonGlow()
    {
        TextFrameToggle(false);
        Glow(cariButtonGlow, (int)nowAction.parameter);
        nowGlow = ActionKeyword.CariButtonGlow;
        isGlowing[(int)ActionKeyword.CariButtonGlow] = true;
    }
    void TreeterButtonGlow()
    {
        TextFrameToggle(false);
        Glow(treeterButtonGlow, (int)nowAction.parameter);
        nowGlow = ActionKeyword.TreeterButtonGlow;
        isGlowing[(int)ActionKeyword.TreeterButtonGlow] = true;
    }
    void TreeterPostButtonGlow()
    {
        TextFrameToggle(false);
        Glow(treeterPostButtonGlow, (int)nowAction.parameter);
        nowGlow = ActionKeyword.TreeterPostButtonGlow;
        isGlowing[(int)ActionKeyword.TreeterPostButtonGlow] = true;
    }
    void TabletExitButtonGlow()
    {
        TextFrameToggle(false);
        Glow(tabletExitButtonGlow, (int)nowAction.parameter);
        nowGlow = ActionKeyword.TabletExitButtonGlow;
        isGlowing[(int)ActionKeyword.TabletExitButtonGlow] = true;
    }




}
