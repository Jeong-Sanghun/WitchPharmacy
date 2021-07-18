using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NowStoryScene
{
    DayStart, AfterCounter, AfterExplore
}

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

    public List<OwningMedicineClass> owningSpecialMedicineList;

    public float chaosMeter;

    public string nowCounterDialogBundleName;
    public string nowStoryDialogBundleName;

    public List<string> solvedQuestBundleName;
    public List<string> progressingQuestBundleName;
    public List<OneDayBillWrapper> billWrapperList;
    public List<OwningDocumentClass> owningDocumentList;
    public List<string> readStoryList;
    public string nextLoadSceneName;
    public SaveTime nowSaveTime;
    //public string nowLanguageDirectory;  //Korean/ 이렇게 들어가야함.


    public SaveDataClass()
    {
        name = "initName";
        coin = 1000;
        nowDay = 0;
        nowTime = 0;
        ownedMedicineList = new List<int>();
        ownedMedicineList.Add(0);
        owningMedicineList = new List<OwningMedicineClass>();
        owningMedicineList.Add(new OwningMedicineClass(0, 10));
        owningSpecialMedicineList = new List<OwningMedicineClass>();
        progressingQuestBundleName = new List<string>();
        unlockedRegionIndex = new List<int>();
        for(int i = 0; i < 9; i++)
        {
            unlockedRegionIndex.Add(i);
        }
        owningToolList = new List<OwningToolClass>();
        chaosMeter = 0;
        solvedQuestBundleName = new List<string>();
        nowCounterDialogBundleName = "testBundle";
        nowStoryDialogBundleName = "testBundle";
        //nowLanguageDirectory = "Korean/";
        billWrapperList = new List<OneDayBillWrapper>();
        owningDocumentList = new List<OwningDocumentClass>();
        readStoryList = new List<string>();
        nextLoadSceneName = "StoryScene";
        nowSaveTime = SaveTime.DayStart;
        //billWrapperList.Add(new OneDayBillWrapper());
        //        solvedQuestBundleName.Add("testBundle");


    }
}
