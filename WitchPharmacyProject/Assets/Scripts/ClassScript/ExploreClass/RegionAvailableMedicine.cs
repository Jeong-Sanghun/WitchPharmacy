using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RegionAvailableMedicine
{
    public List<int> medicineIndexList;

    public RegionAvailableMedicine()
    {
        medicineIndexList = new List<int>();
        for(int i = 0; i < 8; i++)
        {
            medicineIndexList.Add(i);
        }
    }
}
