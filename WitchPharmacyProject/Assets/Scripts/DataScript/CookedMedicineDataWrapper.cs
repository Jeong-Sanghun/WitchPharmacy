using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//제이슨으로 받아올 정보
[System.Serializable]
public class CookedMedicineDataWrapper  
{
    public List<CookedMedicineData> cookedMedicineDataList;

    public CookedMedicineDataWrapper()
    {
        cookedMedicineDataList = new List<CookedMedicineData>();
    }
}
