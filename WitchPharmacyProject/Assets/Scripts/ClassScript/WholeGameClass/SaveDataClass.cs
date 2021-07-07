using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//게임 전체를 저장하는 세이브데이터 클래스
[System.Serializable]
public class SaveDataClass
{
    public string name;
    public float nowTime;
    public int nowDay;
    public int coin;
    //public List<int> owningMedicineList;  //현재 가지고있는거
    public List<int> ownedMedicineList;   //딕셔너리
    //Key값은 medicine종류에 해당하는 medicineDictionary의 index
    //Value값은 그 약을 얼마나 가지고 있는지. 만약 0이라면 Dictionary에서 삭제.

    //이시발....시발........딕셔너리는 json저장이 안된대. ....
    //public Dictionary<int, int> owningMedicineDictionary;

    //그래서 리스트로 저장을 하고 딕셔너리를 만들어줄거임 ㅋㅋ....
    //이거 세이브 할 때 dictionary랑 owningMedicineList랑 동기화 해줘야됨 안그러면 세이브 젓댐
    public List<OwningMedicineClass> owningMedicineList;
    public List<int> unlockedRegionIndex;
    public List<OwningToolClass> owningToolList;

    public string nowCounterDialogBundleName;
    public string nowStoryDialogBundleName;



    public SaveDataClass()
    {
        name = "initName";
        coin = 1000;
        nowDay = 0;
        nowTime = 0;
        //owningMedicineDictionary = new Dictionary<int, int>();
        ownedMedicineList = new List<int>();
        owningMedicineList = new List<OwningMedicineClass>();
        unlockedRegionIndex = new List<int>();
        owningToolList = new List<OwningToolClass>();

    }
}
