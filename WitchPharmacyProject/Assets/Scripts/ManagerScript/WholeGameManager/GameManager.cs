using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour //SH
{
    //singleTon 모든 매니저 스크립트에서 참조
    public static GameManager singleTon;
    [HideInInspector]
    public SceneManager SceneManagerScirpt;
    //모든 매니저 스크립트에서 참조
    [HideInInspector]
    public SaveDataClass saveData;
    [HideInInspector]
    public MedicineDataWrapper medicineDataWrapper;
    [HideInInspector]
    public RegionPropertyWrapper regionPropertyWrapper;
    [HideInInspector]
    public StoreToolDataWrapper storeToolDataWrapper;

    public ConversationDialogBundleWrapper conversationDialogBundleWrapper;
    public ConversationDialogBundle conversationDialogBundle;
    [HideInInspector]
    public StartDialogClassWrapper randomDialogDataWrapper;
    //public CookedMedicineDataWrapper cookedMedicineDataWrapper;
    [HideInInspector]
    public RandomVisitorEndDialogWrapper randomVisitorEndDialogWrapper;
    [HideInInspector]
    public RandomVisitorDiseaseDialogWrapper randomVisitorDiseaseDialogWrapper;

    //이거는 세이브 로드 확인해볼라고
    [HideInInspector]
    public SymptomDialog symptomDialog;

    //세이브데이터 불러오기 및 딕셔너리 읽기.
    JsonManager jsonManager;

    public float nowTime;
    public int nowDay;


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
        SceneManagerScirpt = SceneManager.inst;

        DebugDataJson();
        medicineDataWrapper = jsonManager.ResourceDataLoad<MedicineDataWrapper>("MedicineDataWrapper");
        symptomDialog = jsonManager.ResourceDataLoad<SymptomDialog>("SymptomDialog");
        regionPropertyWrapper = jsonManager.ResourceDataLoad<RegionPropertyWrapper>("RegionPropertyWrapper");
        storeToolDataWrapper = jsonManager.ResourceDataLoad<StoreToolDataWrapper>("StoreToolDataWrapper");
        randomDialogDataWrapper = jsonManager.ResourceDataLoad<StartDialogClassWrapper>("RandomDialogDataWrapper");

        randomVisitorEndDialogWrapper = jsonManager.ResourceDataLoad<RandomVisitorEndDialogWrapper>("RandomVisitorEndDialogWrapper");
        randomVisitorDiseaseDialogWrapper = jsonManager.ResourceDataLoad<RandomVisitorDiseaseDialogWrapper>("RandomVisitorDiseaseDialogWrapper");
        


        for (int i = 0; i < storeToolDataWrapper.storeToolDataList.Count; i++)
        {
            storeToolDataWrapper.storeToolDataList[i].SetIndex(i);
        }
        for (int i = 0; i < medicineDataWrapper.medicineDataList.Count; i++)
        {
            medicineDataWrapper.medicineDataList[i].SetIndex(i);
        }
        conversationDialogBundleWrapper = jsonManager.ResourceDataLoad<ConversationDialogBundleWrapper>("ConversationDialogBundleWrapper");
        saveData = jsonManager.LoadSaveData();
        nowTime = saveData.nowTime;
        nowDay = saveData.nowDay;
    }

    public ConversationDialogBundle LoadBundle(string bundleName)
    {
        ConversationDialogBundle bundle;
        bundle = jsonManager.ResourceDataLoad<ConversationDialogBundle>("StoryBundle/" + bundleName);
        return bundle;
    }

    //아마 모든 매니저에서 참조할것.
    public void SaveJson()
    {
        saveData.nowTime = nowTime;
        saveData.nowDay = nowDay;
        jsonManager.SaveJson(saveData);
    }

    //아직 영표형한테서 안나왔으니까 디버깅용으로 파일을 만들어야함.
    void DebugDataJson()
    {

        for (int i = 0; i < medicineDataWrapper.medicineDataList.Count; i++)
        {
            saveData.ownedMedicineList.Add(i);
        }
        for (int i = 0; i < medicineDataWrapper.medicineDataList.Count; i++)
        {
            int quant = Random.Range(1, 13);
            OwningMedicineClass med = new OwningMedicineClass(i, quant);
            saveData.owningMedicineList.Add(med);
            //i += Random.Range(0, 4);
        }
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

        jsonManager.SaveJson<MedicineDataWrapper>(medicineDataWrapper, "MedicineDataWrapper");
        jsonManager.SaveJson<SymptomDialog>(symptomDialog, "SymptomDialog");
        jsonManager.SaveJson<RegionPropertyWrapper>(regionPropertyWrapper, "RegionPropertyWrapper");
        jsonManager.SaveJson<StoreToolDataWrapper>(storeToolDataWrapper, "StoreToolDataWrapper");
        jsonManager.SaveJson<ConversationDialogBundle>(conversationDialogBundle, conversationDialogBundle.bundleName);
        jsonManager.SaveJson<StartDialogClassWrapper>(randomDialogDataWrapper, "RandomDialogDataWrapper");
        //jsonManager.SaveJson(saveData);
        
        //medicineDataWrapper = jsonManager.ResourceDataLoad<MedicineDataWrapper>("MedicineDataWrapper");
        //symptomDialog = jsonManager.ResourceDataLoad<SymptomDialog>("SymptomDialog");
        //saveData = jsonManager.LoadSaveData();

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
        nowTime += 4*plusTime;

    }

    public void NextDay()
    {
        if (nowTime >= 3600 * 24)
        {
            nowTime =0;
            nowDay++;
        }
    }
}
