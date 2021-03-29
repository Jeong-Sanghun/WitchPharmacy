using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//카운터씬 매니저
//여기서 증상까지 만들어서 RoomManager로 넘겨줌
public class CounterManager : MonoBehaviour //SH
{
    GameManager gameManager;
    SceneManager sceneManager;
    SaveDataClass saveData;
    SymptomDialog symptomDialog;
    List<int> ownedMedicineIndexList;
    List<MedicineClass> ownedMedicineList;
    Dictionary<int,int> owningMedicineDictionary;
    //List<MedicineClass> owningMedicineList;
    List<MedicineClass> medicineDataList;

    [SerializeField]
    List<RandomVisitorClass> randomVisitorList;
    public Text visitorText;

    void Start()
    {
        gameManager = GameManager.singleTon;
        sceneManager = gameManager.SceneManagerScirpt;
        symptomDialog = gameManager.symptomDialog;
        saveData = gameManager.saveData;
        medicineDataList = gameManager.medicineDataWrapper.medicineDataList;
        
        ownedMedicineIndexList = saveData.ownedMedicineList;
        ownedMedicineList = new List<MedicineClass>();
        for(int i = 0; i < ownedMedicineIndexList.Count; i++)
        {
            ownedMedicineList.Add(medicineDataList[ownedMedicineIndexList[i]]);
        }
        owningMedicineDictionary = saveData.owningMedicineDictionary;
        //owningMedicineList = new List<MedicineClass>();
        /*
        for (int i = 0; i < owningMedicineIndexList.Count; i++)
        {
            owningMedicineList.Add(medicineDictionary.medicineList[owningMedicineIndexList[i]]);
        }*/
        //여기까진 게임메니저랑 세이브데이터에서 데이터 받아오는거 및 초기화.

        randomVisitorList = new List<RandomVisitorClass>();
        for (int i = 0; i< 100; i++)
        {
            randomVisitorList.Add(new RandomVisitorClass(symptomDialog,ownedMedicineList));
            //Debug.Log(randomVisitorList[i].answerMedicineList.Count +"개 짜리");

        }
        // LoadTextOneByOne(string inputTextString, Text inputTextUI, float eachTime = 0.05f)
    }



    int index = 0;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            randomVisitorList.Add(new RandomVisitorClass(symptomDialog, ownedMedicineList));
            StartCoroutine(sceneManager.LoadTextOneByOne(randomVisitorList[index].fullDialog, visitorText));
            index++;
        }
    }
}
