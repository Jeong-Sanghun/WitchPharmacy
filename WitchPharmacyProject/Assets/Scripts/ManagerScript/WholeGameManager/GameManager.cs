using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour //SH
{
    //singleTon 모든 매니저 스크립트에서 참조
    public static GameManager singleTon;
    [HideInInspector]
    public SceneManager sceneManager;
    TabletManager tabletManager;
    //모든 매니저 스크립트에서 참조
    
    public SaveDataClass saveData;
    public SaveDataTimeWrapper saveDataTimeWrapper;
    public MedicineDataWrapper medicineDataWrapper;
    [HideInInspector]
    public RegionPropertyWrapper regionPropertyWrapper;
    [HideInInspector]
    public StoreToolDataWrapper storeToolDataWrapper;

    [HideInInspector]
    public ConversationDialogBundleWrapper conversationDialogBundleWrapper;
    [HideInInspector]
    public ConversationDialogBundle conversationDialogBundle;
    //[HideInInspector]
    public StartDialogClassWrapper randomDialogDataWrapper;
    //public CookedMedicineDataWrapper cookedMedicineDataWrapper;
    [HideInInspector]
    public RandomVisitorEndDialogWrapper randomVisitorEndDialogWrapper;
    [HideInInspector]
    public RandomVisitorDiseaseDialogWrapper randomVisitorDiseaseDialogWrapper;
    [HideInInspector]
    public SpecialMedicineDataWrapper specialMedicineDataWrapper;
    //이거는 세이브 로드 확인해볼라고
    [HideInInspector]
    public SymptomDialog symptomDialog;
    [HideInInspector]
    public SpecialVisitorConditionWrapper specialVisitorConditionWrapper;
    [HideInInspector]
    public DocumentConditionWrapper documentConditionWrapper;
    [HideInInspector]
    public UILanguagePack languagePack;

    public int nowSaveDataIndex;

    //세이브데이터 불러오기 및 딕셔너리 읽기.
    JsonManager jsonManager;

    //public float nowTime;
    //public int nowDay;


    void Awake()
    {
        if (singleTon == null)
        {
            singleTon = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {

        jsonManager = new JsonManager();
        sceneManager = SceneManager.inst;
        tabletManager = TabletManager.inst;
        medicineDataWrapper = jsonManager.ResourceDataLoad<MedicineDataWrapper>("MedicineDataWrapper");
        //DebugDataJson();

        
        languagePack = jsonManager.ResourceDataLoad<UILanguagePack>("LanguagePack");
        symptomDialog = jsonManager.ResourceDataLoad<SymptomDialog>("SymptomDialog");
        regionPropertyWrapper = jsonManager.ResourceDataLoad<RegionPropertyWrapper>("RegionPropertyWrapper");
        storeToolDataWrapper = jsonManager.ResourceDataLoad<StoreToolDataWrapper>("StoreToolDataWrapper");
        randomDialogDataWrapper = jsonManager.ResourceDataLoad<StartDialogClassWrapper>("RandomDialogDataWrapper");
        specialMedicineDataWrapper = jsonManager.ResourceDataLoad<SpecialMedicineDataWrapper>("SpecialMedicineDataWrapper");
        documentConditionWrapper = jsonManager.ResourceDataLoad<DocumentConditionWrapper>("DocumentConditionWrapper");
        for(int i = 0; i < documentConditionWrapper.documentConditionList.Count; i++)
        {
            documentConditionWrapper.documentConditionList[i].ConditionStringParse();
           
        }
        //languagePack = jsonManager.ResourceDataLoad<UILanguagePack>("LanguagePack");

        randomVisitorEndDialogWrapper = jsonManager.ResourceDataLoad<RandomVisitorEndDialogWrapper>("RandomVisitorEndDialogWrapper");
        randomVisitorDiseaseDialogWrapper = jsonManager.ResourceDataLoad<RandomVisitorDiseaseDialogWrapper>("RandomVisitorDiseaseDialogWrapper");
        specialVisitorConditionWrapper = jsonManager.ResourceDataLoad<SpecialVisitorConditionWrapper>("SpecialVisitorConditionWrapper");
        conversationDialogBundleWrapper = jsonManager.ResourceDataLoad<ConversationDialogBundleWrapper>("ConversationDialogBundleWrapper");


        for (int i = 0; i < storeToolDataWrapper.storeToolDataList.Count; i++)
        {
            storeToolDataWrapper.storeToolDataList[i].SetIndex(i);
        }
        for (int i = 0; i < medicineDataWrapper.medicineDataList.Count; i++)
        {
            medicineDataWrapper.medicineDataList[i].SetIndex(i);
            medicineDataWrapper.medicineDataList[i].ParseSymptom();
        }

        for (int i = 0; i < specialMedicineDataWrapper.specialMedicineDataList.Count; i++)
        {
            specialMedicineDataWrapper.specialMedicineDataList[i].SetIndex(i);
        }

        saveDataTimeWrapper = jsonManager.LoadSaveDataTime();

        if(sceneManager.nowSaveIndex == -1)
        {
            saveData = new SaveDataClass();
        }
        else
        {
            saveData = jsonManager.LoadSaveData(sceneManager.nowSaveIndex);
        }
        

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            randomDialogDataWrapper = jsonManager.ResourceDataLoad<StartDialogClassWrapper>("RandomDialogDataWrapper");
        }
    }

    public ConversationDialogBundle LoadBundle(string bundleName)
    {
        ConversationDialogBundle bundle;
        bundle = jsonManager.ResourceDataLoad<ConversationDialogBundle>("StoryBundle/" + bundleName);
        return bundle;
    }

    public SpecialVisitorDialogBundle LoadVisitorBundle(string bundleName)
    {
        SpecialVisitorDialogBundle bundle;
        bundle = jsonManager.ResourceDataLoad<SpecialVisitorDialogBundle>("SpecialVisitorBundle/" + bundleName);
        return bundle;
    }

    public DocumentBundle LoadDocumentBundle(string bundleName)
    {
        DocumentBundle bundle;
        bundle = jsonManager.ResourceDataLoad<DocumentBundle>("DocumentBundle/" + bundleName);
        return bundle;
    }

    //아마 모든 매니저에서 참조할것.
    public void SaveJson(int index,SaveTime saveTime)
    {
        saveDataTimeWrapper.saveDataTimeList[index].day = saveData.nowDay;
        saveDataTimeWrapper.saveDataTimeList[index].saveTime = saveTime;
        jsonManager.SaveJson<SaveDataTimeWrapper>(saveDataTimeWrapper, "SaveDataTimeWrapper");

        jsonManager.SaveJson(saveData,index);
    }

    //public void AutoSave(string nextScene)
    //{
    //    saveData.nextLoadSceneName = nextScene;

    //}

    public void ForceSaveButtonActive(string nextScene,SaveTime saveTime)
    {
        saveData.nextLoadSceneName = nextScene;
        saveData.nowSaveTime = saveTime;
        jsonManager.SaveJson(saveData, 0);
        saveDataTimeWrapper.saveDataTimeList[0].day = saveData.nowDay;
        saveDataTimeWrapper.saveDataTimeList[0].saveTime = saveTime;
        jsonManager.SaveJson<SaveDataTimeWrapper>(saveDataTimeWrapper, "SaveDataTimeWrapper");
        tabletManager.TabletOpenButtonActive(true);
        tabletManager.ForceSaveButtonActive(true,saveTime);
    }


    public void LoadJson(int index)
    {
        saveData = jsonManager.LoadSaveData(index);
        if(saveData.nowSaveTime == SaveTime.ExploreStart)
        {
            sceneManager.lastSceneName = "RoomCounterScene";
        }
        else if(saveData.nowSaveTime == SaveTime.DayStart)
        {
            sceneManager.lastSceneName = "StoryScene";
        }
        sceneManager.SaveDataLoadScene();
    }

    //아직 영표형한테서 안나왔으니까 디버깅용으로 파일을 만들어야함.
    void DebugDataJson()
    {
        saveData = new SaveDataClass();
        for (int i = 0; i < 10; i++)
        {
            saveData.unlockedRegionIndex.Add(i);
        }
        symptomDialog = new SymptomDialog();

        /*
        cookedMedicineDataWrapper = new CookedMedicineDataWrapper();
        for(int i = 0; i < medicineDataWrapper.medicineDataList.Count; i++)
        {
            for (int j = i; j < medicineDataWrapper.medicineDataList.Count; j++)
            {
                for (int k = j; k < medicineDataWrapper.medicineDataList.Count; k++)
                {
                    CookedMedicineData cookedMedicine = new CookedMedicineData();
                    cookedMedicine.name = i.ToString() +j.ToString() +k.ToString();
                    cookedMedicine.medicineArray[0] = i;
                    cookedMedicine.medicineArray[1] = j;
                    cookedMedicine.medicineArray[2] = k;
                    cookedMedicineDataWrapper.cookedMedicineDataList.Add(cookedMedicine);
                }
            }

        }*/

        regionPropertyWrapper = new RegionPropertyWrapper();
        storeToolDataWrapper = new StoreToolDataWrapper();
        conversationDialogBundleWrapper = new ConversationDialogBundleWrapper();
        randomDialogDataWrapper = new StartDialogClassWrapper();
        conversationDialogBundle = new ConversationDialogBundle();
        documentConditionWrapper = new DocumentConditionWrapper();

        SpecialVisitorDialogBundle bundle = new SpecialVisitorDialogBundle();
        SpecialMedicineDataWrapper wrapper = new SpecialMedicineDataWrapper();
        specialVisitorConditionWrapper = new SpecialVisitorConditionWrapper();
        ConversationDialogBundle storyBundle = new ConversationDialogBundle();
        //jsonManager.SaveJson<ConversationDialogBundle>(storyBundle, "testBundle");
        saveDataTimeWrapper = new SaveDataTimeWrapper();
        
        languagePack = new UILanguagePack();
        jsonManager.SaveJson<SaveDataTimeWrapper>(saveDataTimeWrapper, "SaveDataTimeWrapper");
        //jsonManager.SaveJson<UILanguagePack>(languagePack, "languagePack");
        //jsonManager.SaveJson<SpecialVisitorDialogBundle>(bundle, "specialVisitor");
        //jsonManager.SaveJson<MedicineDataWrapper>(medicineDataWrapper, "MedicineDataWrapper");
        //jsonManager.SaveJson<SymptomDialog>(symptomDialog, "SymptomDialog");
        //jsonManager.SaveJson<RegionPropertyWrapper>(regionPropertyWrapper, "RegionPropertyWrapper");
        //jsonManager.SaveJson<StoreToolDataWrapper>(storeToolDataWrapper, "StoreToolDataWrapper");
        //jsonManager.SaveJson<ConversationDialogBundle>(conversationDialogBundle, conversationDialogBundle.bundleName);
        //jsonManager.SaveJson<StartDialogClassWrapper>(randomDialogDataWrapper, "RandomDialogDataWrapper");
        //jsonManager.SaveJson<SpecialMedicineDataWrapper>(wrapper, "SpecialMedicineDataWrapper");
        jsonManager.SaveJson<SpecialVisitorConditionWrapper>(specialVisitorConditionWrapper, "SpecialVisitorConditionWrapper");
        //jsonManager.SaveJson<DocumentConditionWrapper>(documentConditionWrapper, "DocumentConditionWrapper");
        //DocumentBundle doc = new DocumentBundle();
        //jsonManager.SaveJson<DocumentBundle>(doc, "testDocument");

        for (int i = 0; i < 4; i++)
        {
            jsonManager.SaveJson(saveData, i);
        }


        medicineDataWrapper = jsonManager.ResourceDataLoad<MedicineDataWrapper>("MedicineDataWrapper");
        symptomDialog = jsonManager.ResourceDataLoad<SymptomDialog>("SymptomDialog");
        //saveData = jsonManager.LoadSaveData(0);

        /*
        for (int i = 0; i < saveData.owningMedicineList.Count; i++)
        {
            if (saveData.owningMedicineDictionary.ContainsKey(saveData.owningMedicineList[i].medicineIndex))
            {
                saveData.owningMedicineDictionary[saveData.owningMedicineList[i].medicineIndex] = saveData.owningMedicineList[i].medicineQuantity;
                    //saveData.owningMedicineDictionary[saveData.owningMedicineList[i]] + 1;
            }
            else
            {
                saveData.owningMedicineDictionary.Add(saveData.owningMedicineList[i].medicineIndex, saveData.owningMedicineList[i].medicineQuantity);
            }
        }*/
    }


    //이거 버튼누를때마다 불러옴.
    //이거 단위 초다.
    public void TimeChange(float plusTime)
    {
        saveData.nowTime += 2*plusTime;

    }

    public void NextDay()
    {
        if (saveData.nowTime >= 3600 * 24)
        {
            saveData.nowTime =0;
            saveData.nowDay++;
            TabletManager.inst.SetTodayBill();
        }
    }
}
