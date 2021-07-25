using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
//저장용. 딕셔너리 안쓸거임.
public class OwningMedicineClass    //SH
{
    public int medicineIndex;
    public int medicineCost;

    public OwningMedicineClass()
    {
        medicineIndex = 0;
        medicineCost = 30;
    }

    public OwningMedicineClass(int index, int cost)
    {
        medicineIndex = index;
        medicineCost = cost;
    }
}
