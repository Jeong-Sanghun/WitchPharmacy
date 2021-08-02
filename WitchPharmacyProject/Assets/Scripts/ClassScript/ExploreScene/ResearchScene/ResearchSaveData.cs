using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ResearchSaveData
{
    public List<OneResearch> progressingMeasureToolReaserchList;
    public List<string> endMeasureToolResearchList;

    public List<OneResearch> progressingMedicineResearchList;
    public List<string> endMedicineResearchList;

    public ResearchSaveData()
    {
        progressingMedicineResearchList = new List<OneResearch>();
        endMedicineResearchList = new List<string>();

        progressingMeasureToolReaserchList = new List<OneResearch>();
        endMeasureToolResearchList = new List<string>();
    }
}
