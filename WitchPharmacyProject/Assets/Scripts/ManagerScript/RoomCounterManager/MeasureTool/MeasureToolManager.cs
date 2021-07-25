using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeasureToolManager : MonoBehaviour
{
    enum ToolState {
        NotResearched,ToolResearched, AutoResearched
    }
    GameManager gameManager;
    [SerializeField]
    CounterManager counterManager;
    [SerializeField]
    MedicineManager medicineManager;

    [SerializeField]
    GameObject buttonParent;

    [SerializeField]
    GameObject[] toolObjects;
    int nowMeasureToolIndex;

    [SerializeField]
    GameObject[] existingUIObjects;
    [SerializeField]
    GameObject measureToolExitButtonObject;
    [SerializeField]
    GameObject measureToolButtonParent;
    [SerializeField]
    GameObject[] measureToolButtonArray;
    ResearchSaveData researchSaveData;


    [SerializeField]
   public MeasureTool[] measureToolArray;

    int[] symptomNumberArray;
    ToolState[] toolStateArray;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.singleTon;
        measureToolExitButtonObject.SetActive(false);
        researchSaveData = gameManager.saveData.researchSaveData;
        toolStateArray = new ToolState[4];
        for(int i = 0; i < toolStateArray.Length; i++)
        {
            toolStateArray[i] = ToolState.NotResearched;
        }

        for(int i = 0; i < researchSaveData.endMeasureToolResearchList.Count; i++)
        {
            Symptom containingSymptom = Symptom.none;
            string nowString = researchSaveData.endMeasureToolResearchList[i];
            for(int j = 0; j < 4; j++)
            {
                string contain = ((Symptom)j).ToString();
                if (nowString.Contains(contain))
                {
                    containingSymptom = (Symptom)j;
                    break;
                }
            }
            if(containingSymptom == Symptom.none)
            {
                continue;
            }

            if (nowString.Contains("Tool"))
            {
                if(toolStateArray[(int)containingSymptom] == ToolState.NotResearched)
                {
                    toolStateArray[(int)containingSymptom] = ToolState.ToolResearched;
                }
            }
            if (nowString.Contains("Auto"))
            {
                
                toolStateArray[(int)containingSymptom] = ToolState.AutoResearched;
            }
        }
        for(int i = 0; i < toolStateArray.Length; i++)
        {
            if(toolStateArray[i] == ToolState.NotResearched)
            {
                measureToolButtonArray[i].SetActive(false);
            }
            else
            {
                measureToolButtonArray[i].SetActive(true);
            }
        }


        
    }

    //CounterManager의 SpawnRandomVisitor에서 호출
    public void OnNewVisitor(int[] symptomNumArr)
    {
        symptomNumberArray = symptomNumArr;
        for (int i = 0; i < measureToolArray.Length; i++)
        {
            if (toolStateArray[i] == ToolState.AutoResearched)
            {
                measureToolArray[i].OnNewVisitor(symptomNumberArray[i], i,true);
            }
            else if (toolStateArray[i] == ToolState.ToolResearched)
            {
                measureToolArray[i].OnNewVisitor(symptomNumberArray[i], i, false);
            }
            
        }
    }

    //measureTool에서 메져링이 끝나면 불러옴
    public void MeasureEnd(int index)
    {

        counterManager.DialogActive(true);
        toolObjects[index].SetActive(false);
    }

    public void BackButton()
    {
        counterManager.DialogActive(true);
        measureToolArray[nowMeasureToolIndex].ToolActive(false);
        for (int i = 0; i < existingUIObjects.Length; i++)
        {
            existingUIObjects[i].SetActive(true);
        }
        measureToolExitButtonObject.SetActive(false);
    }

    //드래그앤드롭하면 이거 실행됨. 카운터매니저에서 실행
    public void ToolOpenButton(int index)
    {
        nowMeasureToolIndex = index;
        counterManager.DialogActive(false);
        measureToolArray[index].ToolActive(true);
        for(int i = 0; i < existingUIObjects.Length; i++)
        {
            existingUIObjects[i].SetActive(false);
        }
        measureToolExitButtonObject.SetActive(true);
    }
    
    public void ToolBoxOpenButton()
    {
        measureToolButtonParent.SetActive(!measureToolButtonParent.activeSelf);
    }
}
