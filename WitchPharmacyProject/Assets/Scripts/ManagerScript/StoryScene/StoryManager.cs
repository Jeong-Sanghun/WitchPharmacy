using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class StoryManager : MonoBehaviour
{
    [SerializeField] BlurManager blurManager;
    protected SceneManager sceneManager;
    GameManager gameManager;
    SaveDataClass saveData;

    //protected List<ConversationDialogBundle> conversationDialogBundleList;
    //[SerializeField]
    //ConversationDialogBundle nowBundle;
    //[SerializeField]
    //ConversationDialogWrapper nowWrapper;
    //ConversationRouter nowRouter;

        [SerializeField]
    StoryDialogBundle nowBundle;
    StoryDialog[] nowDialogArray;
    StoryDialog nowDialog;
    int nowDialogIndex;
  
    StoryParser storyParser;

    CharacterIndexToName characterIndexToName;
    [SerializeField]
    GameObject cameraObject;
    [SerializeField]
    Text blackOutText;

    [SerializeField]
    SpriteRenderer middleCharacterSprite;
    [SerializeField]
    SpriteRenderer[] characterSprite;
    [SerializeField]
    SpriteRenderer cutSceneSprite;
    [SerializeField]
    SpriteRenderer backgroundSprite;
    [SerializeField]
    Image popupSprite;

    [SerializeField]
    Text conversationText;
    [SerializeField]
    Text nameText;

    [SerializeField]
    GameObject textFrameObject;
    [SerializeField]
    GameObject nextButtonObject;



    [SerializeField]
    Text[] routeTextArray;
    [SerializeField]
    GameObject[] routeButtonArray;

    [SerializeField]
    GameObject toNextSceneButton;
    [SerializeField]
    GameObject fadeObject;
    [SerializeField]
    GameObject blackOutObject;


    List<TutorialRoute> routeList;
    Vector3[] characterOriginPosArray;

    Vector3 characterMiddleOriginPos;
    Text nowConversationText;

    CharacterName[] nowCharacterArray;
    CharacterFeeling[] nowFeelingArray;

    CharacterName nowMiddleCharacter;
    CharacterFeeling nowMiddleFeeling;

    string nowTalkingCharacterString;
    CharacterName nowTalkingCharacterEnum;

    UILanguagePack languagePack;

    float downYpos = -10;
    float upYpos = -1;
    float leftXPos = -30;
    float rightXPos = 30;

    bool nowTextFrameToggleActive;
    bool isRouteButtonAble;

    //어느 번들인지.
    //int nowBundleIndex;
    //어디에서 분기해서 어디 래퍼인지.
    protected int nowWrapperIndex;
    protected int nowConversationIndex;
    bool[] faded;
    bool blurred;
    protected int nowRouterIndex;
    bool delaying;
    bool isRouting;
    //string nextStory;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.singleton;
        saveData = gameManager.saveData;
        sceneManager = SceneManager.inst;
        languagePack = gameManager.languagePack;
        //conversationDialogBundleList = gameManager.conversationDialogBundleWrapper.conversationDialogBundleList;
        characterIndexToName = new CharacterIndexToName();
        //nowBundle = gameManager.LoadBundle("testBundle");
        //nowWrapper = nowBundle.dialogWrapperList[0];
        //routingTime = nowBundle.conversationRouter.routingTime;
        //nextStory = null;
        blurred = false;
        faded = new bool[4];
        for(int i = 0; i < 4; i++)
        {
            faded[i] = false;
            characterSprite[i].color = new Color(0.2f, 0.2f, 0.2f, 1f);
        }
        nowConversationIndex = 0;
        nowWrapperIndex = 0;
        nowRouterIndex = 0;
        nowDialogIndex = 0;
        delaying = false;
        nowTextFrameToggleActive = true;
        isRouteButtonAble = false;
        isRouting = false;
        storyParser = new StoryParser(characterIndexToName,gameManager.languagePack);
        saveData.readStoryList.Add(saveData.nextStory);
        Debug.Log(sceneManager.sceneParameter);
        nowBundle = storyParser.LoadStoryBundle(sceneManager.sceneParameter, gameManager.saveDataTimeWrapper.nowLanguageDirectory,false);
        nowDialogArray = nowBundle.storyDialogArray;
        nowConversationText = conversationText;
        blackOutText.gameObject.SetActive(false);
        


        characterOriginPosArray = new Vector3[4];
        nowFeelingArray = new CharacterFeeling[4];
        nowCharacterArray = new CharacterName[4];
        for (int i = 0; i < 4; i++)
        {
            characterOriginPosArray[i] = characterSprite[i].gameObject.transform.position;
            nowCharacterArray[i] = CharacterName.Null;
            nowFeelingArray[i] = CharacterFeeling.Null;
        }
        nowMiddleCharacter = CharacterName.Null;
        nowMiddleFeeling = CharacterFeeling.Null;
        characterMiddleOriginPos = middleCharacterSprite.gameObject.transform.position;



        TabletManager.inst.TabletOpenButtonActive(false, false);
        NextDialog();
    }

    void NextDialog()
    {
        if (nowTextFrameToggleActive == false && nowConversationText == conversationText)
        {
            TextFrameToggle(true);
            return;
        }
            bool immediateNext = false;
        if(nowDialogIndex >= nowDialogArray.Length)
        {
            sceneManager.LoadNextScene();
            return;
        }
        nowDialog = nowDialogArray[nowDialogIndex];
        Debug.Log(nowDialogIndex);
        PrintCharacter();
        PrintEffect(out immediateNext);
        
     
        PrintConversation();
        nowDialogIndex++;

        if(immediateNext == true)
        {
            NextDialog();
        }


    }

    void SetCharacterEffect(int characterIndex, string character,string effect, string feeling,bool isMiddle)
    {
        bool charDiffer = false;
        bool feelingDiffer = false;
        CharacterName enumCharacter = (CharacterName)Enum.Parse(typeof(CharacterName), character);
        CharacterFeeling enumFeeling = CharacterFeeling.Null;
        nowDialog.enumCharacterArray[characterIndex] = enumCharacter;
         GameObject obj = characterSprite[characterIndex].gameObject;

        if (enumCharacter != nowCharacterArray[characterIndex])
        {
            charDiffer = true;
        }
        if (effect != null && effect.Contains("feeling"))
        {
            //<bold> <boldEnd>
            //<b>안<\b> <b>녕<\b>
            enumFeeling = (CharacterFeeling)Enum.Parse(typeof(CharacterFeeling), feeling);
            if (enumFeeling != nowFeelingArray[characterIndex] && enumFeeling != CharacterFeeling.Null)
            {
                feelingDiffer = true;
            }
            nowFeelingArray[characterIndex] = enumFeeling;
        }
        else
        {
            if(nowFeelingArray[characterIndex] == CharacterFeeling.Null)
            {
                enumFeeling = CharacterFeeling.nothing;
            }
            else
            {
                enumFeeling = nowFeelingArray[characterIndex];
            }
        }
        if (charDiffer || feelingDiffer)
        {
            Sprite sprite = characterIndexToName.GetSprite(enumCharacter, enumFeeling);
            if (isMiddle)
            {
                middleCharacterSprite.sprite = sprite;
                middleCharacterSprite.transform.position = characterMiddleOriginPos;
            }
            else
            {
                characterSprite[characterIndex].sprite = sprite;
                characterSprite[characterIndex].transform.position = characterOriginPosArray[characterIndex];
            }
        }
        if (isMiddle)
        {

            obj = middleCharacterSprite.gameObject;
        }

        if(effect == null)
        {
            return;
        }
        if (effect.Contains("up"))
        {
            obj.transform.position = new Vector3(obj.transform.position.x, downYpos, 0);
            StartCoroutine(sceneManager.MoveModule_Linear(obj, new Vector3(obj.transform.position.x, upYpos, 0), 0.5f));
        }
        else if (effect.Contains("down"))
        {
            obj.transform.position = new Vector3(obj.transform.position.x, upYpos, 0);
            StartCoroutine(sceneManager.MoveModule_Linear(obj, new Vector3(obj.transform.position.x, downYpos, 0), 0.5f));
        }
        else if (effect.Contains("right"))
        {
            obj.transform.position = new Vector3(obj.transform.position.x, upYpos, 0);
            StartCoroutine(sceneManager.MoveModule_Linear(obj, new Vector3(rightXPos, upYpos, 0), 0.5f));
        }
        else if (effect.Contains("left"))
        {
            obj.transform.position = new Vector3(obj.transform.position.x, upYpos, 0);
            StartCoroutine(sceneManager.MoveModule_Linear(obj, new Vector3(leftXPos, downYpos, 0), 0.5f));
        }
        else if (effect.Contains("shake"))
        {
            obj.transform.position = new Vector3(obj.transform.position.x, upYpos, 0);
            //StartCoroutine(sceneManager.ShakeModule(obj, 0, float.Parse(feeling),1f));
            StartCoroutine(sceneManager.ShakeModule(obj, 0, 0.5f, 1f));
        }
    }

    void ChangeCharacterSpriteNull(int index)
    {
        nowCharacterArray[index] = CharacterName.Null;
        nowFeelingArray[index] = CharacterFeeling.Null;
        characterSprite[index].sprite = null;
    }


    void PrintCharacter()
    {
        int characterCount = 0;
        bool isMiddle = false;

        if (nowDialog.charSlot1 != null)
        {
            characterCount++;
        }
        if (nowDialog.charSlot2 != null)
        {
            characterCount++;
        }
        if (nowDialog.charSlot3 != null)
        {
            characterCount++;
        }
        if (nowDialog.charSlot4 != null)
        {
            characterCount++;
        }
        if(characterCount == 1)
        {
            isMiddle = true;
        }
        else 
        {
            nowMiddleCharacter = CharacterName.Null;
            nowMiddleFeeling = CharacterFeeling.Null;
            middleCharacterSprite.sprite = null;
        }

        if (nowDialog.charSlot1 != null)
        {
            SetCharacterEffect(0, nowDialog.charSlot1, nowDialog.charEffect1, nowDialog.charEffectParameter1, isMiddle);
        }
        else
        {
            ChangeCharacterSpriteNull(0);
        }

        if (nowDialog.charSlot2 != null)
        {
            SetCharacterEffect(1, nowDialog.charSlot2, nowDialog.charEffect2, nowDialog.charEffectParameter2, isMiddle);

        }
        else
        {
            ChangeCharacterSpriteNull(1);
        }

        if (nowDialog.charSlot3 != null)
        {
            SetCharacterEffect(2, nowDialog.charSlot3, nowDialog.charEffect3, nowDialog.charEffectParameter3, isMiddle);
        }
        else
        {
            ChangeCharacterSpriteNull(2);
        }

        if (nowDialog.charSlot4 != null)
        {
            SetCharacterEffect(3, nowDialog.charSlot4, nowDialog.charEffect4, nowDialog.charEffectParameter4, isMiddle);
        }
        else
        {
            ChangeCharacterSpriteNull(3);
        }


    }

    void PrintEffect(out bool immediateNext)
    {
        immediateNext = false;

        if (nowDialog.effect == null)
        {
            return;
        }

        if (nowDialog.dialog == null)
        {
            immediateNext = true;
        }

        if (nowDialog.effect.Contains("cutScene"))
        {
            if (nowDialog.effect.Contains("End"))
            {
                cutSceneSprite.sprite = null;
            }
            else
            {
                cutSceneSprite.sprite = characterIndexToName.GetBackGroundSprite(nowDialog.effectParameter, true);
            }
            
        }
        else if (nowDialog.effect.Contains("background"))
        {
           
            if (nowDialog.effect.Contains("End"))
            {
                backgroundSprite.sprite = null;
            }
            else
            {
                backgroundSprite.sprite = characterIndexToName.GetBackGroundSprite(nowDialog.effectParameter, false);
            }
        }
        else if (nowDialog.effect.Contains("popup"))
        {

            if (nowDialog.effect.Contains("End"))
            {
                popupSprite.sprite = null;
                popupSprite.color = new Color(1, 1, 1, 0);
            }
            else
            {
                Sprite spr = Resources.Load<Sprite>("Popup/" + nowDialog.effectParameter);
                popupSprite.color = new Color(1, 1, 1, 1);
                popupSprite.sprite = spr;
            }
        }
        else if (nowDialog.effect.Contains("route"))
        {
            isRouting = true;
            isRouteButtonAble = false;
        }
        else if (nowDialog.effect.Contains("blackOut"))
        {
            if (nowDialog.effect.Contains("End"))
            {
                blackOutObject.SetActive(false);
                nowConversationText = conversationText;
                blackOutText.gameObject.SetActive(false);
            }
            else
            {
                nowConversationText = blackOutText;
                blackOutObject.SetActive(true);
                blackOutText.gameObject.SetActive(true);
            }
    
        }
        else if (nowDialog.effect.Contains("bgm"))
        {
            Debug.LogError("브금 미구현");
        }
        else if (nowDialog.effect.Contains("sfx"))
        {
            Debug.LogError("사운드이펙트 미구현");
        }
        else if (nowDialog.effect.Contains("dialogBoxHide"))
        {
            //immediateNext = false;
            //float delay = 1;
            //if(!float.TryParse(nowDialog.effectParameter, out delay))
            //{
            //    delay = 1;
            //}

            //Debug.Log(delay+ " 딜레이");
            //StartCoroutine(DelayCoroutine(delay));

            TextFrameToggle(false,float.Parse(nowDialog.effectParameter));
            //if (nowDialog.dialog != null)
            //{

            //}
            //else
            //{
            //    StartCoroutine(DelayActionCoroutine(delay, NextDialog));
            //}

        }
        else if (nowDialog.effect.Contains("jump"))
        {
            immediateNext = false;
            nowDialogIndex += (int.Parse(nowDialog.effectParameter)-1);
            if(nowDialogIndex < nowDialogArray.Length)
            {
                nowDialog = nowDialogArray[nowDialogIndex];
            }
            
        }
        else if (nowDialog.effect.Contains("delay"))
        {
            immediateNext = false;
            StartCoroutine(DelayActionCoroutine(float.Parse(nowDialog.effectParameter), NextDialog));
        }
        else if (nowDialog.effect.Contains("fadeInAndFocus"))
        {
            for (int i = 0; i < 4; i++)
            {
                blurManager.ChangeLayer(true, characterSprite[i].gameObject);
            }
            blurManager.ChangeLayer(true, middleCharacterSprite.gameObject);
            blurManager.OnBlur(false,()=>
            {
                for (int i = 0; i < 4; i++)
                {
                    blurManager.ChangeLayer(false, characterSprite[i].gameObject);
                }
                blurManager.ChangeLayer(false, middleCharacterSprite.gameObject);
            }
            );
            StartCoroutine(sceneManager.FadeModule_Image(fadeObject, 1, 0, 1));
        }
        else if (nowDialog.effect.Contains("fade"))
        {
            float fadeTime = float.Parse(nowDialog.effectParameter);
            float initTransparency = 0;
            float endTransparency = 1;
            if (nowDialog.effect.Contains("Out") || nowDialog.effect.Contains("out"))
            {
                initTransparency = 0;
                endTransparency = 1;
            }
            if (nowDialog.effect.Contains("In") || nowDialog.effect.Contains("in"))
            {

                initTransparency = 1;
                endTransparency = 0;
            }
            StartCoroutine(sceneManager.FadeModule_Image(fadeObject, initTransparency, endTransparency, fadeTime));
        }
        else if (nowDialog.effect.Contains("shakeScreen"))
        {
            StartCoroutine(sceneManager.ShakeModule(cameraObject, 1, 1,1));
        }
        else if (nowDialog.effect.Contains("blur"))
        {
            if (nowDialog.effect.Contains("On") || nowDialog.effect.Contains("on"))
            {
                blurred = true;

            }
            if (nowDialog.effect.Contains("Off") || nowDialog.effect.Contains("off"))
            {

                blurred = false;

            }
            for (int i = 0; i < 4; i++)
            {
                blurManager.ChangeLayer(blurred, characterSprite[i].gameObject);
            }
            blurManager.ChangeLayer(blurred, middleCharacterSprite.gameObject);
            blurManager.OnBlur(blurred);
        }
    
    }


    //그 버튼이 뜨는거임. 라우팅 버튼
    void RouteCheck()
    {
        ////if (routingTime <= 0)
        ////{
        ////    conversationText.text = "이야기끝";
        ////    if (!isTile)
        ////    {
        ////        toNextSceneButton.SetActive(true);
        ////    }
            
        ////    return;
        ////}
        //if(nowRouterIndex >= nowBundle.conversationRouterList.Count)
        //{
        //    return;
        //}
        //ConversationRouter router = nowBundle.conversationRouterList[nowRouterIndex];
        //nowRouter = router;
        //if (router == null)
        //{
        //    nowWrapperIndex++;
        //    nowWrapper = nowBundle.dialogWrapperList[nowWrapperIndex];
        //    return;
        //} else if (router.routeButtonText.Count == 0)
        //{
        //    if(nowWrapperIndex+1 < nowBundle.dialogWrapperList.Count)
        //    {
        //        nowWrapperIndex++;
        //        nowWrapper = nowBundle.dialogWrapperList[nowWrapperIndex];
        //    }
        //    return;
        //}
        //checkingRouter = true;
        //nowInRouterWrapper = true;
        //for (int i = 0; i < routingButtonArray.Length; i++)
        //{
        //    routingButtonArray[i].SetActive(false);
        //}

        //for(int i = 0; i < router.routeButtonText.Count; i++)
        //{
        //    routingButtonArray[i].SetActive(true);
        //    routingTextArray[i].text = router.routeButtonText[i];
        //}
        //nowRouterIndex++;
    }


    //한 줄 띄우는거.
    void PrintConversation()
    {
        if (nowDialog.dialog == null)
        {
            return;
        }
        float speed = 0.05f;
        bool skippable = true;
        if(nowDialog.speed != null)
        {
            speed *= float.Parse(nowDialog.speed);
        }
        if (nowDialog.effect != null)
        {
            if (nowDialog.effect.Contains("unskippable"))
            {
                skippable = false;
            }
        }
        StartCoroutine(sceneManager.LoadTextOneByOne(nowDialog.dialog, nowConversationText,speed,skippable));
        CharacterName talkingCharEnum = CharacterName.Null;
        if (nowDialog.talkingCharName != null)
        {
            nameText.text = nowDialog.talkingCharName;
            talkingCharEnum = characterIndexToName.IngameNameToEnum(nowDialog.talkingCharName, languagePack);
        }
        else
        {
            nameText.text = null;
        }


        if (talkingCharEnum == CharacterName.Null)
        {
            for (int i = 0; i < 4; i++)
            {
                if (!faded[i])
                {
                    faded[i] = true;
                    //StartCoroutine(sceneManager.FadeModule_Sprite(characterSprite[i].gameObject, 0.2f, 1, 0.5f));
                    StartCoroutine(sceneManager.ColorChange_Sprite(characterSprite[i].gameObject, 1, 0.2f, 0.2f));
                    StartCoroutine(sceneManager.ChangeScale_Object(characterSprite[i].gameObject, 1f, 0.9f, 0.2f));
                }
            }
        }
        else
        {
            for (int i = 0; i < 4; i++)
            {
               if (nowDialog.enumCharacterArray[i] == talkingCharEnum)
                {
                    if(faded[i] ==true)
                    {
                        faded[i] = false;
                        //StartCoroutine(sceneManager.FadeModule_Sprite(characterSprite[i].gameObject, 1f, 0.2f, 0.5f));

                        StartCoroutine(sceneManager.ColorChange_Sprite(characterSprite[i].gameObject, 0.2f, 1, 0.2f));
                        StartCoroutine(sceneManager.ChangeScale_Object(characterSprite[i].gameObject, 0.9f, 1f, 0.2f));

                    }


                }
                else
                {
                    if(faded[i] == false)
                    {
                        faded[i] = true;
                        //StartCoroutine(sceneManager.FadeModule_Sprite(characterSprite[i].gameObject, 0.2f, 1, 0.5f));
                        StartCoroutine(sceneManager.ColorChange_Sprite(characterSprite[i].gameObject, 1, 0.2f, 0.2f));
                        StartCoroutine(sceneManager.ChangeScale_Object(characterSprite[i].gameObject, 1, 0.9f, 0.2f));
                    }
                    
                }
            }
        }
        
       

    }

    

 

    void TextFrameToggle(bool active)
    {
        TextFrameToggle(active, 1);

    }

    void TextFrameToggle(bool active, float time)
    {
        nowTextFrameToggleActive = active;
        if (active)
        {
            textFrameObject.SetActive(true);
            nextButtonObject.SetActive(true);
            conversationText.text = null;
            nameText.text = null;
            StartCoroutine(sceneManager.FadeModule_Image(textFrameObject, 0, 1, time, true));
            StartCoroutine(sceneManager.FadeModule_Image(nextButtonObject, 0, 1, time, true));
            StartCoroutine(sceneManager.FadeModule_Text(conversationText, 0, 1, time));
            StartCoroutine(sceneManager.FadeModule_Text(nameText, 0, 1, time));
            StartCoroutine(DelayActionCoroutine(1, NextDialog));

        }
        else
        {
            StartCoroutine(sceneManager.FadeModule_Image(textFrameObject, 1, 0, time, false));
            StartCoroutine(sceneManager.FadeModule_Image(nextButtonObject, 1, 0, time, false));
            StartCoroutine(sceneManager.FadeModule_Text(conversationText, 1, 0, time));
            StartCoroutine(sceneManager.FadeModule_Text(nameText, 1, 0, time));
        }

    }

    public void OnTouchScreen()
    {
        if (!sceneManager.nowTexting && !delaying)
        {
            if(isRouting)
            {
                if (!isRouteButtonAble)
                {
                    RouteButtonActive();
                }
                
            }
            else
            {

                    NextDialog();

            }
            
            
        }
    }
    

    IEnumerator DelayCoroutine(float time)
    {
        delaying = true;
        yield return new WaitForSeconds(time);
        delaying = false;
    }

    IEnumerator DelayActionCoroutine(float time, Action action)
    {
        delaying = true;
        yield return new WaitForSeconds(time);
        delaying = false;
        action();
    }

    void RouteButtonActive()
    {
        routeList = new List<TutorialRoute>();
        if (nowDialog.routeFirst != null)
        {
            TutorialRoute route = new TutorialRoute();
            route.jump = int.Parse(nowDialog.routeFirstJump);
            route.routeString = nowDialog.routeFirst;
            routeList.Add(route);
        }

        if (nowDialog.routeSecond != null)
        {
            TutorialRoute route = new TutorialRoute();
            route.jump = int.Parse(nowDialog.routeSecondJump);
            route.routeString = nowDialog.routeSecond;
            routeList.Add(route);
        }

        if (nowDialog.routeThird != null)
        {
            TutorialRoute route = new TutorialRoute();
            route.jump = int.Parse(nowDialog.routeThirdJump);
            route.routeString = nowDialog.routeThird;
            routeList.Add(route);
        }

       
        for (int i = 0; i < routeList.Count; i++)
        {
            routeButtonArray[i].SetActive(true);
        }
        isRouting = true;


        List<Text> routeTextList = new List<Text>();
        isRouteButtonAble = false;
        
        for (int i = 0; i < 4; i++)
        {
            blurManager.ChangeLayer(true, characterSprite[i].gameObject);
        }
        blurManager.ChangeLayer(true, middleCharacterSprite.gameObject);
        blurManager.OnBlur(true);
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
        StartCoroutine(sceneManager.InvokerCoroutine(1, RouteButtonAbleTrue));


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

        for (int i = 0; i < routeList.Count; i++)
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
        blurManager.OnBlur(false, () =>
        {
            for (int i = 0; i < 4; i++)
            {
                blurManager.ChangeLayer(false, characterSprite[i].gameObject);
            }
            blurManager.ChangeLayer(false, middleCharacterSprite.gameObject);
        }
        );
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
        for (int i = 0; i < routeButtonArray.Length; i++)
        {
            routeButtonArray[i].SetActive(false);
        }

        nowDialogIndex += routeList[index].jump - 1;
        isRouting = false;

        NextDialog();

    }

    void RouteButtonAbleTrue()
    {
        isRouteButtonAble = true;
    }


}
