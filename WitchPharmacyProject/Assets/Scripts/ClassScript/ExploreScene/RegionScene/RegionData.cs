using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum RegionEvent
{
    MedicineDiscount, ResearchProgress, DocumentMedicine, DocumentResearch, RandomCoin,SpecialEvent
}


[System.Serializable]
public class RegionData
{
    public string fileName;
    public string ingameName;
    public List<SpecialEventCondition> specialEventConditionList;
    public int[] appearingMedicineArray;
    public int[] eventTimeArray;
    public string[] appearingDocumentArray;
    public string[] appearingResearchArray;

    public RegionData()
    {
        fileName = "null";
        ingameName = "널";
        specialEventConditionList = new List<SpecialEventCondition>();
        specialEventConditionList.Add(new SpecialEventCondition());
        specialEventConditionList.Add(new SpecialEventCondition());
        eventTimeArray = new int[4];
        appearingDocumentArray = new string[3];
        appearingMedicineArray = new int[10];
        for(int i = 0; i < 10; i++)
        {
            appearingMedicineArray[i] = i;
        }
        for(int i = 0; i < 3; i++)
        {
            appearingDocumentArray[i] = "null";
        }
        
    }

}
