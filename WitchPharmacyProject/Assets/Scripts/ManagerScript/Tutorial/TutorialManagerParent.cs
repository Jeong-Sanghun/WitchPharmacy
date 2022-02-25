using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Rendering.PostProcessing;

public class TutorialManagerParent : MonoBehaviour
{
    //protected TutorialDialogWrapper nowWrapper;

    protected GameManager gameManager;
    protected JsonManager jsonManager;
    [SerializeField]
    protected TutorialDialogWrapper dialogWrapper;
    protected SaveDataClass saveData;

    protected SceneManager sceneManager;
    protected UILanguagePack languagePack;
    protected CharacterIndexToName characterIndexToName;

    [SerializeField]
    protected Image fadeInImage;
    [SerializeField]
    protected Image textFrameImage;
    [SerializeField]
    protected Image characterImage;
    [SerializeField]
    protected Text dialogText;
    [SerializeField]
    protected Text dialogNameText;
    protected Text nowTextComponent;
    [SerializeField]
    protected Text systemText;
    [SerializeField]
    GameObject[] routeButtonArray;
    [SerializeField]
    protected PostProcessVolume blurVolume;
    [SerializeField]
    protected GameObject screenTouchCanvas;
    



    public bool[] isGlowing;
    protected ActionKeyword nowGlow;
    protected int nowDialogIndex;
    protected bool isDialogStopping;
    //bool isTalkingSystem;
    protected bool isStartOfWrapper;
    protected bool isStopActionable;
    bool isRouteButtonAble;
    TutorialDialog routeDialog;
    bool isRouting;
    bool textFrameTransparent;
    bool isStarted;
    protected bool dialogEnd;
    protected CharacterName nowCharacter;
    protected DialogType nowType;
    protected CharacterFeeling nowFeeling;
    protected ActionClass nowAction;


    protected virtual void Start()
    {
        gameManager = GameManager.singleton;
        sceneManager = SceneManager.inst;
        jsonManager = new JsonManager();
        characterIndexToName = new CharacterIndexToName();
        languagePack = gameManager.languagePack;
        isStartOfWrapper = true;
        fadeInImage.gameObject.SetActive(true);
        fadeInImage.color = new Color(0, 0, 0, 1);
        textFrameImage.color = new Color(1, 1, 1, 0);
        characterImage.color = new Color(1, 1, 1, 0);
        dialogNameText.color= new Color(1, 1, 1, 0);
        dialogText.text = "";
        dialogNameText.text = "";
        systemText.text = "";
        textFrameTransparent = true;
        dialogEnd = false;
        isStopActionable = false;
        isDialogStopping = true;
        isStarted = false;
        isRouteButtonAble = false;
        routeDialog = null;
        isRouting = false;
        nowCharacter = CharacterName.Null;
        nowType = DialogType.Null;
        nowFeeling = CharacterFeeling.Null;
        nowGlow = ActionKeyword.Null;


        saveData = gameManager.saveData;
        StartCoroutine(sceneManager.FadeModule_Image(fadeInImage.gameObject, 1, 0, 0.5f)) ;

        isGlowing = new bool[Enum.GetValues(typeof(ActionKeyword)).Length];

        for (int i = 0; i < isGlowing.Length; i++)
        {
            isGlowing[i] = false;
        }


    }

    protected virtual void Update()
    {

    }



    public void ScreenTouchEvent()
    {
        if (isStarted == false || sceneManager.nowTexting || isRouting == true)
        {
            
            return;
        }
        if (isDialogStopping == false)
        {
           
            NextDialog();
        }
        else if (isStopActionable == true)
        {
            OnActionKeyword();
        }
    }

    protected virtual void NextDialog()
    {
        if (dialogEnd == true || isRouting == true)
        {
            return;
        }
        if (nowDialogIndex >= dialogWrapper.dialogArray.Length)
        {
            return;
        }
        
        TutorialDialog nowDialog = dialogWrapper.dialogArray[nowDialogIndex];
        screenTouchCanvas.SetActive(true);


        PrintDialog();



        if (nowDialog.actionKeyword != null)
        {
            isStartOfWrapper = true;
            StartCoroutine(CheckStopPointTextEnd());
            nowAction = dialogWrapper.dialogArray[nowDialogIndex].action;
        }

        if (nowDialog.routeList != null)
        {
            isRouting = true;
            isStartOfWrapper = true;
            routeDialog = nowDialog;
            StartCoroutine(CheckRoutePointTextEnd());

        }


        if (nowDialogIndex == dialogWrapper.dialogArray.Length)
        {
            isDialogStopping = true;
            dialogEnd = true;
        }
        else
        {
            nowDialogIndex++;
        }

    }

    protected virtual void PrintDialog()
    {
        TutorialDialog nowDialog = dialogWrapper.dialogArray[nowDialogIndex];
        bool isPropertyChanged = false;
        bool lastTextFrameTransparent = textFrameTransparent;
        bool isStart = isStartOfWrapper;
        if (isStart)
        {
            if (nowDialog.character != CharacterName.Null)
            {
                nowCharacter = nowDialog.character;
                isPropertyChanged = true;
            }

            isStartOfWrapper = false;
        }
        else if (nowDialog.character != nowCharacter && nowDialog.character != CharacterName.Null)
        {
            nowCharacter = nowDialog.character;
            isPropertyChanged = true;
        }

        if (isStart)
        {
            if (nowDialog.type != DialogType.Null)
            {
                nowType = nowDialog.type;
                isPropertyChanged = true;
            }

            isStartOfWrapper = false;
        }
        else if (nowDialog.type != nowType && nowDialog.type != DialogType.Null)
        {
            nowType = nowDialog.type;
            isPropertyChanged = true;
        }

        if (isStart)
        {
            if (nowDialog.feeling != CharacterFeeling.Null)
            {
                nowFeeling = nowDialog.feeling;
                isPropertyChanged = true;
            }
            isStartOfWrapper = false;

        }
        else if (nowDialog.feeling != nowFeeling && nowDialog.feeling != CharacterFeeling.Null)
        {
            nowFeeling = nowDialog.feeling;
            isPropertyChanged = true;
        }
        //else if (nowCharacter == Character.System || nowCharacter == Character.Message)
        //{
        //    isNewCharacter = true;
        //}
        isDialogStopping = false;
        if (nowDialog.dialog != null)
        {
            if (isPropertyChanged)
            {
                SetDialogText();
            }
        }

        if (nowDialog.dialog != null)
        {
            if(textFrameTransparent == false)
            {
                if (lastTextFrameTransparent != textFrameTransparent)
                {
                    sceneManager.nowTexting = true;
                    StartCoroutine(sceneManager.
                  AfterRunCoroutine(0.8f, sceneManager.LoadTextOneByOne(nowDialog.dialog, nowTextComponent)));

                }
                else
                {
                    StartCoroutine(sceneManager.LoadTextOneByOne(nowDialog.dialog, nowTextComponent));

                }
            }
            else
            {
                StartCoroutine(sceneManager.LoadTextOneByOne(nowDialog.dialog, nowTextComponent));
            }
            
        }
        
    }

    protected virtual void SetDialogText()
    {
        switch (nowType)
        {
            case DialogType.Dialog:
                TextFrameToggle(true);
                nowTextComponent = dialogText;
                break;
            default:
                //일단 암것도 하지말아봐.
                break;
        }
    }


    protected virtual void OnActionKeyword()
    {
        bool stop = true;
        if (nowAction.action == ActionKeyword.Jump)
        {
            stop = false;
        }
        OverrideAction();
        if (!stop)
        {
            isDialogStopping = false;
            //NextDialog();
            return;
        }
        isStopActionable = false;
        TextFrameToggle(false);
    }

    protected virtual void OverrideAction()
    {
        switch (nowAction.action)
        {
            case ActionKeyword.Delay:
                Debug.Log(nowAction.parameter);
                StartCoroutine(InvokerCoroutine(nowAction.parameter, NextDialog));
                break;
            case ActionKeyword.Jump:
                Debug.Log(nowAction.parameter);
                nowDialogIndex += (int)nowAction.parameter;
                break;
        }
    }



    //트루면 메인프레임이 켜짐.
    protected virtual void TextFrameToggle(bool mainActive)
    {
        if (!mainActive == textFrameTransparent)
        {
            return;
        }
        textFrameTransparent = !mainActive;
        if (textFrameTransparent)
        {
            StartCoroutine(sceneManager.FadeModule_Image(characterImage.gameObject, 1, 0, 0.7f));
            StartCoroutine(sceneManager.FadeModule_Image(textFrameImage.gameObject, 1, 0, 0.7f));
            StartCoroutine(sceneManager.FadeModule_Text(dialogNameText, 1, 0, 0.7f));
            StartCoroutine(sceneManager.FadeModule_Text(dialogText, 1, 0, 0.7f));
            //dialogText.gameObject.SetActive(false);
            //dialogNameText.gameObject.SetActive(false);
        }
        else
        {
            systemText.gameObject.SetActive(false);
            dialogText.gameObject.SetActive(true);
            dialogNameText.gameObject.SetActive(true);
            dialogText.text = "";
            StartCoroutine(sceneManager.FadeModule_Image(characterImage.gameObject, 0, 1, 0.7f));
            StartCoroutine(sceneManager.FadeModule_Image(textFrameImage.gameObject, 0, 1, 0.7f));
            StartCoroutine(sceneManager.FadeModule_Text(dialogNameText, 0, 1, 0.7f));
            StartCoroutine(sceneManager.FadeModule_Text(dialogText, 0, 1, 0.7f));
        }
    }


    protected IEnumerator InvokerCoroutine(float time, Action method)
    {
        yield return new WaitForSeconds(time);
        Debug.Log("어디여");
        isStarted = true;
        method();
    }

    protected void SetSystemTextFalse()
    {
        systemText.text = "";
        systemText.gameObject.SetActive(false);
    }

    protected void SetStopActionableTrue()
    {
        isStopActionable = true;
    }

    protected void SetDialogStopFalse()
    {
        isDialogStopping = false;

    }

    //protected void SetTalkingSystemFalse()
    //{
    //    isTalkingSystem = false;

    //}


    IEnumerator CheckStopPointTextEnd()
    {
        while (sceneManager.nowTexting)
        {
            yield return null;
        }
        isDialogStopping = true;
        isStopActionable = true;
    }


    IEnumerator CheckRoutePointTextEnd()
    {
        isDialogStopping = true;
        isStopActionable = false;
        while (sceneManager.nowTexting)
        {
            yield return null;
        }
        yield return null;
        isDialogStopping = true;
        isStopActionable = false;
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                break;
            }
            yield return null;
        }

        //TextFrameToggle(false);
        yield return new WaitForSeconds(0.8f);
        RouteButtonActive();

    }




    protected IEnumerator SceneEndCoroutine(string scene)
    {
        fadeInImage.gameObject.SetActive(true);
        //SaveUserData();
        StartCoroutine(sceneManager.FadeModule_Image(fadeInImage.gameObject, 0, 1, 1));
        yield return new WaitForSeconds(1f);
        sceneManager.LoadScene(scene);
    }

    void RouteButtonActive()
    {
        List<TutorialRoute> routeList = routeDialog.routeList;
        for (int i = 0; i < routeList.Count; i++)
        {
            routeButtonArray[i].SetActive(true);
        }


        List<Text> routeTextList = new List<Text>();
        isRouteButtonAble = false;


        StartCoroutine(sceneManager.VolumeModule(blurVolume, true, 1));
        for (int i = 0; i < routeList.Count; i++)
        {
            GameObject txtObj = routeButtonArray[i].transform.GetChild(0).gameObject;
            GameObject imgObj = routeButtonArray[i].transform.gameObject;
            Text txt = txtObj.GetComponent<Text>();
            Image img = imgObj.GetComponent<Image>();
            img.color = new Color(1, 1, 1, 0);
            txt.color = new Color(0, 0, 0, 0);
            txt.text = routeList[i].routeString;

            StartCoroutine(sceneManager.FadeModule_Image(img.gameObject, 0, 1, 1));
            StartCoroutine(sceneManager.FadeModule_Text(txt, 0, 1, 1));
        }
        StartCoroutine(InvokerCoroutine(1, RouteButtonAbleTrue));


    }

    public void OnRouteButton(int index)
    {

        if (isRouteButtonAble == true)
        {
            StartCoroutine(ButtonAnimCoroutine(index));
        }
        isRouteButtonAble = false;


    }

    IEnumerator ButtonAnimCoroutine(int index)
    {
        
        for (int i = 0; i < routeDialog.routeList.Count; i++)
        {
            GameObject obj = routeButtonArray[i];
            Text txt = obj.transform.GetChild(0).GetComponent<Text>();
            Image img = obj.GetComponent<Image>();
            if (i != index)
            {
                StartCoroutine(sceneManager.FadeModule_Image(img.gameObject, 1, 0, 1));
                StartCoroutine(sceneManager.FadeModule_Text(txt, 1, 0, 1));
            }
        }

        Vector3 targetSize = new Vector3(1.05f, 1.05f, 1);
        Vector3 originSize = Vector3.one;
        float timer = 0;
        while (timer < 1)
        {
            timer += Time.deltaTime * 6;
            routeButtonArray[index].transform.localScale = Vector3.Lerp(originSize, targetSize, timer);
            yield return null;
        }
        timer = 0;
        while (timer < 1)
        {
            timer += Time.deltaTime * 6;
            routeButtonArray[index].transform.localScale = Vector3.Lerp(targetSize, originSize, timer);
            yield return null;
        }
        routeButtonArray[index].transform.localScale = originSize;
        yield return new WaitForSeconds(0.1f);

        isDialogStopping = false;
        for(int i = 0; i < routeButtonArray.Length; i++)
        {
            routeButtonArray[i].SetActive(false);
        }
        StartCoroutine(sceneManager.VolumeModule(blurVolume, false, 1));
        nowDialogIndex += routeDialog.routeList[index].jump-1;
        isRouting = false;

        NextDialog();

    }

    void RouteButtonAbleTrue()
    {
        isRouteButtonAble = true;
    }

    protected void Glow(SpriteRenderer sprite,int param)
    {
        screenTouchCanvas.SetActive(false);
        StartCoroutine(GlowCoroutine(sprite, param));
    }

    protected void Glow(Image sprite,int param)
    {
        screenTouchCanvas.SetActive(false);
        StartCoroutine(GlowCoroutine(sprite, param));
    }

    protected IEnumerator GlowCoroutine(SpriteRenderer sprite,int param)
    {
        float timer = -0.5f;
        int one = 1;
        int targetIndex = param + nowDialogIndex;
        Color originColor = sprite.color;
        sprite.gameObject.SetActive(true);
        sprite.color = new Color(originColor.r, originColor.g, originColor.b, 0);
        while (nowDialogIndex != targetIndex)
        {
            timer += Time.deltaTime * one;
            sprite.color = new Color(originColor.r, originColor.g, originColor.b, timer+0.5f);
            if (Mathf.Abs(timer) >= 0.5f)
            {
                one *= -1;
            }
            yield return null;
        }
        sprite.gameObject.SetActive(false);

    }
    protected IEnumerator GlowCoroutine(Image sprite,int param)
    {
        float timer = -0.5f;
        int one = 1;
        int targetIndex = param + nowDialogIndex;
        Color originColor = sprite.color;
        sprite.gameObject.SetActive(true);
        sprite.color = new Color(originColor.r, originColor.g, originColor.b, 0);
        while(nowDialogIndex != targetIndex)
        {
            timer += Time.deltaTime * one;
            sprite.color = new Color(originColor.r, originColor.g, originColor.b, timer+0.5f);
            if (Mathf.Abs(timer) >= 0.5f)
            {
                one *= -1;
            }
            yield return null;
        }
        sprite.gameObject.SetActive(false);

    }

    public void GlowNextDialog(string action)
    {
        ActionKeyword nowAction = (ActionKeyword)Enum.Parse(typeof(ActionKeyword), action);
        if(nowAction != nowGlow)
        {
            return;
        }
        if (isGlowing[(int)nowGlow] == true)
        {
            NextDialog();

            isGlowing[(int)nowGlow] = false;
            nowGlow = ActionKeyword.Null;
        }
  
    }

}
