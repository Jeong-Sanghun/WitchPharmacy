using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RegionSaveData
{
    public int regionIndex;
    public List<int> firstDiscountedMedicineIndex;
    public List<int> secondDiscountedMedicineIndex;
    public int[] eventTimeArray;

    public RegionSaveData()
    {
        regionIndex = -1;
        firstDiscountedMedicineIndex = new List<int>();
        secondDiscountedMedicineIndex = new List<int>();
        eventTimeArray = new int[4];

    }

}
