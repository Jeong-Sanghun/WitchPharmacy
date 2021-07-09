using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpecialVisitorCondition
{
    public int priority;
    public string bundleName;
    public float appearingProgression;
    public List<string> appearingQuestBundleList;

    public SpecialVisitorCondition()
    {
        priority = 0;
        bundleName = "Lily";
        appearingProgression = 0;
        appearingQuestBundleList = new List<string>();
        appearingQuestBundleList.Add("none");
    }

}
