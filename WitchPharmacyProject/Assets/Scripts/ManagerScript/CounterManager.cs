﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
//카운터씬 매니저
//여기서 증상까지 만들어서 RoomManager로 넘겨줌
public class CounterManager : MonoBehaviour //SH
{
    GameManager gameManager;
    SceneManager sceneManager;
    [SerializeField]
    RoomManager roomManager;
    SaveDataClass saveData;
    SymptomDialog symptomDialog;
    List<int> ownedMedicineIndexList;
    List<MedicineClass> ownedMedicineList;
    //Dictionary<int,int> owningMedicineDictionary;
    //List<MedicineClass> owningMedicineList;
    List<MedicineClass> medicineDataList;
   

    [SerializeField]
    List<RandomVisitorClass> randomVisitorList;
    RandomVisitorClass nowVisitor;
    public Text visitorText;

    [SerializeField]
    GameObject visitorObject;

    [SerializeField]
    GameObject[] visitorBodyParts;

    Vector3 visitorAppearPos;
    Vector3 visitorDisappearPos;

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
        //owningMedicineDictionary = saveData.owningMedicineDictionary;

        for(int i = 0; i < visitorBodyParts.Length; i++)
        {
            visitorBodyParts[i].SetActive(false);
        }

        visitorAppearPos = new Vector3(-7.06f, 1.22f, -1);
        visitorDisappearPos = new Vector3(-7.06f, -12, -1);
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

            //Debug.Log(randomVisitorList[i].answerMedicineList.Count +"개 짜리");

        }
        // LoadTextOneByOne(string inputTextString, Text inputTextUI, float eachTime = 0.05f)

        SpawnRandomVisitor();
    }

    int index = 0;
    void SpawnRandomVisitor()
    {
        nowVisitor = new RandomVisitorClass(symptomDialog, ownedMedicineList);

        for(int i = 0; i < visitorBodyParts.Length; i++)
        {
            if(i == nowVisitor.bodyIndex)
            {
                visitorBodyParts[i].SetActive(true);
            }
            else
            {
                visitorBodyParts[i].SetActive(false);
            }
        }

        randomVisitorList.Add(nowVisitor);
        roomManager.VisitorVisits(nowVisitor);
        StartCoroutine(VisitorAppearCoroutine());
        index++;
    }

    public void OnMedicineDelivery(CookedMedicine medicine)
    {
        int[] medicineIndexArray = medicine.medicineArray;
        int[] medicineSymptomArray = new int[6];
        int[] visitorSymptomArray = nowVisitor.symptomAmountArray;
        bool goodMedicine = true;
        List<Symptom> badSymptomList = new List<Symptom>();
        for(int i =0; i < 6; i++)
        {
            medicineSymptomArray[i] = 0;
        }
        for(int i = 0; i < 3; i++)
        {
            MedicineClass med = medicineDataList[medicineIndexArray[i]];
            if (med.firstSymptom == Symptom.none)
            {
                continue;
            }
            medicineSymptomArray[(int)med.firstSymptom] += med.firstNumber;
            medicineSymptomArray[(int)med.secondSymptom] += med.secondNumber;
        }
        for(int i = 0; i < 6; i++)
        {
            if(medicineSymptomArray[i] + visitorSymptomArray[i] != 0)
            {
                goodMedicine = false;
                badSymptomList.Add((Symptom)i);
            }
        }
        StringBuilder builder;
        if (goodMedicine)
        {
            builder = new StringBuilder("아주 좋은 약이에요!!! 감사합니다!!");
        }
        else
        {
            builder = new StringBuilder("윽.....어딘가 이상해요......특히 ");
            for(int i = 0; i < badSymptomList.Count; i++)
            {
                builder.Append(badSymptomList[i]);
                if(i+1 != badSymptomList.Count)
                    builder.Append("하고 ");
            }
            builder.Append(" 쪽이 이상해요...");
        }

        StartCoroutine(VisitorDisapperCoroutine(builder.ToString()));
    }

    
    IEnumerator VisitorAppearCoroutine()
    {

        StartCoroutine(sceneManager.MoveModule_Accel2(visitorObject, visitorAppearPos, 2f));
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(sceneManager.LoadTextOneByOne(randomVisitorList[index-1].fullDialog, visitorText));


    }

    IEnumerator VisitorDisapperCoroutine(string dialog)
    {
        StartCoroutine(sceneManager.LoadTextOneByOne(dialog, visitorText));
        yield return new WaitForSeconds(4f);
        StartCoroutine(sceneManager.MoveModule_Accel2(visitorObject, visitorDisappearPos, 2f));
        yield return new WaitForSeconds(1.5f);
        SpawnRandomVisitor();
    }


    void Update()
    {

    }

}
