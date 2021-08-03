using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum RegularEvent
{
    MedicineDiscount, ResearchProgress, DocumnetMedicine, DocumentResearch
}
[System.Serializable]
public class RegionData
{
    public string fileName;
    public string ingameName;
    public List<SpecialEventCondition> specialEventConditionList;
    public int[] eventTimeArray;
    public string[] appearingDocumentList;

    public RegionData()
    {
        fileName = "null";
        ingameName = "널";
        specialEventConditionList = new List<SpecialEventCondition>();
        specialEventConditionList.Add(new SpecialEventCondition());
        specialEventConditionList.Add(new SpecialEventCondition());
        eventTimeArray = new int[4];
        appearingDocumentList = new string[3];
        for(int i = 0; i < 3; i++)
        {
            appearingDocumentList[i] = "null";
        }
        
    }

}
