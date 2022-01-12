﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StoryRegion
{
    Narin,Themnos,Rebav,Mikcha,TreeOfLife
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
