using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
