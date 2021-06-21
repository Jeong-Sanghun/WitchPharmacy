using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//json
[System.Serializable]
public class RegionAvailableMedicine
{
    public List<int> medicineIndexList;
    //이거도 제이슨으로 받아올거. 여기에 무슨 약이 들어가는지.
    public RegionAvailableMedicine()
    {
        medicineIndexList = new List<int>();
        for(int i = 0; i < 8; i++)
        {
            medicineIndexList.Add(i);
        }
    }
}
