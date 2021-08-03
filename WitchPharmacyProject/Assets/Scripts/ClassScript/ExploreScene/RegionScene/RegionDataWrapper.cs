using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RegionDataWrapper
{
    public List<RegionData> regionDataList;

    public RegionDataWrapper()
    {
        regionDataList = new List<RegionData>();
        regionDataList.Add(new RegionData());
        regionDataList.Add(new RegionData());
        regionDataList.Add(new RegionData());
        regionDataList.Add(new RegionData());
    }
}
