using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ResearchSaveData
{
    public List<OneMeasureToolResearch> progressingMeasureToolReaserchList;
    public List<string> endMeasureToolResearchList;

    public ResearchSaveData()
    {
        progressingMeasureToolReaserchList = new List<OneMeasureToolResearch>();
        endMeasureToolResearchList = new List<string>();
    }
}
