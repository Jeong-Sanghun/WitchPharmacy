using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StoryRegion
{
    RuinCity,Narin,Themnos,Rebav,Mikcha,TreeOfLife,NotAllocated
}

[System.Serializable]
public class SpecialVisitorConditionWrapper
{
    public List<SpecialVisitorCondition> specialVisitorConditionDataList;

    public SpecialVisitorConditionWrapper()
    {
        specialVisitorConditionDataList = new List<SpecialVisitorCondition>();
        specialVisitorConditionDataList.Add(new SpecialVisitorCondition());
        specialVisitorConditionDataList.Add(new SpecialVisitorCondition());
    }
}
