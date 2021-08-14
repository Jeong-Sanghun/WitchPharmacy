using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RegionSaveData
{
    public StoryRegion storyRegion;
    public RegionName regionName;
    public List<int> firstDiscountedMedicineIndex;
    public List<int> secondDiscountedMedicineIndex;
    public int[] eventTimeArray;
    public List<string> seenSpecialEventList;

    public RegionSaveData()
    {
        storyRegion = StoryRegion.Narin;
        regionName = RegionName.Library;
        firstDiscountedMedicineIndex = new List<int>();
        secondDiscountedMedicineIndex = new List<int>();
        eventTimeArray = new int[4];
        seenSpecialEventList = new List<string>();

    }

}
