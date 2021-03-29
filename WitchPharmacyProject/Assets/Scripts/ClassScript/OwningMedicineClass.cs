using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
//저장용. 딕셔너리 안쓸거임.
public class OwningMedicineClass    //SH
{
    public int medicineIndex;
    public int medicineQuantity;

    public OwningMedicineClass()
    {
        medicineIndex = 0;
        medicineQuantity = 0;
    }

    public OwningMedicineClass(int index, int quantity)
    {
        medicineIndex = index;
        medicineQuantity = quantity;
    }
}
