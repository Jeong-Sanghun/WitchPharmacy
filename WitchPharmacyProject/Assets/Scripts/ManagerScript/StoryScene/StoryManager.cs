using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryManager : MonoBehaviour
{
    GameManager gameManager;
    SaveDataClass saveData;
    SceneManager sceneManager;

    List<ConversationDialogBundle> conversationDialogBundleList;
    ConversationDialogBundle nowBundle;
    ConversationDialogWrapper nowWrapper;

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


    //어느 번들인지.
    int nowBundleIndex;
    //어디에서 분기해서 어디 래퍼인지.
    int nowWrapperIndex;
    int nowConversationIndex;
    bool checkingRouter;
    bool leftFaded;
    bool rightFaded;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.singleTon;
        saveData = gameManager.saveData;
        sceneManager = SceneManager.inst;
        conversationDialogBundleList = gameManager.conversationDialogBundleWrapper.conversationDialogBundleList;
        characterIndexToName = new CharacterIndexToName();
        nowBundle = conversationDialogBundleList[0];
        nowWrapper = nowBundle.dialogWrapperList[0];
        checkingRouter = false;
        nowConversationIndex = 0;
        nowWrapperIndex = 0;
        nowBundleIndex = 0;
        PrintConversation();
    }

    void RouteCheck()
    {
        if (nowBundle.conversationRouter.routingTime <= 0)
        {
            conversationText.text = "이야기끝";
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
            nowWrapperIndex++;
            nowWrapper = nowBundle.dialogWrapperList[nowWrapperIndex];
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

    void PrintConversation()
    {
        if (nowConversationIndex >= nowWrapper.conversationDialogList.Count)
        {
            RouteCheck();
            return;
        }
        ConversationDialog nowConversation = nowWrapper.conversationDialogList[nowConversationIndex];
        conversationText.text = nowConversation.dialog;
        leftSideSprite.sprite = characterIndexToName.GetSprite(nowConversation.leftCharacterIndex, nowConversation.leftCharacterFeeling);
        rightSideSprite.sprite = characterIndexToName.GetSprite(nowConversation.rightCharacterIndex, nowConversation.rightCharacterFeeling);

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
        nowBundle.conversationRouter.routingTime--;
        nowWrapperIndex = wrapperIndex;
        nowConversationIndex = 0;
        nowWrapper = nowBundle.dialogWrapperList[nowWrapperIndex];
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!checkingRouter)
            {
                PrintConversation();
            }
        }

    }
}
