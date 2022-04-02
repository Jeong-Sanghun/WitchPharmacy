using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeasureToolManager : MonoBehaviour
{
    enum ToolState {
        NotOwned,ToolOwned, AutoOwned
    }
    GameManager gameManager;
    [SerializeField]
    CounterManager counterManager;
    [SerializeField]
    MedicineManager medicineManager;
    [SerializeField]
    BlurManager blurManager;

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
    SaveDataClass saveData;
    [SerializeField]
    GameObject visitorParent;
    [SerializeField]
    GameObject counterTable;
    

    [SerializeField]
   public MeasureTool[] measureToolArray;

    int[] symptomNumberArray;
    ToolState[] toolStateArray;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.singleton;
        measureToolExitButtonObject.SetActive(false);
        saveData = gameManager.saveData;
        toolStateArray = new ToolState[4];

        
        for(int i = 0; i < toolStateArray.Length; i++)
        {
            toolStateArray[i] = ToolState.NotOwned;
        }

        for(int i = 0; i < saveData.owningMeasureToolList.Count; i++)
        {
            int symptom;
            if(saveData.owningMeasureToolList[i] > 3)
            {
                symptom = saveData.owningMeasureToolList[i] % 4;
                toolStateArray[symptom] = ToolState.AutoOwned;
            }
            else
            {
                
                symptom = saveData.owningMeasureToolList[i];
                if (toolStateArray[symptom] == ToolState.NotOwned)
                    toolStateArray[symptom] = ToolState.ToolOwned;
            }
        }
        for(int i = 0; i < toolStateArray.Length; i++)
        {
            if(toolStateArray[i] == ToolState.NotOwned)
            {
                measureToolButtonArray[i].SetActive(false);
            }
            else
            {
                measureToolButtonArray[i].SetActive(true);
            }
        }

        for(int i = 0; i < gameManager.saveData.owningMeasureToolList.Count; i++)
        {
            measureToolButtonArray[gameManager.saveData.owningMeasureToolList[i]%4].SetActive(true);
        }


        
    }

    //CounterManager의 SpawnRandomVisitor에서 호출
    public void OnNewVisitor(int[] symptomNumArr)
    {
        symptomNumberArray = symptomNumArr;
        for (int i = 0; i < measureToolArray.Length; i++)
        {
            if (toolStateArray[i] == ToolState.AutoOwned)
            {
                measureToolArray[i].OnNewVisitor(symptomNumberArray[i], i,true);
            }
            else if (toolStateArray[i] == ToolState.ToolOwned)
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
        //blurManager.OnBlur(false);
        blurManager.ChangeLayer(false, visitorParent);
        blurManager.ChangeLayer(false, counterTable);
        counterManager.DialogActive(true);
        measureToolArray[nowMeasureToolIndex].ToolActive(false);
        for (int i = 0; i < existingUIObjects.Length; i++)
        {
            existingUIObjects[i].SetActive(true);
        }
        TabletManager.inst.TabletOpenButtonActive(true,false);
        measureToolExitButtonObject.SetActive(false);
    }

    //드래그앤드롭하면 이거 실행됨. 카운터매니저에서 실행
    public void ToolOpenButton(int index)
    {
        //blurManager.OnBlur(true);
        blurManager.ChangeLayer(true, visitorParent);
        blurManager.ChangeLayer(true, counterTable);
        nowMeasureToolIndex = index;
        counterManager.DialogActive(false);
        measureToolArray[index].ToolActive(true);
        for(int i = 0; i < existingUIObjects.Length; i++)
        {
            existingUIObjects[i].SetActive(false);
        }
        TabletManager.inst.TabletOpenButtonActive(false,false);
        measureToolExitButtonObject.SetActive(true);
    }
    
    public void ToolBoxOpenButton()
    {
        measureToolButtonParent.SetActive(!measureToolButtonParent.activeSelf);
    }
}
