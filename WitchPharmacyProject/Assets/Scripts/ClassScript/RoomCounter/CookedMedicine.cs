using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//조제완료 된 조합의 이름을 저장하는 리스트
public class CookedMedicine
{
    public string name;
    public int[] medicineArray;
    public GameObject medicineObject;
    public int medicineCount;
    public bool isSpecialMedicine;

    public CookedMedicine()
    {
        name = "null";
        medicineArray = new int[3];
        for(int i = 0; i < 3; i++)
        {
            medicineArray[i] = 0;
        }
        isSpecialMedicine = false;
    }
}
