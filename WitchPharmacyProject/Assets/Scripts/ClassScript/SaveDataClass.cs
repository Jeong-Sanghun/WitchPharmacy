using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//게임 전체를 저장하는 세이브데이터 클래스
[System.Serializable]
public class SaveDataClass
{
    public string name;
    public List<int> owningMedicineList;  //현재 가지고있는거
    public List<int> ownedMedicineList;   //딕셔너리

    public SaveDataClass()
    {
        name = "initName";
        owningMedicineList = new List<int>();
        ownedMedicineList = new List<int>();
        


    }
}
