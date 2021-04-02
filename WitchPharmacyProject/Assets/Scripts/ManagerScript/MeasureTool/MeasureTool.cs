using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeasureTool : MonoBehaviour    //SH
{
    GameManager gameManager;
    [SerializeField]
    MeasureToolManager measureToolManager;

    GameObject toolObject;
    

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.singleTon;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //measureToolManager에서 켜줄거임. MeasurTool[5]개로 받아올거
    public void ToolActive(bool active)
    {
        toolObject.SetActive(active);
    }

    //상속된것들에서 씀
    protected void MeasureEnd(int index)
    {
        measureToolManager.MeasureEnd(index);
    }
}
