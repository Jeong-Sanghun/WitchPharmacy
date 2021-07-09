using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Region
{
    Narin,Themnos,Rebav,Mikcha,TreeOfLIfe
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
