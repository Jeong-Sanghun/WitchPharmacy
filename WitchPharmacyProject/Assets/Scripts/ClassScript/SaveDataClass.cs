using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//게임 전체를 저장하는 세이브데이터 클래스
[System.Serializable]
public class SaveDataClass
{
    public string name;
    public List<MedicineClass> owningMedicineList;
    

    public SaveDataClass()
    {

    }
}
