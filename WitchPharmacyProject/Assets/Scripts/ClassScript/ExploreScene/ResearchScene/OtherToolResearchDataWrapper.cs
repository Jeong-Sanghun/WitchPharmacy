using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OtherToolResearchDataWrapper
{
    public List<OtherToolResearchData> otherToolResearchDataList;

    public OtherToolResearchDataWrapper()
    {
        otherToolResearchDataList = new List<OtherToolResearchData>();
        otherToolResearchDataList.Add(new OtherToolResearchData());
        otherToolResearchDataList.Add(new OtherToolResearchData());
        otherToolResearchDataList.Add(new OtherToolResearchData());

    }
}
