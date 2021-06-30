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
    Text counterText;
    [SerializeField]
    Text visitorText;
    [SerializeField]
    Text ruelliaText;
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

        nowDialogIndex = 0;
        nowDialogClassIndex = Random.Range(0, randomDialogClassList.Count);
        dialogCount = randomDialogClassList[nowDialogClassIndex].dialogList.Count;
        StartCoroutine(sceneManager.LoadTextOneByOne(randomDialogClassList[nowDialogClassIndex].dialogList[nowDialogIndex].str, counterText));
        nowDialogIndex++;
    }

    public void OnVisitorVisit(RandomVisitorClass visitor)
    {
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

    public void OnVisitorEnd(bool wrongMedicine)
    {
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
        if (Input.GetMouseButtonDown(0) && !sceneManager.nowTexting)
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
    
    void VisitUpdate()
    {
        if (nowDialogIndex < dialogCount)
        {
            if (visitorRuelliaToggle)
            {
                StartCoroutine(sceneManager.LoadTextOneByOne(visitDialog[nowDialogIndex], visitorText));
                visitorRuelliaToggle = !visitorRuelliaToggle;
            }
            else
            {
                StartCoroutine(sceneManager.LoadTextOneByOne(symptomDialog.ruelliaDialog[Random.Range(0,symptomDialog.ruelliaDialog.Length)], ruelliaText));
                visitorRuelliaToggle = !visitorRuelliaToggle;
                nowDialogIndex++;
            }
        }
        else
        {
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
            StartCoroutine(sceneManager.LoadTextOneByOne(randomDialogClassList[nowDialogClassIndex].dialogList[nowDialogIndex].str, counterText));
            nowDialogIndex++;
        }
        else
        {
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
                StartCoroutine(sceneManager.LoadTextOneByOne(nowEndDialog.visitorDialog[nowDialogIndex].str, visitorText));
                visitorRuelliaToggle = !visitorRuelliaToggle;
            }
            else
            {
                StartCoroutine(sceneManager.LoadTextOneByOne(nowEndDialog.ruelliaDialog[nowDialogIndex].str, ruelliaText));
                visitorRuelliaToggle = !visitorRuelliaToggle;
                nowDialogIndex++;
            }
        }
        else
        {
            ruelliaText.text = "";
            visitorText.text = "";
            nowState = CounterState.NotTalking;
            counterManager.VisitorDisappear();
            visitorRuelliaToggle = true;
        }
    }
}
