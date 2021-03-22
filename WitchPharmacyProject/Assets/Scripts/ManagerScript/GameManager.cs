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
    public MedicineDictionary medicineDictionary;
    
    //이거는 세이브 로드 확인해볼라고
    public SymptomDialog symptomDialog;

    //세이브데이터 불러오기 및 딕셔너리 읽기.
    JsonManager jsonManager;   
    

    void Start()
    {
        if(singleTon == null)
        {
            singleTon = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        //중복방지 싱글톤

        jsonManager = new JsonManager();
        DebugDataJson();
    }

    //아직 영표형한테서 안나왔으니까 디버깅용으로 파일을 만들어야함.
    void DebugDataJson()
    {
        List<int> random = new List<int>();
        for(int i = 0; i < 200; i++)
        {
            random.Add(Random.Range(0, 2));
        }
        medicineDictionary = new MedicineDictionary(random);
        saveData = new SaveDataClass();
        for(int i = 0; i < medicineDictionary.medicineList.Count; i++)
        {
            saveData.ownedMedicineList.Add(i);
            saveData.owningMedicineList.Add(i);
        }
        symptomDialog = new SymptomDialog();
        jsonManager.SaveJson<MedicineDictionary>(medicineDictionary, "MedicineDictionary");
        jsonManager.SaveJson<SymptomDialog>(symptomDialog, "SymptomDialog");
        jsonManager.SaveJson(saveData);
    }
}
