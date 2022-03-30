using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public enum VisitorEndState
{
    Right,Wrong,Skip
}

public class CounterDialogManager : MonoBehaviour
{
    enum CounterState{
        Start,Visit, End, NotTalking,FirstSpecialVisitor, SecondSpecialVisitor, SpecialVisitorEnd, OddVisit,OddEnd
    }
    GameManager gameManager;
    SaveDataClass saveData;
    SceneManager sceneManager;
    //List<StartDialogClass> randomDialogClassList;
    //RandomVisitorEndDialogWrapper randomVisitorEndDialogWrapper;
    //RandomVisitorDiseaseBundle randomVisitorDiseaseBundle;
    //SpecialVisitorDialogBundle nowSpecialVisitorDialogBundle;
    //SpecialVisitorDialogWrapper nowWrapper;
    //OddVisitorDialogBundle oddVisitorDialogBundle;
    //List<OddVisitorDialog> nowOddVisitorWrapper;
    VisitorDialogBundle nowBundle;
    VisitorDialogWrapper nowWrapper;
    List<VisitorDialogWrapper> nowWrapperList;
    VisitorDialogWrapper lastWrapper;
    StoryParser storyParser;
    UILanguagePack languagePack;



    //중간어들 다이얼로그.
    //SymptomDialog symptomDialog;
    //RandomVisitorEndDialog nowEndDialog;
    int nowDialogIndex;
    int nowSymptomInsertIndex;
    int nowWrapperIndex;
    CounterState nowState;
    VisitorType nowVisitorType;
    VisitorClass nowVisitor;
    
    //List<string> visitDialog;

    CharacterIndexToName characterIndexToName;
    [SerializeField]
    CounterManager counterManager;
    [SerializeField]
    MedicineManager medicineManager;
    [SerializeField]
    RoomManager roomManager;
    [SerializeField]
    VisitorTriggerManager visitorTriggerManager;
    [SerializeField]
    Text counterText;
    [SerializeField]
    Text visitorText;
    [SerializeField]
    Text visitorName;
    [SerializeField]
    Text ruelliaText;
    [SerializeField]
    Text ruelliaName;

    [SerializeField]
    GameObject visitorParent;
    [SerializeField]
    GameObject specialVisitorPrefab;



    [SerializeField]
    GameObject backLogParent;
    [SerializeField]
    GameObject visitorTextObjectPrefab;
    [SerializeField]
    GameObject ruelliaTextObjectPrefab;
    [SerializeField]
    Text visitorTextPrefab;
    [SerializeField]
    Text ruelliaTextPrefab;
    [SerializeField]
    Transform backLogContent;
    [SerializeField]
    ScrollRect backLogScroll;
    [SerializeField]
    GameObject dialogMouseEventObject;
    [SerializeField]
    GameObject[] routingButtonArray;
    RectTransform contentRect;
    int nowBackLogIndex;
    //bool isRouted = false;
    //bool isOddVisitor = false;

    string languageDirectory;
    
    //룸매니저에서 못넘어가게 쓰임.
    public bool nowTalking;




    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.singleton;
        saveData = gameManager.saveData;
        sceneManager = SceneManager.inst;
        //randomDialogClassList = gameManager.randomDialogDataWrapper.randomDialogList;
        //randomVisitorEndDialogWrapper = gameManager.randomVisitorEndDialogWrapper;
        //randomVisitorDiseaseBundle = gameManager.randomVisitorDiseaseBundle;
        ruelliaText.transform.parent.gameObject.SetActive(true);
        visitorText.transform.parent.gameObject.SetActive(false);
        contentRect = backLogContent.GetComponent<RectTransform>();
        //nowSpecialVisitordialogBundle = gameManager.LoadVisitorBundle(gameManager.saveData.nowCounterDialogBundleName);
        languageDirectory = gameManager.saveDataTimeWrapper.nowLanguageDirectory;
        characterIndexToName = new CharacterIndexToName();
        storyParser = new StoryParser(characterIndexToName, gameManager.languagePack);
        languagePack = gameManager.languagePack;
        nowTalking = true;
        nowDialogIndex = 0;
        nowSymptomInsertIndex = 0;
        nowWrapperIndex = 0;
        nowBackLogIndex = 0;
  
        OnStart();
        //nowDialogClassIndex = Random.Range(0, randomDialogClassList.Count);
        //dialogCount = randomDialogClassList[nowDialogClassIndex].dialogList.Count;

    }

  


        //백로그 버튼 누를떄
    public void OnBackLogButton()
    {
        if (nowTalking)
        {
            dialogMouseEventObject.SetActive(!dialogMouseEventObject.activeSelf);
        }
        

        backLogParent.SetActive(!backLogParent.activeSelf);
        if (nowBackLogIndex >= 3)
        {
            backLogContent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 400 * (nowBackLogIndex -3));
        }
        
    }

    void InitializeBackLog()
    {
        int childCount = backLogContent.transform.childCount;
        for(int i = 0; i < childCount; i++)
        {
            backLogContent.GetChild(i).gameObject.SetActive(false);
        }
        nowBackLogIndex = 0;
    }

    void MakeBackLog(bool isVisitor,string logStr)
    {
        GameObject prefab;
        Text text;
        Vector2 rectPos;
        if (isVisitor)
        {
            rectPos = new Vector2(-350,(-400) * (nowBackLogIndex));
            prefab = visitorTextObjectPrefab;
            text = visitorTextPrefab;
        }
        else
        {
            rectPos = new Vector2(350, (-400) * (nowBackLogIndex));
            prefab = ruelliaTextObjectPrefab;
            text = ruelliaTextPrefab;
        }
        text.text = logStr;
        GameObject inst = Instantiate(prefab, backLogContent);
        inst.SetActive(true);
        inst.GetComponent<RectTransform>().anchoredPosition = rectPos;

        nowBackLogIndex++;
        contentRect.sizeDelta = new Vector2(0, 400 * (nowBackLogIndex));
        //backLogScroll.verticalScrollbar.value = 0;


    }

    //public void OnOddVisitorVisit(OddVisitorDialogBundle bundle, RandomVisitorClass visitor)
    //{
    //    isOddVisitor = true;
    //    oddVisitorDialogBundle = bundle;
        
    //    OnVisitorVisit(visitor);
    //}


    //public void OddVisitorDialogChange()
    //{
    //    nowOddVisitorWrapper = oddVisitorDialogBundle.startDialogList;
    //    nowState = CounterState.OddVisit;
    //    nowDialogIndex = 0;
    //    dialogCount = nowOddVisitorWrapper.Count;
    //    OddVisitUpdate();
    //}

    void OnStart()
    {
        nowState = CounterState.Start;
        nowWrapperIndex = 0;
        nowVisitorType = VisitorType.RuelliaStart;
        nowBundle = storyParser.LoadBundle(Random.Range(0, 2).ToString(), languageDirectory,VisitorType.RuelliaStart,0, saveData.nowRegion);
        nowWrapperList = nowBundle.startWrapperList;
        nowWrapper = nowBundle.startWrapperList[0];
        ruelliaName.text = characterIndexToName.EnumNameToIngame("Ruellia", languagePack);
        visitorName.text = characterIndexToName.EnumNameToIngame(nowWrapper.characterName, languagePack);
        if (nowWrapper.characterName != null)
        {
            if (lastWrapper.characterFeeling != nowWrapper.characterFeeling || lastWrapper.characterName != nowWrapper.characterName)
            {
                counterManager.SetSpecialVisitor(nowWrapper.characterName, nowWrapper.characterFeeling);
            }
        }

        if (nowWrapper.dialogFX == DialogFX.Up)
        {
            counterManager.VisitorUpFx();
        }
        else if (nowWrapper.dialogFX == DialogFX.Down)
        {
            counterManager.VisitorDownFx();
        }
        VisitUpdate();
    }

    public VisitorDialogBundle LoadSpecialOddBundle(SpecialVisitorCondition condition,VisitorType type)
    {
        //nowVisitor = visitor;
        //nowVisitorType = visitor.visitorType;
        nowBundle = storyParser.LoadBundle(condition.bundleName, languageDirectory,type);
        return nowBundle;
    }


    //카운터매니저에서 불러옴. 랜덤비지터 스폰하고 들어올 떄.
    public void OnVisitorVisit(VisitorClass visitor)
    {
        nowVisitor = visitor;
        nowTalking = true;
        if(visitor.visitorType == VisitorType.Random)
        {
            nowVisitorType = VisitorType.Random;
            nowBundle = storyParser.LoadBundle(Random.Range(0, 2).ToString(), languageDirectory, nowVisitorType, visitor.diseaseList.Count,saveData.nowRegion);
            visitorName.text = "";
        }
        else
        {
            nowVisitorType = visitor.visitorType;
        }

        nowWrapperList = nowBundle.startWrapperList;
        nowWrapper = nowBundle.startWrapperList[0];
        if(nowBundle.visitorType == VisitorType.Special)
        {
            visitorName.text = characterIndexToName.EnumNameToIngame(nowWrapper.characterName, languagePack);
        }
        else if (nowBundle.visitorType == VisitorType.Odd)
        {
            visitorName.text = nowWrapper.characterName;
        }
        
        //if (nowVisitorType == VisitorType.Special)
        //{
        //    ((SpecialVisitorClass)visitor).SetObjectImage(nowWrapper.characterName,nowWrapper.characterFeeling);
        //}
        dialogMouseEventObject.SetActive(true);
        roomManager.ToCounterButton(false);
        InitializeBackLog();

        nowSymptomInsertIndex = 0;
        nowWrapperIndex = 0;
        if (nowWrapper.characterName != null && nowBundle.visitorType == VisitorType.Special)
        {
            if (lastWrapper.characterFeeling != nowWrapper.characterFeeling || lastWrapper.characterName != nowWrapper.characterName)
            {
                counterManager.SetSpecialVisitor(nowWrapper.characterName, nowWrapper.characterFeeling);
            }
        }
        if (nowWrapper.characterFeeling.Contains("angry"))
        {
            visitor.FaceShifter(Feeling.Angry);
        }
        else if(nowWrapper.characterFeeling.Contains("nothing"))
        {
            visitor.FaceShifter(Feeling.Happy);
        }

        if (nowWrapper.dialogFX == DialogFX.Up)
        {
            counterManager.VisitorUpFx();
        }
        else if (nowWrapper.dialogFX == DialogFX.Down)
        {
            counterManager.VisitorDownFx();
        }
        if (nowWrapper.giveCoin == true)
        {
            counterManager.CoinGain(nowWrapper.coin);
        }

        nowState = CounterState.Visit;
        nowDialogIndex = 0;
        VisitUpdate();
    }

    ////트리거 매니저에서 불러옴.
    //public void OnSpecialVisitorVisit(SpecialVisitorDialogBundle bundle,bool isSecond)
    //{
    //    nowSpecialVisitorDialogBundle = bundle;
    //    dialogMouseEventObject.SetActive(true);
    //    nowTalking = true;
    //    roomManager.ToCounterButton(false);
    //    InitializeBackLog();
    //    if (isSecond)
    //    {
    //        nowState = CounterState.SecondSpecialVisitor;
    //    }
    //    else
    //    {
    //        //gameManager.saveData.progressingQuestBundleName.Add(bundle.bundleName);
    //        nowState = CounterState.FirstSpecialVisitor;
    //    }

    //    nowDialogIndex = 0;
    //    isRouted = false;
    //    if (nowSpecialVisitorDialogBundle.conversationRouter == null)
    //    {
    //        isRouted = true;
    //    }
    //    else if(nowSpecialVisitorDialogBundle.conversationRouter.routeButtonText.Count == 0)
    //    {
    //        isRouted = true;
    //    }
    //    if (isSecond)
    //    {
    //        nowWrapper = nowSpecialVisitorDialogBundle.secondDialogWrapperList[0];
    //        dialogCount = nowSpecialVisitorDialogBundle.secondDialogWrapperList[0].specialVisitorDialogList.Count;
    //    }
    //    else
    //    {
    //        nowWrapper = nowSpecialVisitorDialogBundle.firstDialogWrapper;
    //        dialogCount = nowWrapper.specialVisitorDialogList.Count;
    //    }

    //    SpecialVisitorUpdate();
    //}

    ////메디쓴을 전해줬을 때. 카운터메니저
    public void OnVisitorEnd(VisitorEndState state)
    {
        nowTalking = true;
        dialogMouseEventObject.SetActive(true);
        counterManager.VisitorTalkStart();
        switch (state)
        {
            case VisitorEndState.Right:
                nowWrapperList = nowBundle.rightWrapperList;
                break;
            case VisitorEndState.Wrong:
                nowWrapperList = nowBundle.wrongWrapperList;
                break;
            case VisitorEndState.Skip:
                nowWrapperList = nowBundle.skipWrapperList;
                break;
        }
        nowWrapper = nowWrapperList[0];
        nowWrapperIndex = 0;
        nowDialogIndex = 0;
        nowState = CounterState.End;
        if (nowVisitorType == VisitorType.Special)
        {
            ((SpecialVisitorClass)nowVisitor).SetObjectImage(nowWrapper.characterName,nowWrapper.characterFeeling);
        }


        if (nowWrapper.characterName != null && nowBundle.visitorType == VisitorType.Special)
        {

            if (lastWrapper.characterFeeling != nowWrapper.characterFeeling || lastWrapper.characterName != nowWrapper.characterName)
            {
                counterManager.SetSpecialVisitor(nowWrapper.characterName, nowWrapper.characterFeeling);
            }
        }
        if (nowWrapper.giveCoin == true)
        {
            counterManager.CoinGain(nowWrapper.coin);
        }
        Debug.Log(nowWrapper.characterFeeling);
        if (nowWrapper.characterFeeling == null)
        {
            nowVisitor.FaceShifter(Feeling.Happy);

        }
        else
        {
            if (nowWrapper.characterFeeling.Contains("angry"))
            {
                nowVisitor.FaceShifter(Feeling.Angry);
            }
            else if (nowWrapper.characterFeeling.Contains("nothing"))
            {
                nowVisitor.FaceShifter(Feeling.Happy);
            }
        }



        if (nowWrapper.dialogFX == DialogFX.Up)
        {
            counterManager.VisitorUpFx();
        }
        else if (nowWrapper.dialogFX == DialogFX.Down)
        {

            counterManager.VisitorDownFx();
        }

        VisitUpdate();
    }

    //public void OnOddVisitorEnd(bool wrongMedicine)
    //{
    //    nowTalking = true;
    //    dialogMouseEventObject.SetActive(true);
    //    counterManager.VisitorTalkStart();
    //    if (wrongMedicine)
    //    {
    //        nowOddVisitorWrapper = oddVisitorDialogBundle.wrongDialogList;
    //    }
    //    else
    //    {
    //        nowOddVisitorWrapper = oddVisitorDialogBundle.answerDialogList;
    //    }
    //    nowDialogIndex = 0;
    //    dialogCount = nowOddVisitorWrapper.Count;
    //    nowState = CounterState.OddEnd;
    //    OddEndUpdate();
    //}


    //public void OnSpecialVisitorEnd(bool wrongMedicine)
    //{
    //    nowTalking = true;
    //    dialogMouseEventObject.SetActive(true);
    //    counterManager.VisitorTalkStart();
    //    if (wrongMedicine)
    //    {
    //        nowWrapper = nowSpecialVisitorDialogBundle.wrongDialogWrapper;
    //        //if (gameManager.saveData.progressingQuestBundleName.Contains(nowSpecialVisitorDialogBundle.bundleName))
    //        //{
    //        //    gameManager.saveData.progressingQuestBundleName.Add(nowSpecialVisitorDialogBundle.bundleName);
    //        //}
    //    }
    //    else
    //    {
    //        //if (gameManager.saveData.progressingQuestBundleName.Contains(nowSpecialVisitorDialogBundle.bundleName))
    //        //{
    //        //    gameManager.saveData.progressingQuestBundleName.Remove(nowSpecialVisitorDialogBundle.bundleName);
    //        //}
    //        //gameManager.saveData.solvedQuestBundleName.Add(nowSpecialVisitorDialogBundle.bundleName);
    //        gameManager.saveData.chaosMeter -= nowSpecialVisitorDialogBundle.progressingNumber;
    //        nowWrapper = nowSpecialVisitorDialogBundle.answerDialogWrapper;
    //    }
    //    nowDialogIndex = 0;
    //    dialogCount = nowWrapper.specialVisitorDialogList.Count;
    //    nowState = CounterState.SpecialVisitorEnd;
    //    SpecialVisitorEndUpdate();
    //}



    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(nowWrapper != null && nowTalking)
            DebugSkip();
        }
    }

    public void OnDialogMousButtonDown()
    {
        if (!sceneManager.nowTexting)
        {
            VisitUpdate();
            //switch (nowState)
            //{
            //    case CounterState.Start:
            //        StartUpdate();
            //        break;
            //    case CounterState.End:
            //        EndUpdate();
            //        break;
            //    case CounterState.OddVisit:
            //        OddVisitUpdate();
            //        break;
            //    case CounterState.Visit:
            //        VisitUpdate();
            //        break;
            //    case CounterState.FirstSpecialVisitor:
            //    case CounterState.SecondSpecialVisitor:
            //        SpecialVisitorUpdate();
            //        break;
            //    case CounterState.SpecialVisitorEnd:
            //        SpecialVisitorEndUpdate();
            //        break;
            //    case CounterState.OddEnd:
            //        OddEndUpdate();
            //        break;
            //    default:
            //        break;
            //}
        }
    }

    //public void OnRouteButton(int index)
    //{
    //    ConversationRouter router = nowSpecialVisitorDialogBundle.conversationRouter;
    //    dialogMouseEventObject.SetActive(true);
    //    for(int i = 0; i < 3; i++)
    //    {
    //        routingButtonArray[i].SetActive(false);
    //    }
    //    //나중에 바꿔야할 부분
    //    //for(int i = 0; i < nowSpecialVisitorDialogBundle.secondDialogWrapperList.Count; i++)
    //    //{
    //    //    if(router.routingWrapperName[index] == nowSpecialVisitorDialogBundle.secondDialogWrapperList[i].wrapperName)
    //    //    {
    //    //        nowWrapper = nowSpecialVisitorDialogBundle.secondDialogWrapperList[i];
    //    //        break;
    //    //    }
    //    //}
    //    nowDialogIndex = 0;
    //    dialogCount = nowWrapper.specialVisitorDialogList.Count;
    //    isRouted = true;

    //    SpecialVisitorUpdate();
    //}
    
    //void SpecialVisitorUpdate()
    //{
    //    if (nowDialogIndex < dialogCount)
    //    {
    //        if (!nowWrapper.specialVisitorDialogList[nowDialogIndex].ruelliaTalking)
    //        {
    //            if (!visitorText.transform.parent.gameObject.activeSelf)
    //                visitorText.transform.parent.gameObject.SetActive(true);
    //            counterManager.ChangeSpecialVisitorSprite(characterIndexToName.GetSprite(nowSpecialVisitorDialogBundle.characterName, nowWrapper.specialVisitorDialogList[nowDialogIndex].characterFeeling));
    //            string str = nowWrapper.specialVisitorDialogList[nowDialogIndex].dialog;
    //            MakeBackLog(true, str);
    //            StartCoroutine(sceneManager.LoadTextOneByOne(str, visitorText));
    //            nowDialogIndex++;
    //        }
    //        else
    //        {
    //            if (!ruelliaText.transform.parent.gameObject.activeSelf)
    //                ruelliaText.transform.parent.gameObject.SetActive(true);
    //            string str = nowWrapper.specialVisitorDialogList[nowDialogIndex].dialog;
    //            MakeBackLog(false, str);
    //            StartCoroutine(sceneManager.LoadTextOneByOne(str, ruelliaText));
    //            nowDialogIndex++;

    //        }
    //    }
    //    else
    //    {
    //        //if (isRouted || nowState == CounterState.FirstSpecialVisitor)
    //        //{
    //            nowTalking = false;
    //            counterManager.VisitorTalkEnd();
    //            ruelliaText.transform.parent.gameObject.SetActive(false);
    //            visitorText.transform.parent.gameObject.SetActive(false);
    //            ruelliaText.text = "";
    //            visitorText.text = "";
    //            nowState = CounterState.NotTalking;
    //            isRouted = false;
    //        dialogMouseEventObject.SetActive(false);
    //        //}
    //        //else
    //        //{
    //        //    isRouted = true;
    //        //    dialogMouseEventObject.SetActive(false);
    //        //    nowDialogIndex = 0;
    //        //    ConversationRouter router = nowSpecialVisitorDialogBundle.conversationRouter;
    //        //    for(int i = 0; i < router.routeButtonText.Count; i++)
    //        //    {
    //        //        routingButtonArray[i].SetActive(true);
    //        //        routingButtonArray[i].GetComponentInChildren<Text>().text = router.routeButtonText[i];
    //        //    }
    //        //}

    //    }
    //}

    //void OddVisitUpdate()
    //{
    //    if (nowDialogIndex < dialogCount)
    //    {
    //        if (!nowOddVisitorWrapper[nowDialogIndex].ruelliaTalking)
    //        {
    //            if (!visitorText.transform.parent.gameObject.activeSelf)
    //                visitorText.transform.parent.gameObject.SetActive(true);
    //            string str = nowOddVisitorWrapper[nowDialogIndex].dialog;
    //            MakeBackLog(true, str);
    //            StartCoroutine(sceneManager.LoadTextOneByOne(str, visitorText));
    //            nowDialogIndex++;
    //        }
    //        else
    //        {
    //            if (!ruelliaText.transform.parent.gameObject.activeSelf)
    //                ruelliaText.transform.parent.gameObject.SetActive(true);
    //            string str = nowOddVisitorWrapper[nowDialogIndex].dialog;
    //            MakeBackLog(false, str);
    //            StartCoroutine(sceneManager.LoadTextOneByOne(str, ruelliaText));
    //            nowDialogIndex++;

    //        }
    //    }
    //    else
    //    {
    //        nowTalking = false;
    //        counterManager.VisitorTalkEnd();
    //        ruelliaText.transform.parent.gameObject.SetActive(false);
    //        visitorText.transform.parent.gameObject.SetActive(false);
    //        ruelliaText.text = "";
    //        visitorText.text = "";
    //        nowState = CounterState.NotTalking;
    //        dialogMouseEventObject.SetActive(false);
    //        isOddVisitor = false;

    //    }
    //}

    void NextWrapper()
    {
        nowDialogIndex = 0;
        if(nowWrapperIndex < nowWrapperList.Count-1)
        {
            nowWrapperIndex++;
            lastWrapper = nowWrapper;
            nowWrapper = nowWrapperList[nowWrapperIndex];
            visitorName.text = characterIndexToName.EnumNameToIngame(nowWrapper.characterName, languagePack);
            if (lastWrapper != null )
            {
                Debug.Log(nowWrapper.characterName);
                Debug.Log(nowBundle.visitorType);
                if(nowWrapper.characterName != null && (nowBundle.visitorType == VisitorType.Special
                   || nowBundle.visitorType == VisitorType.RuelliaStart))
                {
                    Debug.Log(nowWrapper.characterName);
                    counterManager.SetSpecialVisitor(nowWrapper.characterName, nowWrapper.characterFeeling);
                }

                //if (lastWrapper.characterFeeling != nowWrapper.characterFeeling || lastWrapper.characterName != nowWrapper.characterName)
                //{
                //    Debug.Log(nowWrapper.characterName);
                //    counterManager.SetSpecialVisitor(nowWrapper.characterName, nowWrapper.characterFeeling);
                //}
            }
            else if(nowBundle.visitorType == VisitorType.RuelliaStart && nowWrapper.characterName != null)
            {
                Debug.Log(nowWrapper.characterName);
                counterManager.SetSpecialVisitor(nowWrapper.characterName, nowWrapper.characterFeeling);
                nowVisitor = counterManager.GetVisitor();
            }

            Debug.Log(nowWrapper.characterFeeling);
            if(nowVisitor  != null)
            {
                if (nowBundle.visitorType != VisitorType.RuelliaStart &&
               nowBundle.visitorType != VisitorType.Special)
                {
                    if (nowWrapper.characterFeeling.Contains("angry"))
                    {
                        nowVisitor.FaceShifter(Feeling.Angry);
                    }
                    else if (nowWrapper.characterFeeling.Contains("nothing"))
                    {
                        nowVisitor.FaceShifter(Feeling.Happy);
                    }
                }
                else
                {
                    ((SpecialVisitorClass)nowVisitor).SetObjectImage(nowWrapper.characterName, nowWrapper.characterFeeling);
                }
            }
           
            



            if (nowWrapper.dialogFX == DialogFX.Up)
            {
                counterManager.VisitorUpFx();
            }
            else if (nowWrapper.dialogFX == DialogFX.Down)
            {
                visitorText.transform.parent.gameObject.SetActive(false);
                visitorText.text = "";
                counterManager.VisitorDownFx();
            }
            if (nowWrapper.giveCoin == true)
            { 
                
                counterManager.CoinGain(nowWrapper.coin);
            }


            VisitUpdate();
        }
        else
        {
            if(nowWrapper.forceEnd == true)
            {
                counterManager.VisitorDisappear(false);
            }
            else
            {
                switch (nowState)
                {
                    case CounterState.Start:
                        counterManager.VisitorDownFx();
                        roomManager.FadeShopOpen();
                        break;
                    case CounterState.Visit:
                        counterManager.VisitorTalkEnd();
                        nowState = CounterState.NotTalking;
                        break;
                    case CounterState.End:
                        counterManager.VisitorDisappear(false);
                        break;

                }
            }

            nowTalking = false;

            ruelliaText.transform.parent.gameObject.SetActive(false);
            visitorText.transform.parent.gameObject.SetActive(false);
            ruelliaText.text = "";
            visitorText.text = "";
            dialogMouseEventObject.SetActive(false);
            
        }
    }

    void DebugSkip()
    {
        nowDialogIndex = 0;
        switch (nowState)
        {
            case CounterState.Start:
                counterManager.VisitorDownFx();
                roomManager.FadeShopOpen();
                break;
            case CounterState.Visit:
                counterManager.VisitorTalkEnd();
                nowState = CounterState.NotTalking;
                break;
            case CounterState.End:
                counterManager.VisitorDisappear(false);
                break;

        }
        nowWrapper = null;
        nowTalking = false;

        ruelliaText.transform.parent.gameObject.SetActive(false);
        visitorText.transform.parent.gameObject.SetActive(false);
        ruelliaText.text = "";
        visitorText.text = "";
        dialogMouseEventObject.SetActive(false);
    }

    //각 업데이트가 달려있다.
    void VisitUpdate()
    {
        
        if (nowDialogIndex < nowWrapper.dialogList.Count)
        {
            VisitorDialog nowDialog = nowWrapper.dialogList[nowDialogIndex];
            string str = nowDialog.dialog;
            while (str.Contains("$") && nowVisitor.diseaseList.Count != 0)
            {
                if (nowSymptomInsertIndex < nowVisitor.diseaseList.Count)
                {
                    str = gameManager.languagePack.Insert(str, nowVisitor.diseaseList[nowSymptomInsertIndex].dialog);
                    nowSymptomInsertIndex++;

                }
            }


            if (nowWrapper.dialogList[nowDialogIndex].ruelliaTalking)
            {
                if (!ruelliaText.transform.parent.gameObject.activeSelf)
                    ruelliaText.transform.parent.gameObject.SetActive(true);
                MakeBackLog(false, str);
                StartCoroutine(sceneManager.LoadTextOneByOne(str, ruelliaText));
            }
            else
            {
                if (!visitorText.transform.parent.gameObject.activeSelf)
                    visitorText.transform.parent.gameObject.SetActive(true);
                MakeBackLog(true, str);
                StartCoroutine(sceneManager.LoadTextOneByOne(str, visitorText));
            }

            nowDialogIndex++;
        }
        else
        {
            NextWrapper();

        }
    }

    void StartUpdate()
    {
        if (nowDialogIndex < nowWrapper.dialogList.Count)
        {
            string str = nowWrapper.dialogList[nowDialogIndex].dialog;
            MakeBackLog(false, str);
            StartCoroutine(sceneManager.LoadTextOneByOne(str, counterText));
            nowDialogIndex++;
        }
        else
        {
            NextWrapper();

        }
    }


    //void EndUpdate()
    //{
    //    if (nowDialogIndex < dialogCount)
    //    {
    //        if (visitorRuelliaToggle)
    //        {
    //            if (!visitorText.transform.parent.gameObject.activeSelf)
    //                visitorText.transform.parent.gameObject.SetActive(true);
    //            string str = nowEndDialog.visitorDialog[nowDialogIndex].str;
    //            MakeBackLog(true, str);
    //            StartCoroutine(sceneManager.LoadTextOneByOne(str, visitorText));
    //            visitorRuelliaToggle = !visitorRuelliaToggle;
    //        }
    //        else
    //        {
    //            if (!ruelliaText.transform.parent.gameObject.activeSelf)
    //                ruelliaText.transform.parent.gameObject.SetActive(true);
    //            string str = nowEndDialog.ruelliaDialog[nowDialogIndex].str;
    //            MakeBackLog(false, str);
    //            StartCoroutine(sceneManager.LoadTextOneByOne(str, ruelliaText));
    //            visitorRuelliaToggle = !visitorRuelliaToggle;
    //            nowDialogIndex++;
    //        }
    //    }
    //    else
    //    {
    //        nowTalking = false;
    //        ruelliaText.text = "";
    //        visitorText.text = "";
    //        ruelliaText.transform.parent.gameObject.SetActive(false);
    //        visitorText.transform.parent.gameObject.SetActive(false);
    //        dialogMouseEventObject.SetActive(false);
    //        nowState = CounterState.NotTalking;
    //        counterManager.VisitorDisappear(false);
    //        visitorRuelliaToggle = true;
    //    }
    //}

    //void SpecialVisitorEndUpdate()
    //{
    //    if (nowDialogIndex < dialogCount)
    //    {
    //        if (!nowWrapper.specialVisitorDialogList[nowDialogIndex].ruelliaTalking)
    //        {
    //            if (!visitorText.transform.parent.gameObject.activeSelf)
    //                visitorText.transform.parent.gameObject.SetActive(true);
    //            counterManager.ChangeSpecialVisitorSprite(characterIndexToName.GetSprite(nowSpecialVisitorDialogBundle.characterName, nowWrapper.specialVisitorDialogList[nowDialogIndex].characterFeeling));
    //            string str = nowWrapper.specialVisitorDialogList[nowDialogIndex].dialog;
    //            MakeBackLog(true, str);
    //            StartCoroutine(sceneManager.LoadTextOneByOne(str, visitorText));
    //            nowDialogIndex++;
    //        }
    //        else
    //        {
    //            if (!ruelliaText.transform.parent.gameObject.activeSelf)
    //                ruelliaText.transform.parent.gameObject.SetActive(true);
    //            string str = nowWrapper.specialVisitorDialogList[nowDialogIndex].dialog;
    //            MakeBackLog(false, str);
    //            StartCoroutine(sceneManager.LoadTextOneByOne(str, ruelliaText));
    //            nowDialogIndex++;

    //        }
    //    }
    //    else
    //    {
    //        nowTalking = false;
    //        counterManager.VisitorDisappear(false);
    //        ruelliaText.transform.parent.gameObject.SetActive(false);
    //        visitorText.transform.parent.gameObject.SetActive(false);
    //        ruelliaText.text = "";
    //        visitorText.text = "";
    //        dialogMouseEventObject.SetActive(false);
    //        nowState = CounterState.NotTalking;
    //        isRouted = false;
    //    }
    //}

    //void OddEndUpdate()
    //{
    //    if (nowDialogIndex < dialogCount)
    //    {
    //        if (!nowOddVisitorWrapper[nowDialogIndex].ruelliaTalking)
    //        {
    //            if (!visitorText.transform.parent.gameObject.activeSelf)
    //                visitorText.transform.parent.gameObject.SetActive(true);

    //            string str = nowOddVisitorWrapper[nowDialogIndex].dialog;
    //            MakeBackLog(true, str);
    //            StartCoroutine(sceneManager.LoadTextOneByOne(str, visitorText));
    //            nowDialogIndex++;
    //        }
    //        else
    //        {
    //            if (!ruelliaText.transform.parent.gameObject.activeSelf)
    //                ruelliaText.transform.parent.gameObject.SetActive(true);
    //            string str = nowOddVisitorWrapper[nowDialogIndex].dialog;
    //            MakeBackLog(false, str);
    //            StartCoroutine(sceneManager.LoadTextOneByOne(str, ruelliaText));
    //            nowDialogIndex++;

    //        }
    //    }
    //    else
    //    {
    //        nowTalking = false;
    //        counterManager.VisitorDisappear(false);
    //        ruelliaText.transform.parent.gameObject.SetActive(false);
    //        visitorText.transform.parent.gameObject.SetActive(false);
    //        ruelliaText.text = "";
    //        visitorText.text = "";
    //        dialogMouseEventObject.SetActive(false);
    //        nowState = CounterState.NotTalking;
    //        isRouted = false;
    //    }
    //}
}
