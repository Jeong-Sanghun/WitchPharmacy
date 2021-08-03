using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ResearchSaveData
{
    public List<OneResearch> progressingMeasureToolResearchList;
    public List<string> endMeasureToolResearchList;

    public List<OneResearch> progressingMedicineResearchList;
    public List<string> endMedicineResearchList;

    public List<OneResearch> progressingOtherToolResearchList;
    public List<string> endOtherToolResearchList;

    public ResearchSaveData()
    {
        progressingMedicineResearchList = new List<OneResearch>();
        endMedicineResearchList = new List<string>();

        progressingMeasureToolResearchList = new List<OneResearch>();
        endMeasureToolResearchList = new List<string>();

        progressingOtherToolResearchList = new List<OneResearch>();
        endOtherToolResearchList = new List<string>();
    }
}
