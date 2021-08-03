using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OtherToolDataWrapper
{
    public List<OtherToolData> otherToolDataList;

    public OtherToolDataWrapper()
    {
        otherToolDataList = new List<OtherToolData>();
        otherToolDataList.Add(new OtherToolData());
        otherToolDataList.Add(new OtherToolData());
    }
}
