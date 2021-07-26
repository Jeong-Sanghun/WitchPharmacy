using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MeasureToolDataWrapper
{
    public List<MeasureToolData> measureToolDataList;

    public MeasureToolDataWrapper()
    {
        measureToolDataList = new List<MeasureToolData>();

        measureToolDataList.Add(new MeasureToolData());
        measureToolDataList.Add(new MeasureToolData());
        measureToolDataList.Add(new MeasureToolData());
        measureToolDataList.Add(new MeasureToolData());
    }
}
