using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpecialEventCondition
{
    public string fileName;
    public int leastDayCondition;
    public List<string> questConditionList;
    public List<RoutePair> routeConditionList;
    
    public SpecialEventCondition()
    {
        fileName = "null";
        leastDayCondition = 0;
        questConditionList = new List<string>();
        questConditionList.Add("null");
        routeConditionList = new List<RoutePair>();
        routeConditionList.Add(new RoutePair());
    }

}
