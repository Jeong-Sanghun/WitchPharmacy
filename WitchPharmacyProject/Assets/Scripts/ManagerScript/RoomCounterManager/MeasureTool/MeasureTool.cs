using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeasureTool : MonoBehaviour    //SH
{
    GameManager gameManager;
    [SerializeField]
    MeasureToolManager measureToolManager;
    [SerializeField]
    MedicineManager medicineManager;
    [SerializeField]
    CounterManager countermanager;
    [SerializeField]
    SymptomChartManager symptomChartManager;

    GameObject toolObject;
    protected int symptomNumber;

    //CounterManager에서 측정이 끝났는지 알아야 토글을 못하게 막는다.
    public bool measureEnd = false;
    int toolIndex = 1;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.singleTon;
        measureEnd = false;
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

    //measureToolManager에서 불러줌씨발;
    public virtual void OnNewVisitor(int symptomNum,int index)
    {
        symptomNumber = symptomNum;
        measureEnd = false;
        toolIndex = index;
    }

    //상속된것들에서 씀
    protected virtual void MeasureEnd()
    {
        symptomChartManager.SymptomMeasured(toolIndex);
        measureEnd = true;
        countermanager.OnMeasureEnd(toolIndex);
    }


}
