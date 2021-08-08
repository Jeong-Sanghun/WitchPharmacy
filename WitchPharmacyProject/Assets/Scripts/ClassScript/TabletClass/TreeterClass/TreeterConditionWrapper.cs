using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TreeterConditionWrapper
{
    public List<TreeterCondition> treeterConditionList;

    public TreeterConditionWrapper()
    {
        treeterConditionList = new List<TreeterCondition>();
        treeterConditionList.Add(new TreeterCondition());
        treeterConditionList.Add(new TreeterCondition());
        treeterConditionList.Add(new TreeterCondition());
        treeterConditionList.Add(new TreeterCondition());
    }
}
