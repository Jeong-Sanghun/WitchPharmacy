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


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.singleTon;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //measureTool에서 메져링이 끝나면 불러옴
    public void MeasureEnd(int index)
    {
        counterManager.DialogActive(true);
        toolObjects[index].SetActive(false);
    }

    //버튼누르면 이거실행됨
    public void ToolOpenButton(int index)
    {
        counterManager.DialogActive(false);
        toolObjects[index].SetActive(true);
    }
}
