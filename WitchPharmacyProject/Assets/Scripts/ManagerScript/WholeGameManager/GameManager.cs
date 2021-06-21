using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour //SH
{
    //singleTon 모든 매니저 스크립트에서 참조
    public static GameManager singleTon;
    public SceneManager SceneManagerScirpt;
    //모든 매니저 스크립트에서 참조
    public SaveDataClass saveData;
    public MedicineDataWrapper medicineDataWrapper;
    //public CookedMedicineDataWrapper cookedMedicineDataWrapper;
    
    //이거는 세이브 로드 확인해볼라고
    public SymptomDialog symptomDialog;

    //세이브데이터 불러오기 및 딕셔너리 읽기.
    JsonManager jsonManager;



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
        DebugDataJson();
    }

    //아마 모든 매니저에서 참조할것.
    public void SaveJson()
    {

        jsonManager.SaveJson(saveData);
    }

    //아직 영표형한테서 안나왔으니까 디버깅용으로 파일을 만들어야함.
    void DebugDataJson()
    {
        
        medicineDataWrapper = new MedicineDataWrapper();
        saveData = new SaveDataClass();
        for(int i = 0; i < medicineDataWrapper.medicineDataList.Count; i++)
        {
            saveData.ownedMedicineList.Add(i);
        }
        for (int i = 0; i < medicineDataWrapper.medicineDataList.Count; i++)
        {
            int quant = Random.Range(1, 50);
            OwningMedicineClass med = new OwningMedicineClass(i, quant);
            saveData.owningMedicineList.Add(med);
            //i += Random.Range(0, 4);
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

        jsonManager.SaveJson<MedicineDataWrapper>(medicineDataWrapper, "MedicineDataWrapper");
        jsonManager.SaveJson<SymptomDialog>(symptomDialog, "SymptomDialog");
        jsonManager.SaveJson(saveData);
        
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
}
