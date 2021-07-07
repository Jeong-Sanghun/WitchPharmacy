using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class CounterDialogManager : MonoBehaviour
{
    enum CounterState{
        Start,Visit, End, NotTalking
    }
    GameManager gameManager;
    SceneManager sceneManager;
    List<StartDialogClass> randomDialogClassList;
    RandomVisitorEndDialogWrapper randomVisitorEndDialogWrapper;
    RandomVisitorDiseaseDialogWrapper randomVisitorDiseaseDialogWrapper;
    


    //중간어들 다이얼로그.
    SymptomDialog symptomDialog;
    RandomVisitorEndDialog nowEndDialog;
    int nowDialogIndex;
    int nowDialogClassIndex;
    int dialogCount;
    bool visitorRuelliaToggle;
    CounterState nowState;
    List<string> visitDialog;
    
    [SerializeField]
    CounterManager counterManager;
    [SerializeField]
    RoomManager roomManager;
    [SerializeField]
    Text counterText;
    [SerializeField]
    Text visitorText;
    [SerializeField]
    Text ruelliaText;

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
    RectTransform contentRect;
    int nowBackLogIndex;

    //룸매니저에서 못넘어가게 쓰임.
    public bool nowTalking;




    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.singleTon;
        sceneManager = SceneManager.inst;
        randomDialogClassList = gameManager.randomDialogDataWrapper.randomDialogList;
        randomVisitorEndDialogWrapper = gameManager.randomVisitorEndDialogWrapper;
        randomVisitorDiseaseDialogWrapper = gameManager.randomVisitorDiseaseDialogWrapper;
        symptomDialog = gameManager.symptomDialog;
        visitorRuelliaToggle = true;
        ruelliaText.transform.parent.gameObject.SetActive(true);
        visitorText.transform.parent.gameObject.SetActive(false);
        contentRect = backLogContent.GetComponent<RectTransform>();



        nowTalking = true;
        nowDialogIndex = 0;
        nowBackLogIndex = 0;
        nowDialogClassIndex = Random.Range(0, randomDialogClassList.Count);
        dialogCount = randomDialogClassList[nowDialogClassIndex].dialogList.Count;
        StartUpdate();
    }

    public void OnBackLogButton()
    {
        dialogMouseEventObject.SetActive(!dialogMouseEventObject.activeSelf);

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

    //카운터매니저에서 불러옴. 랜덤비지터 스폰하고 들어올 떄.
    public void OnVisitorVisit(RandomVisitorClass visitor)
    {
        nowTalking = true;
        roomManager.ToCounterButton(false);
        InitializeBackLog();
        visitDialog = new List<string>();
        StringBuilder builder = new StringBuilder(symptomDialog.startDialog[Random.Range(0, symptomDialog.startDialog.Length)]);
        for (int i = 0; i < visitor.symptomAmountArray.Length; i++)
        {
            if (visitor.symptomAmountArray[i] == 0)
            {
                continue;
            }
            int symptomIndex;
            if (visitor.symptomAmountArray[i] < 0)
            {
                symptomIndex = visitor.symptomAmountArray[i] + 2;
            }
            else
            {
                symptomIndex = visitor.symptomAmountArray[i] + 1;
            }
            builder.Append(randomVisitorDiseaseDialogWrapper.diseaseDialogBundleArray[i].diseaseDialogArray[symptomIndex].dialogArray[Random.Range(0, 3)].str);
            visitDialog.Add(builder.ToString());
            builder = new StringBuilder();
            builder.Append(symptomDialog.middleDialog[Random.Range(0, 6)]);
        }

        nowState = CounterState.Visit;
        nowDialogIndex = 0;
        dialogCount = visitDialog.Count;
        VisitUpdate();
    }

    //메디쓴을 전해줬을 때. 카운터메니저
    public void OnVisitorEnd(bool wrongMedicine)
    {
        nowTalking = true;
        counterManager.VisitorTalkStart();
        if (wrongMedicine)
        {
            int rand = Random.Range(0, randomVisitorEndDialogWrapper.wrongDialogList.Count);
            nowEndDialog = randomVisitorEndDialogWrapper.wrongDialogList[rand];
        }
        else
        {
            int rand = Random.Range(0, randomVisitorEndDialogWrapper.rightDialogList.Count);
            nowEndDialog = randomVisitorEndDialogWrapper.rightDialogList[rand];
        }
        nowDialogIndex = 0;
        dialogCount = nowEndDialog.ruelliaDialog.Length;
        nowState = CounterState.End;
        EndUpdate();
    }



    // Update is called once per frame
    void Update()
    {

    }

    public void OnDialogMousButtonDown()
    {
        if (!sceneManager.nowTexting)
        {
            switch (nowState)
            {
                case CounterState.Start:
                    StartUpdate();
                    break;
                case CounterState.End:
                    EndUpdate();
                    break;
                case CounterState.Visit:
                    VisitUpdate();
                    break;
                default:
                    break;
            }
        }
    }
    
    //각 업데이트가 달려있다.
    void VisitUpdate()
    {
        if (nowDialogIndex < dialogCount)
        {
            if (visitorRuelliaToggle)
            {
                if (!visitorText.transform.parent.gameObject.activeSelf)
                    visitorText.transform.parent.gameObject.SetActive(true);
                string str = visitDialog[nowDialogIndex];
                MakeBackLog(true, str);
                StartCoroutine(sceneManager.LoadTextOneByOne(str, visitorText));
                visitorRuelliaToggle = !visitorRuelliaToggle;

            }
            else
            {
                if (!ruelliaText.transform.parent.gameObject.activeSelf)
                    ruelliaText.transform.parent.gameObject.SetActive(true);
                string str = symptomDialog.ruelliaDialog[Random.Range(0, symptomDialog.ruelliaDialog.Length)];
                MakeBackLog(false, str);
                StartCoroutine(sceneManager.LoadTextOneByOne(str, ruelliaText));
                visitorRuelliaToggle = !visitorRuelliaToggle;
                nowDialogIndex++;

            }
        }
        else
        {
            nowTalking = false;
            counterManager.VisitorTalkEnd();
            ruelliaText.transform.parent.gameObject.SetActive(false);
            visitorText.transform.parent.gameObject.SetActive(false);
            ruelliaText.text = "";
            visitorText.text = "";
            nowState = CounterState.NotTalking;
            visitorRuelliaToggle = true;
        }
    }

    void StartUpdate()
    {
        if (nowDialogIndex < dialogCount)
        {
            string str = randomDialogClassList[nowDialogClassIndex].dialogList[nowDialogIndex].str;
            MakeBackLog(false, str);
            StartCoroutine(sceneManager.LoadTextOneByOne(str, counterText));
            nowDialogIndex++;
        }
        else
        {
            nowTalking = false;
            counterText.text = "";
            nowState = CounterState.NotTalking;
            counterManager.CounterStart();
        }
    }

    void EndUpdate()
    {
        if (nowDialogIndex < dialogCount)
        {
            if (visitorRuelliaToggle)
            {
                if (!visitorText.transform.parent.gameObject.activeSelf)
                    visitorText.transform.parent.gameObject.SetActive(true);
                string str = nowEndDialog.visitorDialog[nowDialogIndex].str;
                MakeBackLog(true, str);
                StartCoroutine(sceneManager.LoadTextOneByOne(str, visitorText));
                visitorRuelliaToggle = !visitorRuelliaToggle;
            }
            else
            {
                if (!ruelliaText.transform.parent.gameObject.activeSelf)
                    ruelliaText.transform.parent.gameObject.SetActive(true);
                string str = nowEndDialog.ruelliaDialog[nowDialogIndex].str;
                MakeBackLog(false, str);
                StartCoroutine(sceneManager.LoadTextOneByOne(str, ruelliaText));
                visitorRuelliaToggle = !visitorRuelliaToggle;
                nowDialogIndex++;
            }
        }
        else
        {
            nowTalking = false;
            ruelliaText.text = "";
            visitorText.text = "";
            ruelliaText.transform.parent.gameObject.SetActive(false);
            visitorText.transform.parent.gameObject.SetActive(false);
            nowState = CounterState.NotTalking;
            counterManager.VisitorDisappear();
            visitorRuelliaToggle = true;
        }
    }
}
