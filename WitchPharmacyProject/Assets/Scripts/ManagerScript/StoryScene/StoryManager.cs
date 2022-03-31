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
    SpriteRenderer cutSceneBGSprite;
    [SerializeField]
    Image popupSprite;

    [SerializeField]
    Text conversationText;
    [SerializeField]
    Text nameText;

    [SerializeField]
    Text[] routingTextArray;
    [SerializeField]
    GameObject[] routingButtonArray;

    [SerializeField]
    GameObject toNextSceneButton;
    [SerializeField]
    GameObject fadeObject;
    [SerializeField]
    GameObject blackOutObject;

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

    //어느 번들인지.
    //int nowBundleIndex;
    //어디에서 분기해서 어디 래퍼인지.
    protected int nowWrapperIndex;
    protected int nowConversationIndex;
    protected bool checkingRouter;
    bool[] faded;
    bool blurred;
    protected int nowRouterIndex;
    bool nowInRouterWrapper;
    int leftRouterWrapper;
    int nowRouterWrapperIndex;
    bool delaying;
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
        checkingRouter = false;
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




        NextDialog();
    }

    void NextDialog()
    {
        bool immediateNext = false;
        if(nowDialogIndex >= nowDialogArray.Length)
        {
            toNextSceneButton.SetActive(true);
            return;
        }
        nowDialog = nowDialogArray[nowDialogIndex];
       
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
                Debug.Log("ㅁㄴㅇㄹ");
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
        if(nowDialog.effect == null)
        {
            return;
        }

        if(nowDialog.dialog == null)
        {
            immediateNext = true;
        }

        if (nowDialog.effect.Contains("cutScene"))
        {
            if (nowDialog.effect.Contains("End"))
            {
                cutSceneBGSprite.sprite = null;
            }
            else
            {
                cutSceneBGSprite.sprite = characterIndexToName.GetBackGroundSprite(nowDialog.effectParameter, true);
            }
            
        }
        else if (nowDialog.effect.Contains("background"))
        {
           
            if (nowDialog.effect.Contains("End"))
            {
                cutSceneBGSprite.sprite = null;
            }
            else
            {
                cutSceneBGSprite.sprite = characterIndexToName.GetBackGroundSprite(nowDialog.effectParameter, false);
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

        }
        else if (nowDialog.effect.Contains("delay"))
        {
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
            StartCoroutine(sceneManager.ShakeModule(cameraObject, 0, 1, float.Parse(nowDialog.effectParameter)));
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

    //라우터 버튼 눌릴 때
    public void OnRouterButton(int index)
    {
        //checkingRouter = false;
        //for (int i = 0; i < routingButtonArray.Length; i++)
        //{
        //    routingButtonArray[i].SetActive(false);
        //}
        //if(nowRouter.routingWrapperIndex.Count-1 == index)
        //{
        //    leftRouterWrapper = nowRouter.routingWrapperIndex.Count - nowRouter.routingWrapperIndex[index];
        //}
        //else
        //{
        //    leftRouterWrapper = nowRouter.routingWrapperIndex[index+1] - nowRouter.routingWrapperIndex[index];
        //}
        //RoutePair routePair = null;
        //for (int i = 0; i < saveData.routePairList.Count; i++)
        //{
        //    if (saveData.routePairList[i].storyName.Contains(nowBundle.bundleName))
        //    {
        //        routePair = saveData.routePairList[i];
        //        if (routePair.pickedRouteList.Count >= nowBundle.conversationRouterList.Count)
        //        {
        //            saveData.routePairList.RemoveAt(i);
        //            routePair = null;
        //        }
        //        break;
        //    }
        //}
        //if(routePair == null)
        //{
        //    routePair = new RoutePair();
        //    routePair.storyName = nowBundle.bundleName;
        //    saveData.routePairList.Add(routePair);
        //}
        //routePair.pickedRouteList.Add(index);

        //nowRouterWrapperIndex = nowRouter.routingWrapperIndex[index];
        //nowWrapper = nowRouter.routingWrapperList[nowRouterWrapperIndex];
        //nowConversationIndex = 0;
        //if (nowWrapper.nextStory != null && nowWrapper.nextStory.Length > 0)
        //{
        //    Debug.Log("저장");
        //    saveData.nextStory = nowWrapper.nextStory;
        //}
        //if (nowWrapper.nextRegion != null && nowWrapper.nextRegion.Length > 0)
        //{
        //    StoryRegion region = (StoryRegion)Enum.Parse(typeof(StoryRegion), nowWrapper.nextRegion);
        //    if (region != saveData.nowRegion)
        //    {
        //        saveData.nowRegion = region;
        //    }
        //}
        //PrintConversation();
        
    }

    public void ToNextSceneButton()
    {
        //Debug.Log(sceneManager.lastSceneName);

        //if (sceneManager.lastSceneName == null)
        //{
        //    //gameManager.AutoSave("StoreScene");
        //    sceneManager.LoadScene("RoomCounterScene");
        //}
        ////else if(sceneManager.lastSceneName == "StoreScene")
        ////{
        ////    //지금 쓸지말지 모름 여기는.
        ////    //gameManager.ForceSaveButtonActive("RoomCounterScene");
        ////    sceneManager.LoadScene("RoomCounterScene");
        ////}
        //else if (sceneManager.lastSceneName == "StartScene")
        //{
        //    sceneManager.LoadScene("RoomCounterScene");
        //}
        //else if (sceneManager.lastSceneName == "RoomCounterScene")
        //{

        //    //   gameManager.ForceSaveButtonActive("ExploreScene");
        //    sceneManager.LoadScene("ExploreScene");
        //}
        //else if (sceneManager.lastSceneName == "ExploreScene" || sceneManager.lastSceneName == "ResearchScene" || sceneManager.lastSceneName == "StoreScene" || sceneManager.lastSceneName == "RegionScene")
        //{
        //    gameManager.ForceSaveButtonActive("StoryScene", SaveTime.DayStart);
        //    //sceneManager.LoadScene("StoryScene");
        //}
        //else if (sceneManager.lastSceneName == "StoryScene")
        //{
        //    sceneManager.LoadScene("RoomCounterScene");
        //}
        toNextSceneButton.SetActive(false);

        if (sceneManager.sceneWrapper.sceneArray[saveData.nowSceneIndex].saveTimeString != null)
        {
            gameManager.ForceSaveButtonActive();
        }
        else
        {
            sceneManager.LoadNextScene();
        }


    }

    public void OnTouchScreen()
    {
        if (!checkingRouter && !sceneManager.nowTexting && !delaying)
        {
            NextDialog();
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

    
}
