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
    //int로 쓰는 이유는 MeidcineDictionary에 index값을 뽑아서 쓸 것이기 때문이다.
    //json이 길어지면 데이터 저장하는데 귀찮아짐. 그리고 똑같은 값을 dictionary에서도 쓰고 여기서도 쓸 이유가 없음.

    public SaveDataClass()
    {
        name = "initName";
        owningMedicineList = new List<int>();
        ownedMedicineList = new List<int>();
        


    }
}
