using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpecialVisitorCondition
{
    public int priority;
    public string bundleName;
    public string specialMedicine;
    public float appearingChaosMeter;
    public List<string> appearingSolvedQuestBundleList;
    public List<string> appearingProgressingQuestBundleList;
    public List<string> appearingSpecialMedicineList;

    public int appearingLeastDay;
    public List<string> appearingStoryBundleList;


    public SpecialVisitorCondition()
    {
        priority = 0;
        bundleName = "Lily";
        appearingChaosMeter = 0;
        appearingSolvedQuestBundleList = new List<string>();
        appearingProgressingQuestBundleList = new List<string>();
        appearingSpecialMedicineList = new List<string>();
        appearingStoryBundleList = new List<string>();
        appearingLeastDay = 0;
        //appearingQuestBundleList = new List<string>();
        //appearingQuestBundleList.Add("none");
    }

}
