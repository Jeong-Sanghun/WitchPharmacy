using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryManager : MonoBehaviour
{
    [SerializeField] BlurManager blurManager;
    protected SceneManager sceneManager;
    GameManager gameManager;
    SaveDataClass saveData;

    //protected List<ConversationDialogBundle> conversationDialogBundleList;
    ConversationDialogBundle nowBundle;
    ConversationDialogWrapper nowWrapper;
    ConversationRouter nowRouter;
    StoryParser storyParser;

    CharacterIndexToName characterIndexToName;

    [SerializeField]
    SpriteRenderer[] characterSprite;
    [SerializeField]
    SpriteRenderer cutSceneBGSprite;

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
    

    float downYpos = -10;
    float upYpos = 0;

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
    string nextStory;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.singleTon;
        saveData = gameManager.saveData;
        sceneManager = SceneManager.inst;
        //conversationDialogBundleList = gameManager.conversationDialogBundleWrapper.conversationDialogBundleList;
        characterIndexToName = new CharacterIndexToName();
        //nowBundle = gameManager.LoadBundle("testBundle");
        //nowWrapper = nowBundle.dialogWrapperList[0];
        //routingTime = nowBundle.conversationRouter.routingTime;
        nextStory = null;
        checkingRouter = false;
        blurred = false;
        faded = new bool[4];
        for(int i = 0; i < 4; i++)
        {
            faded[i] = false;
            characterSprite[i].color = new Color(1, 1, 1, 0.2f);
        }
        nowConversationIndex = 0;
        nowWrapperIndex = 0;
        nowRouterIndex = 0;
        storyParser = new StoryParser(characterIndexToName,gameManager.languagePack);
        saveData.readStoryList.Add(saveData.nextStory);
        nowBundle = storyParser.LoadBundle(saveData.nextStory, gameManager.saveDataTimeWrapper.nowLanguageDirectory,false);
        nowWrapper = nowBundle.dialogWrapperList[0];
        if (nowWrapper.nextStory != null && nowWrapper.nextStory.Length > 0)
        {
            saveData.nextStory = nowWrapper.nextStory;
        }
        for (int i = 0; i < 4; i++)
        {
            if (nowWrapper.characterName[i] != null)
                characterSprite[i].sprite = characterIndexToName.GetSprite(nowWrapper.characterName[i], nowWrapper.characterFeeling[i]);
            else
                characterSprite[i].sprite = null;
        }
        for(int i = 0; i < nowWrapper.startEffectList.Count; i++)
        {
            DialogEffect effect = nowWrapper.startEffectList[i];
            GameObject obj = characterSprite[(int)effect.characterPosition].gameObject;
            if(effect.effect == DialogFX.Up)
            {
                obj.transform.position = new Vector3(obj.transform.position.x, downYpos, 0);
                StartCoroutine(sceneManager.MoveModule_Linear(obj, new Vector3(obj.transform.position.x, upYpos, 0), 1));
            }
            else if (effect.effect == DialogFX.Down)
            {
                obj.transform.position = new Vector3(obj.transform.position.x, upYpos, 0);
                StartCoroutine(sceneManager.MoveModule_Linear(obj, new Vector3(obj.transform.position.x, downYpos, 0), 1));
            }
        }
        if (nowWrapper.isCutscene)
        {
            cutSceneBGSprite.sprite = characterIndexToName.GetBackGroundSprite(nowWrapper.cutSceneFileName, true);
            if(nowWrapper.cutSceneEffect == CutSceneEffect.Blur)
            {
                blurred = true;
                blurManager.OnBlur(true);
            }
        }
        else
        {
            cutSceneBGSprite.sprite = characterIndexToName.GetBackGroundSprite(nowWrapper.backGroundFileName, false);
            if(nowWrapper.backGroundEffect == CutSceneEffect.Blur)
            {
                blurred = true;
                blurManager.OnBlur(true);
            }
        }
        

        PrintConversation();
    }


    //그 버튼이 뜨는거임. 라우팅 버튼
    void RouteCheck()
    {
        //if (routingTime <= 0)
        //{
        //    conversationText.text = "이야기끝";
        //    if (!isTile)
        //    {
        //        toNextSceneButton.SetActive(true);
        //    }
            
        //    return;
        //}
        if(nowRouterIndex >= nowBundle.conversationRouterList.Count)
        {
            return;
        }
        ConversationRouter router = nowBundle.conversationRouterList[nowRouterIndex];
        nowRouter = router;
        if (router == null)
        {
            nowWrapperIndex++;
            nowWrapper = nowBundle.dialogWrapperList[nowWrapperIndex];
            return;
        } else if (router.routeButtonText.Count == 0)
        {
            if(nowWrapperIndex+1 < nowBundle.dialogWrapperList.Count)
            {
                nowWrapperIndex++;
                nowWrapper = nowBundle.dialogWrapperList[nowWrapperIndex];
            }
            return;
        }
        checkingRouter = true;
        nowInRouterWrapper = true;
        for (int i = 0; i < routingButtonArray.Length; i++)
        {
            routingButtonArray[i].SetActive(false);
        }

        for(int i = 0; i < router.routeButtonText.Count; i++)
        {
            routingButtonArray[i].SetActive(true);
            routingTextArray[i].text = router.routeButtonText[i];
        }
        nowRouterIndex++;
    }

    public void NextWrapper()
    {
        Debug.Log("넥스트래퍼");
        if (nowInRouterWrapper)
        {
            leftRouterWrapper--;
            if(leftRouterWrapper <= 0)
            {
                nowInRouterWrapper = false;
                if (nowWrapperIndex >= nowBundle.dialogWrapperList.Count)
                {
                    toNextSceneButton.SetActive(true);
                    return;
                }
                nowWrapper = nowBundle.dialogWrapperList[nowWrapperIndex];
                

            }
            else
            {
                nowRouterWrapperIndex++;
                nowWrapper = nowRouter.routingWrapperList[nowRouterWrapperIndex];

            }
        }
        else
        {
            nowWrapperIndex++;
            if (nowWrapper.nextWrapperIsRouter)
            {
                RouteCheck();
                return;
            }
            if (nowWrapperIndex >= nowBundle.dialogWrapperList.Count)
            {
                toNextSceneButton.SetActive(true);
                return;
            }

            nowWrapper = nowBundle.dialogWrapperList[nowWrapperIndex];
        }
        if (nowWrapper.nextStory != null && nowWrapper.nextStory.Length>0)
        {
            Debug.Log("저장");
            saveData.nextStory = nowWrapper.nextStory;
        }
        
        nowConversationIndex = 0;
        for(int i = 0; i < 4; i++)
        {
            if(nowWrapper.characterName[i] != null)
                characterSprite[i].sprite = characterIndexToName.GetSprite(nowWrapper.characterName[i], nowWrapper.characterFeeling[i]);
            else
            {
                characterSprite[i].sprite = null;
            }
        }
        for (int i = 0; i < nowWrapper.startEffectList.Count; i++)
        {
            DialogEffect effect = nowWrapper.startEffectList[i];
            GameObject obj = characterSprite[(int)effect.characterPosition].gameObject;
            if (effect.effect == DialogFX.Up)
            {
                obj.transform.position = new Vector3(obj.transform.position.x, downYpos, 0);
                StartCoroutine(sceneManager.MoveModule_Linear(obj, new Vector3(obj.transform.position.x, upYpos, 0), 1));
            }
            else if (effect.effect == DialogFX.Down)
            {
                obj.transform.position = new Vector3(obj.transform.position.x, upYpos, 0);
                StartCoroutine(sceneManager.MoveModule_Linear(obj, new Vector3(obj.transform.position.x, downYpos, 0), 1));

            }
        }
        if (nowWrapper.isCutscene)
        {
            cutSceneBGSprite.sprite = characterIndexToName.GetBackGroundSprite(nowWrapper.cutSceneFileName, true);
            if (nowWrapper.cutSceneEffect == CutSceneEffect.Blur && blurred == false)
            {
                blurManager.OnBlur(true);
                blurred = true;
            }
            else if (nowWrapper.cutSceneEffect == CutSceneEffect.None && blurred == true)
            {
                blurManager.OnBlur(false);
                blurred = false;
            }
        }
        else
        {
            cutSceneBGSprite.sprite = characterIndexToName.GetBackGroundSprite(nowWrapper.backGroundFileName, false);
            if (nowWrapper.backGroundEffect == CutSceneEffect.Blur && blurred == false)
            {
                blurManager.OnBlur(true);
                blurred = true;
            }
            else if(nowWrapper.backGroundEffect == CutSceneEffect.None && blurred == true)
            {
                blurManager.OnBlur(false);
                blurred = false;
            }
        }

        PrintConversation();
        


    }

    //한 줄 띄우는거.
    void PrintConversation()
    {
        //이제 끝나면 RouteCheck가 뜸. 그 wrapper에 있는거 다 쓰면은.
        if (nowConversationIndex >= nowWrapper.conversationDialogList.Count)
        {
            NextWrapper();
            return;
        }
        ConversationDialog nowConversation = nowWrapper.conversationDialogList[nowConversationIndex];
        conversationText.text = nowConversation.dialog;
        StartCoroutine( sceneManager.LoadTextOneByOne(nowConversation.dialog, conversationText));
        nameText.text = nowConversation.ingameName;
        for(int i = 0; i < 4; i++)
        {
            if(faded[i] == nowConversation.fade[i])
            {
                continue;
            }
            else if(!faded[i] && nowConversation.fade[i])
            {
                faded[i] = true;
                StartCoroutine(sceneManager.FadeModule_Sprite(characterSprite[i].gameObject, 0.2f, 1, 0.5f));

            }
            else if(faded[i] && !nowConversation.fade[i])
            {
                faded[i] = false;
                StartCoroutine(sceneManager.FadeModule_Sprite(characterSprite[i].gameObject, 1f, 0.2f, 0.5f));
                

            }
        }
        nowConversationIndex++;

    }

    //라우터 버튼 눌릴 때
    public void OnRouterButton(int index)
    {
        checkingRouter = false;
        for (int i = 0; i < routingButtonArray.Length; i++)
        {
            routingButtonArray[i].SetActive(false);
        }
        if(nowRouter.routingWrapperIndex.Count-1 == index)
        {
            leftRouterWrapper = nowRouter.routingWrapperIndex.Count - nowRouter.routingWrapperIndex[index];
        }
        else
        {
            leftRouterWrapper = nowRouter.routingWrapperIndex[index+1] - nowRouter.routingWrapperIndex[index];
        }
        RoutePair routePair = null;
        for (int i = 0; i < saveData.routePairList.Count; i++)
        {
            if (saveData.routePairList[i].storyName.Contains(nowBundle.bundleName))
            {
                
                if (routePair.pickedRouteList.Count >= nowBundle.conversationRouterList.Count)
                {
                    saveData.routePairList.RemoveAt(i);
                }
                else
                {
                    routePair = saveData.routePairList[i];
                }
                break;
            }
        }
        if(routePair == null)
        {
            routePair = new RoutePair();
            routePair.storyName = nowBundle.bundleName;
        }
        routePair.pickedRouteList.Add(index);

        nowRouterWrapperIndex = nowRouter.routingWrapperIndex[index];
        nowWrapper = nowRouter.routingWrapperList[nowRouterWrapperIndex];
        nowConversationIndex = 0;
        if (nowWrapper.nextStory != null && nowWrapper.nextStory.Length > 0)
        {
            Debug.Log("저장");
            saveData.nextStory = nowWrapper.nextStory;
        }
        PrintConversation();
        
    }

    public void ToNextSceneButton()
    {
        Debug.Log(sceneManager.lastSceneName);

        if(sceneManager.lastSceneName == null)
        {
            //gameManager.AutoSave("StoreScene");
            sceneManager.LoadScene("RoomCounterScene");
        }
        //else if(sceneManager.lastSceneName == "StoreScene")
        //{
        //    //지금 쓸지말지 모름 여기는.
        //    //gameManager.ForceSaveButtonActive("RoomCounterScene");
        //    sceneManager.LoadScene("RoomCounterScene");
        //}
        else if (sceneManager.lastSceneName == "StartScene")
        {
            sceneManager.LoadScene("RoomCounterScene");
        }
        else if (sceneManager.lastSceneName == "RoomCounterScene")
        {

         //   gameManager.ForceSaveButtonActive("ExploreScene");
            sceneManager.LoadScene("ExploreScene");
        }
        else if (sceneManager.lastSceneName == "ExploreScene" || sceneManager.lastSceneName == "ResearchScene" || sceneManager.lastSceneName == "StoreScene" || sceneManager.lastSceneName == "RegionScene")
        {
            gameManager.ForceSaveButtonActive("StoryScene",SaveTime.DayStart);
            //sceneManager.LoadScene("StoryScene");
        }
        else if(sceneManager.lastSceneName == "StoryScene")
        {
            sceneManager.LoadScene("RoomCounterScene");
        }

    }

    public void OnTouchScreen()
    {
        if (!checkingRouter && !sceneManager.nowTexting)
        {
            PrintConversation();
        }
    }


    // Update is called once per frame
    void Update()
    {
        //입력
        if (Input.GetMouseButtonDown(0))
        {

        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            nowConversationIndex = nowWrapper.conversationDialogList.Count;
        }

    }
}
