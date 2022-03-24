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
    //public List<int> ownedMedicineList;   //딕셔너리
    //Key값은 medicine종류에 해당하는 medicineDictionary의 index
    //Value값은 그 약을 얼마나 가지고 있는지. 만약 0이라면 Dictionary에서 삭제.

    //이시발....시발........딕셔너리는 json저장이 안된대. ....
    //public Dictionary<int, int> owningMedicineDictionary;

    //그래서 리스트로 저장을 하고 딕셔너리를 만들어줄거임 ㅋㅋ....
    public List<OwningMedicineClass> owningMedicineList;
    public List<OwningMedicineClass> owningSpecialMedicineList;


    public float chaosMeter;

    public string nowCounterDialogBundleName;
    public string nowStoryDialogBundleName;

    public List<OneDayBillWrapper> billWrapperList;
    public List<OwningDocumentClass> owningDocumentList;
    public List<string> symptomBookList;
    public List<int> owningMeasureToolList;
    public List<string> owningOtherToolList;
    public List<string> endQuestList;
    public ResearchSaveData researchSaveData;
    public List<RoutePair> routePairList;
    public List<RegionSaveData> regionSaveDataList;
    public List<int> likedTreeterIndexList;
    public bool[] highlightedButton;

    public List<string> readStoryList;
    public List<string> readSpecialVisitorBundleList;
    public string nextStory;
    public int nowSceneIndex;
    //public string nextLoadSceneName;
    public SaveTime nowSaveTime;
    public StoryRegion nowRegion;
    public string forcedRegion;
    public string nowBossFile;
    //public string nowLanguageDirectory;  //Korean/ 이렇게 들어가야함.


    public SaveDataClass()
    {
        name = "initName";
        coin = 1000;
        nowDay = 0;
        nowTime = 0;
        //ownedMedicineList = new List<int>();
        //ownedMedicineList.Add(0);
        owningMedicineList = new List<OwningMedicineClass>();
        owningSpecialMedicineList = new List<OwningMedicineClass>();
        //progressingQuestBundleName = new List<string>();
        //calledQuestBundleName = new List<string>();
        chaosMeter = 0;
        //solvedQuestBundleName = new List<string>();
        nowCounterDialogBundleName = "testBundle";
        nowStoryDialogBundleName = "testBundle";
        //nowLanguageDirectory = "Korean/";
        billWrapperList = new List<OneDayBillWrapper>();
        owningDocumentList = new List<OwningDocumentClass>();
        //visitedOddVisitorName = new List<string>();
        //readStoryList = new List<string>();
        nowSceneIndex = 0;
        //nextLoadSceneName = "StoryScene";
        nowSaveTime = SaveTime.DayStart;
        researchSaveData = new ResearchSaveData();
        symptomBookList = new List<string>();
        symptomBookList.Add("water+");
        symptomBookList.Add("water-");
        symptomBookList.Add("fire+");
        symptomBookList.Add("fire-");
        nextStory = "firstDay";
        readStoryList = new List<string>();
        routePairList = new List<RoutePair>();
        regionSaveDataList = new List<RegionSaveData>();
        owningMeasureToolList = new List<int>();
        owningOtherToolList = new List<string>();
        endQuestList = new List<string>();
        nowRegion = StoryRegion.RuinCity;
        likedTreeterIndexList = new List<int>();
        highlightedButton = new bool[3];
        readSpecialVisitorBundleList = new List<string>();
        for(int i = 0; i < highlightedButton.Length;i++)
        {
            highlightedButton[i] = false;
        }

        //billWrapperList.Add(new OneDayBillWrapper());
        //        solvedQuestBundleName.Add("testBundle");
    }

    public void AddMedicineBySymptom(MedicineDataWrapper dataWrapper, Symptom firstSymptom,Symptom secondSymptom)
    {
        List<MedicineClass> dataList = dataWrapper.medicineDataList;
        for(int  i = 0; i < dataList.Count; i++)
        {
            if(dataList[i].GetFirstSymptom() == firstSymptom && dataList[i].GetSecondSymptom() == secondSymptom)
            {
                OwningMedicineClass owningMedicine = new OwningMedicineClass(i,dataList[i].cost);
                owningMedicineList.Add(owningMedicine);
            }
            else if (dataList[i].GetFirstSymptom() == secondSymptom && dataList[i].GetSecondSymptom() == firstSymptom)
            {

                OwningMedicineClass owningMedicine = new OwningMedicineClass(i, dataList[i].cost);
                owningMedicineList.Add(owningMedicine);
            }
        }

        //bool firstContain = false;
        //bool secondContain = false;
        //for(int i = 0; i < symptomBookList.Count; i++)
        //{
        //    if (symptomBookList[i].Contains(firstSymptom.ToString()))
        //    {
        //        firstContain = true;
        //    }
        //    if (symptomBookList[i].Contains(secondSymptom.ToString()))
        //    {
        //        secondContain = true;
        //    }
        //}
        //if(firstContain == false)
        //{
        //    symptomBookList.Add(firstSymptom.ToString() + "+");
        //    symptomBookList.Add(firstSymptom.ToString() + "-");
        //}
        //if (secondContain == false)
        //{
        //    symptomBookList.Add(secondSymptom.ToString() + "+");
        //    symptomBookList.Add(secondSymptom.ToString() + "-");

        //}


    }
}
