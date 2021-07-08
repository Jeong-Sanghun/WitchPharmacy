using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpecialMedicineDataWrapper
{
    public List<SpecialMedicineClass> specialMedicineDataList;

    public SpecialMedicineDataWrapper()
    {
        specialMedicineDataList = new List<SpecialMedicineClass>();
        for(int i = 0; i < 4; i++)
        {
            specialMedicineDataList.Add(new SpecialMedicineClass());
        }
    }
}
