using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeasureToolManager : MonoBehaviour
{
    GameManager gameManager;
    [SerializeField]
    CounterManager counterManager;
    [SerializeField]
    RoomManager roomManager;

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
   public MeasureTool[] measureToolArray;

    int[] symptomNumberArray;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.singleTon;
        measureToolExitButtonObject.SetActive(false);
    }

    //CounterManager의 SpawnRandomVisitor에서 호출
    public void OnNewVisitor(int[] symptomNumArr)
    {
        symptomNumberArray = symptomNumArr;
        for(int i = 0; i < measureToolArray.Length; i++)
        {
            measureToolArray[i].OnNewVisitor(symptomNumberArray[i],i);
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
        toolObjects[nowMeasureToolIndex].SetActive(false);
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
        toolObjects[index].SetActive(true);
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
