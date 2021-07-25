using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MeasureToolResearchDataWrapper
{
    public List<MeasureToolResearchData> measureToolResearchDataList;

    public MeasureToolResearchDataWrapper()
    {
        measureToolResearchDataList = new List<MeasureToolResearchData>();
        for(int i = 0; i < 8; i++)
        {
            measureToolResearchDataList.Add(new MeasureToolResearchData());
        }
    }
}
