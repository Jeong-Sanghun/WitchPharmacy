using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class GameManager : MonoBehaviour //SH
{
    //singleTon 모든 매니저 스크립트에서 참조
    public static GameManager singleton;
    [HideInInspector]
    public SceneManager sceneManager;
    TabletManager tabletManager;
    //모든 매니저 스크립트에서 참조
    
    public SaveDataClass saveData;
    public SaveDataTimeWrapper saveDataTimeWrapper;
    public MedicineDataWrapper medicineDataWrapper;

    [HideInInspector]
    //public ConversationDialogBundle conversationDialogBundle;
    //[HideInInspector]
    public StartDialogClassWrapper randomDialogDataWrapper;
    //public CookedMedicineDataWrapper cookedMedicineDataWrapper;
    [HideInInspector]
    public RandomVisitorEndDialogWrapper randomVisitorEndDialogWrapper;
    //[HideInInspector]
    //public RandomVisitorDiseaseDialogWrapper randomVisitorDiseaseDialogWrapper;
    [HideInInspector]
    public RandomVisitorDiseaseBundle randomVisitorDiseaseBundle;
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

    [HideInInspector]
    public OtherToolResearchDataWrapper otherToolResearchDataWrapper;
    [HideInInspector]
    public MeasureToolResearchDataWrapper measureToolResearchDataWrapper;
    [HideInInspector]
    public MedicineResearchDataWrapper medicineResearchDataWrapper;
    [HideInInspector]
    public BookResearchDataWrapper bookResearchDataWrapper;
    [HideInInspector]
    public TreeterConditionWrapper treeterConditionWrapper;
    


    public int nowSaveDataIndex;

    //세이브데이터 불러오기 및 딕셔너리 읽기.
    public JsonManager jsonManager;

    //public float nowTime;
    //public int nowDay;


    void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
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
        DebugDataJson();

        
        languagePack = jsonManager.ResourceDataLoad<UILanguagePack>("LanguagePack");
        symptomDialog = jsonManager.ResourceDataLoad<SymptomDialog>("SymptomDialog");
        randomDialogDataWrapper = jsonManager.ResourceDataLoad<StartDialogClassWrapper>("RandomDialogDataWrapper");
        specialMedicineDataWrapper = jsonManager.ResourceDataLoad<SpecialMedicineDataWrapper>("SpecialMedicineDataWrapper");
        documentConditionWrapper = jsonManager.ResourceDataLoad<DocumentConditionWrapper>("DocumentConditionWrapper");
        randomVisitorDiseaseBundle = new RandomVisitorDiseaseBundle();
        randomVisitorDiseaseBundle.LoadWrapper(jsonManager);
        //for(int i = 0; i < documentConditionWrapper.documentConditionList.Count; i++)
        //{
        //    documentConditionWrapper.documentConditionList[i].ConditionStringParse();
           
        //}

        treeterConditionWrapper = jsonManager.ResourceDataLoad<TreeterConditionWrapper>("TreeterConditionWrapper");
        //languagePack = jsonManager.ResourceDataLoad<UILanguagePack>("LanguagePack");

        randomVisitorEndDialogWrapper = jsonManager.ResourceDataLoad<RandomVisitorEndDialogWrapper>("RandomVisitorEndDialogWrapper");
        //randomVisitorDiseaseDialogWrapper = jsonManager.ResourceDataLoad<RandomVisitorDiseaseDialogWrapper>("RandomVisitorDiseaseDialogWrapper");
        specialVisitorConditionWrapper = jsonManager.ResourceDataLoad<SpecialVisitorConditionWrapper>("SpecialVisitorConditionWrapper");

        measureToolResearchDataWrapper = jsonManager.ResourceDataLoad<MeasureToolResearchDataWrapper>("ResearchData/MeasureToolResearchDataWrapper");
        medicineResearchDataWrapper = jsonManager.ResourceDataLoad<MedicineResearchDataWrapper>("ResearchData/MedicineResearchDataWrapper");
        otherToolResearchDataWrapper = jsonManager.ResourceDataLoad<OtherToolResearchDataWrapper>("ResearchData/OtherToolResearchDataWrapper");
        bookResearchDataWrapper = jsonManager.ResourceDataLoad<BookResearchDataWrapper>("ResearchData/BookResearchDataWrapper");


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
            saveData.AddMedicineBySymptom(medicineDataWrapper, Symptom.water, Symptom.fire);
        }
        else
        {
            saveData = jsonManager.LoadSaveData(sceneManager.nowSaveIndex);
            if (saveData == null)
            {
                saveData = new SaveDataClass();
                saveData.AddMedicineBySymptom(medicineDataWrapper, Symptom.water, Symptom.fire);
            }
        }
        

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            randomDialogDataWrapper = jsonManager.ResourceDataLoad<StartDialogClassWrapper>("RandomDialogDataWrapper");
        }
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

    public OddVisitorDialogBundle LoadOddBundle(string bundleName)
    {
        OddVisitorDialogBundle bundle;
        bundle = jsonManager.ResourceDataLoad<OddVisitorDialogBundle>("OddVisitorDialogBundle/" + bundleName);
        return bundle;
    }

    public SymptomBookBundle LoadSymptomBookBundle(string bundleName)
    {
        SymptomBookBundle bundle;
        bundle = jsonManager.ResourceDataLoad<SymptomBookBundle>("SymptomBookBundle/" + bundleName);
        bundle.symptom = (Symptom)Enum.Parse(typeof(Symptom), bundle.symptomString);
        return bundle;
    }

    //아마 모든 매니저에서 참조할것.
    public void SaveJson(int index)
    {
        saveDataTimeWrapper.saveDataTimeList[index].day = saveData.nowDay;
        saveDataTimeWrapper.saveDataTimeList[index].sceneIndex = saveData.nowSceneIndex;
        jsonManager.SaveJson<SaveDataTimeWrapper>(saveDataTimeWrapper, "SaveDataTimeWrapper");

        jsonManager.SaveJson(saveData,index);
    }

    //public void AutoSave(string nextScene)
    //{
    //    saveData.nextLoadSceneName = nextScene;

    //}

    public void ForceSaveButtonActive()
    {
        if(sceneManager.sceneWrapper.sceneArray[saveData.nowSceneIndex].day != null)
        {
            int day =int.Parse(sceneManager.sceneWrapper.sceneArray[saveData.nowSceneIndex].day);
            
            saveData.nowDay = day;
        }

        jsonManager.SaveJson(saveData, 0);
        saveDataTimeWrapper.saveDataTimeList[0].day = saveData.nowDay;
        saveDataTimeWrapper.saveDataTimeList[0].sceneIndex = saveData.nowSceneIndex;
        jsonManager.SaveJson<SaveDataTimeWrapper>(saveDataTimeWrapper, "SaveDataTimeWrapper");
        tabletManager.TabletOpenButtonActive(true,false);
        tabletManager.ForceSaveButtonActive(true);
    }



    public void LoadJson(int index)
    {
        saveData = jsonManager.LoadSaveData(index);
        if(saveData == null)
        {
            saveData = new SaveDataClass();
            saveData.AddMedicineBySymptom(medicineDataWrapper, Symptom.water, Symptom.fire);
        }
        sceneManager.SaveDataLoadScene();
    }

    //아직 영표형한테서 안나왔으니까 디버깅용으로 파일을 만들어야함.
    void DebugDataJson()
    {
        //saveData = new SaveDataClass();
        //symptomDialog = new SymptomDialog();

        ///*
        //cookedMedicineDataWrapper = new CookedMedicineDataWrapper();
        //for(int i = 0; i < medicineDataWrapper.medicineDataList.Count; i++)
        //{
        //    for (int j = i; j < medicineDataWrapper.medicineDataList.Count; j++)
        //    {
        //        for (int k = j; k < medicineDataWrapper.medicineDataList.Count; k++)
        //        {
        //            CookedMedicineData cookedMedicine = new CookedMedicineData();
        //            cookedMedicine.name = i.ToString() +j.ToString() +k.ToString();
        //            cookedMedicine.medicineArray[0] = i;
        //            cookedMedicine.medicineArray[1] = j;
        //            cookedMedicine.medicineArray[2] = k;
        //            cookedMedicineDataWrapper.cookedMedicineDataList.Add(cookedMedicine);
        //        }
        //    }

        //}*/
        //conversationDialogBundleWrapper = new ConversationDialogBundleWrapper();
        //randomDialogDataWrapper = new StartDialogClassWrapper();
        //conversationDialogBundle = new ConversationDialogBundle();
        //documentConditionWrapper = new DocumentConditionWrapper();

        //SpecialVisitorDialogBundle bundle = new SpecialVisitorDialogBundle();
        //SpecialMedicineDataWrapper wrapper = new SpecialMedicineDataWrapper();
        //specialVisitorConditionWrapper = new SpecialVisitorConditionWrapper();
        //ConversationDialogBundle storyBundle = new ConversationDialogBundle();
        ////jsonManager.SaveJson<ConversationDialogBundle>(storyBundle, "testBundle");

        //OddVisitorDialogBundle oddBundle = new OddVisitorDialogBundle();
        //QuestDocument doc = new QuestDocument();
        //SymptomBookBundle symptomBookBundle = new SymptomBookBundle();
        //MeasureToolDataWrapper measureToolDataWrapper = new MeasureToolDataWrapper();
        //jsonManager.SaveJson<MeasureToolDataWrapper>(measureToolDataWrapper, "MeasureToolDataWrapper");

        //languagePack = new UILanguagePack();
        //jsonManager.SaveJson<QuestDocument>(doc, "Lily");
        //jsonManager.SaveJson<SymptomBookBundle>(symptomBookBundle, "water+");
        ////jsonManager.SaveJson<OddVisitorDialogBundle>(oddBundle, "meltfire");
        //MeasureToolResearchDataWrapper measureToolWrapper = new MeasureToolResearchDataWrapper();
        //jsonManager.SaveJson<MeasureToolResearchDataWrapper>(measureToolWrapper, "MeasureToolResearchDataWrapper");
        //MeasureToolExplain explain = new MeasureToolExplain();
        //jsonManager.SaveJson<MeasureToolExplain>(explain, "Fire");
        //MedicineResearchDataWrapper medicineResearchWrapper = new MedicineResearchDataWrapper();
        //jsonManager.SaveJson<MedicineResearchDataWrapper>(medicineResearchWrapper, "MedicineResearchDataWrapper");
        //OtherToolResearchDataWrapper otherToolResearchWrapper = new OtherToolResearchDataWrapper();
        //jsonManager.SaveJson<OtherToolResearchDataWrapper>(otherToolResearchWrapper, "OtherToolResearchDataWrapper");
        //OtherToolDataWrapper otherToolWrapper = new OtherToolDataWrapper();
        ////jsonManager.SaveJson<OtherToolDataWrapper>(otherToolWrapper, "OtherToolDataWrapper");
        //RegionDataWrapper region = new RegionDataWrapper();
        //jsonManager.SaveJson<RegionDataWrapper>(region, "RegionDataWrapper");
        ////RandomVisitorDiseaseWrapper diseaseWrapper = new RandomVisitorDiseaseWrapper();
        //jsonManager.SaveJson<RandomVisitorDiseaseWrapper>(diseaseWrapper, "water");
        //treeterConditionWrapper = new TreeterConditionWrapper();
        //jsonManager.SaveJson<TreeterConditionWrapper>(treeterConditionWrapper, "TreeterConditionWrapper");
        //TreeterData treeterData = new TreeterData();
        //jsonManager.SaveJson<TreeterData>(treeterData, "TreeterData");
        //bookResearchDataWrapper = new BookResearchDataWrapper();
        //jsonManager.SaveJson<BookResearchDataWrapper>(bookResearchDataWrapper, "BookResearchDataWrapper");

        //jsonManager.SaveJson<SaveDataTimeWrapper>(saveDataTimeWrapper, "SaveDataTimeWrapper");
        //jsonManager.SaveJson<UILanguagePack>(languagePack, "languagePack");
        //jsonManager.SaveJson<SpecialVisitorDialogBundle>(bundle, "specialVisitor");
        //jsonManager.SaveJson<MedicineDataWrapper>(medicineDataWrapper, "MedicineDataWrapper");
        //jsonManager.SaveJson<SymptomDialog>(symptomDialog, "SymptomDialog");
        //jsonManager.SaveJson<RegionPropertyWrapper>(regionPropertyWrapper, "RegionPropertyWrapper");
        //jsonManager.SaveJson<StoreToolDataWrapper>(storeToolDataWrapper, "StoreToolDataWrapper");
        //jsonManager.SaveJson<ConversationDialogBundle>(conversationDialogBundle, conversationDialogBundle.bundleName);
        //jsonManager.SaveJson<StartDialogClassWrapper>(randomDialogDataWrapper, "RandomDialogDataWrapper");
        //jsonManager.SaveJson<SpecialMedicineDataWrapper>(wrapper, "SpecialMedicineDataWrapper");
        //jsonManager.SaveJson<SpecialVisitorConditionWrapper>(specialVisitorConditionWrapper, "SpecialVisitorConditionWrapper");
        //jsonManager.SaveJson<DocumentConditionWrapper>(documentConditionWrapper, "DocumentConditionWrapper");
        //DocumentBundle doc = new DocumentBundle();
        //jsonManager.SaveJson<DocumentBundle>(doc, "testDocument");
        //RegionDataWrapper regionDataWrapper = new RegionDataWrapper();
        //jsonManager.SaveJson<RegionDataWrapper>(regionDataWrapper, "Narin");

        //BossDataWrapper bossDataWrapper = new BossDataWrapper();
        //bossDataWrapper.bossDataList.Add(new BossData());
        //bossDataWrapper.bossDataList.Add(new BossData());
        //bossDataWrapper.bossDataList.Add(new BossData());
        //jsonManager.SaveJson<BossDataWrapper>(bossDataWrapper, "BossDataWrapper");

        MainCariDialogCondition condition = new MainCariDialogCondition();
        condition.appearingDayArray = new int[1];
        condition.appearingDayArray[0] = 0;
        jsonManager.SaveJson<MainCariDialogCondition>(condition, "MainCariDialogCondition");


        //for (int i = 0; i < 4; i++)
        //{
        //    jsonManager.SaveJson(saveData, i);
        //}
        //saveDataTimeWrapper = new SaveDataTimeWrapper();
        //jsonManager.SaveJson<SaveDataTimeWrapper>(saveDataTimeWrapper, "SaveDataTimeWrapper");


        //medicineDataWrapper = jsonManager.ResourceDataLoad<MedicineDataWrapper>("MedicineDataWrapper");
        //symptomDialog = jsonManager.ResourceDataLoad<SymptomDialog>("SymptomDialog");
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
        //if (saveData.nowTime >= 3600 * 24)
        //{

        //}
        saveData.nowTime = 0;
        saveData.nowDay++;
        Debug.Log(saveData.nowDay);
        TabletManager.inst.OnNextDay();
    }
}
