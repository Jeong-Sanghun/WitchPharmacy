using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryManager : TileManager
{

    protected SceneManager sceneManager;

    //protected List<ConversationDialogBundle> conversationDialogBundleList;
    protected ConversationDialogBundle nowBundle;
    protected ConversationDialogWrapper nowWrapper;

    CharacterIndexToName characterIndexToName;

    [SerializeField]
    SpriteRenderer leftSideSprite;

    [SerializeField]
    SpriteRenderer rightSideSprite;

    [SerializeField]
    Text conversationText;

    [SerializeField]
    Text[] routingTextArray;
    [SerializeField]
    GameObject[] routingButtonArray;

    [SerializeField]
    GameObject toNextSceneButton;

    //어느 번들인지.
    //int nowBundleIndex;
    //어디에서 분기해서 어디 래퍼인지.
    protected int nowWrapperIndex;
    protected int nowConversationIndex;
    protected bool checkingRouter;
    bool leftFaded;
    bool rightFaded;
    protected int routingTime;

    bool isTile;

    // Start is called before the first frame update
    protected virtual new void Start()
    {
        exploreManager = ExploreManager.inst;
        gameManager = GameManager.singleTon;
        saveData = gameManager.saveData;
        sceneManager = SceneManager.inst;
        //conversationDialogBundleList = gameManager.conversationDialogBundleWrapper.conversationDialogBundleList;
        characterIndexToName = new CharacterIndexToName();
        nowBundle = gameManager.LoadBundle("testBundle");
        nowWrapper = nowBundle.dialogWrapperList[0];
        routingTime = nowBundle.conversationRouter.routingTime;
        checkingRouter = false;
        nowConversationIndex = 0;
        nowWrapperIndex = 0;
        //nowBundleIndex = 0;
        PrintConversation();
        isTile = false;
    }

    public override void TileOpen(TileButtonClass tile)
    {
        base.TileOpen(tile);
        isTile = true;
    }

    //그 버튼이 뜨는거임. 라우팅 버튼
    void RouteCheck()
    {
        if (routingTime <= 0)
        {
            conversationText.text = "이야기끝";
            if (!isTile)
            {
                toNextSceneButton.SetActive(true);
            }
            
            return;
        }

        ConversationRouter router = nowBundle.conversationRouter;
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
        for (int i = 0; i < routingButtonArray.Length; i++)
        {
            routingButtonArray[i].SetActive(false);
        }

        for(int i = 0; i < router.routeButtonText.Count; i++)
        {
            routingButtonArray[i].SetActive(true);
            routingTextArray[i].text = router.routeButtonText[i];
        }
    }


    //한 줄 띄우는거.
    protected void PrintConversation()
    {
        //이제 끝나면 RouteCheck가 뜸. 그 wrapper에 있는거 다 쓰면은.
        if (nowConversationIndex >= nowWrapper.conversationDialogList.Count)
        {
            RouteCheck();
            return;
        }
        ConversationDialog nowConversation = nowWrapper.conversationDialogList[nowConversationIndex];
        conversationText.text = nowConversation.dialog;
        StartCoroutine( sceneManager.LoadTextOneByOne(nowConversation.dialog, conversationText));
        
        if(nowConversationIndex == 0)
        {
            Debug.Log(nowConversation.leftCharacterFeeling);
            Debug.Log(nowConversation.leftCharacterName);
            leftSideSprite.sprite = characterIndexToName.GetSprite(nowConversation.leftCharacterName, nowConversation.leftCharacterFeeling);
            rightSideSprite.sprite = characterIndexToName.GetSprite(nowConversation.rightCharacterName, nowConversation.rightCharacterFeeling);

        }
        else
        {
            if (nowWrapper.conversationDialogList[nowConversationIndex - 1].leftCharacterName
    != nowConversation.leftCharacterName)
            {
                leftSideSprite.sprite = characterIndexToName.GetSprite(nowConversation.leftCharacterName, nowConversation.leftCharacterFeeling);

            }

            if (nowWrapper.conversationDialogList[nowConversationIndex - 1].rightCharacterName
!= nowConversation.rightCharacterName)
            {
                rightSideSprite.sprite = characterIndexToName.GetSprite(nowConversation.rightCharacterName, nowConversation.rightCharacterFeeling);

            }
        }
       

        if (leftFaded && !nowConversation.leftFade)
        {
            leftFaded = false;
            StartCoroutine(sceneManager.FadeModule_Sprite(leftSideSprite.gameObject,0.6f, 1, 0.5f));
        }
        else if(!leftFaded && nowConversation.leftFade)
        {
            leftFaded = true;
            StartCoroutine(sceneManager.FadeModule_Sprite(leftSideSprite.gameObject, 1, 0.6f, 0.5f));
        }
        if (rightFaded && !nowConversation.rightFade)
        {
            rightFaded = false;
            StartCoroutine(sceneManager.FadeModule_Sprite(rightSideSprite.gameObject, 0.6f, 1, 0.5f));
        }
        else if (!rightFaded && nowConversation.rightFade)
        {
            rightFaded = true;
            StartCoroutine(sceneManager.FadeModule_Sprite(rightSideSprite.gameObject, 1, 0.6f, 0.5f));
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

        string name = nowBundle.conversationRouter.routingWrapperName[index];
        int wrapperIndex = 0;
        for(int i = 0; i < nowBundle.dialogWrapperList.Count; i++)
        {
            if(name == nowBundle.dialogWrapperList[i].dialogWrapperName)
            {
                wrapperIndex = i;
                break;
            }
        }
        routingTime--;
        nowWrapperIndex = wrapperIndex;
        nowConversationIndex = 0;
        nowWrapper = nowBundle.dialogWrapperList[nowWrapperIndex];
        PrintConversation();
        
    }

    public void ToNextSceneButton()
    {
        Debug.Log(sceneManager.lastSceneName);

        if(sceneManager.lastSceneName == null)
        {
            //gameManager.AutoSave("StoreScene");
            sceneManager.LoadScene("StoreScene");
        }
        else if(sceneManager.lastSceneName == "StoreScene")
        {
            //지금 쓸지말지 모름 여기는.
            //gameManager.ForceSaveButtonActive("RoomCounterScene");
            sceneManager.LoadScene("RoomCounterScene");
        }
        else if (sceneManager.lastSceneName == "StartScene")
        {
            sceneManager.LoadScene("StoreScene");
        }
        else if (sceneManager.lastSceneName == "RoomCounterScene")
        {

         //   gameManager.ForceSaveButtonActive("ExploreScene");
            sceneManager.LoadScene("ExploreScene");
        }
        else if (sceneManager.lastSceneName == "ExploreScene")
        {
            gameManager.ForceSaveButtonActive("StoryScene",SaveTime.DayStart);
            //sceneManager.LoadScene("StoryScene");
        }
        else if(sceneManager.lastSceneName == "StoryScene")
        {
            sceneManager.LoadScene("StoreScene");
        }

    }




    // Update is called once per frame
    void Update()
    {
        //입력
        if (Input.GetMouseButtonDown(0))
        {
            if (!checkingRouter && !sceneManager.nowTexting)
            {
                PrintConversation();
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            nowConversationIndex = nowWrapper.conversationDialogList.Count;
        }

    }
}
