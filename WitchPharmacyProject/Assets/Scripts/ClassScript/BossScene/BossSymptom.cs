using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
public class BossSymptom
{
    static List<MedicineClass> ownedMedicineList;
    public static void SetStaticData(List<MedicineClass> ownedMedicineList)
    {
        BossSymptom.ownedMedicineList = ownedMedicineList;
    }

    public int[] symptomAmountArray;
    List<Symptom> symptomList;
    List<int> symptomAmountList;
    List<MedicineClass> answerMedicineList;

    public GameObject symptomObjectParent;


    public BossSymptom(GameObject parent)
    {
        //earSymptom = (Symptom)Random.Range(0, 6);
        //hornSymptom = (Symptom)Random.Range(0, 6);
        symptomObjectParent = parent;
        symptomList = new List<Symptom>();
        symptomAmountList = new List<int>();
        symptomAmountArray = new int[5];
        SymptomAccepter();

        Text symptomText = symptomObjectParent.transform.GetChild(0).GetChild(0).GetComponent<Text>();
        StringBuilder builder = new StringBuilder();
        UILanguagePack languagePack = GameManager.singleton.languagePack;

        for(int i = 0; i < symptomAmountArray.Length; i++)
        {
            if(symptomAmountArray[i] ==0)
            {
                continue;
            }

            builder.Append(languagePack.symptomArray[i]);
            builder.Append(symptomAmountArray[i].ToString());
        }
        symptomText.text = builder.ToString();


    }


    void SymptomAccepter()
    {
        List<MedicineClass> availableMedicineList = new List<MedicineClass>();
        int[] symptomNumberArray = new int[5];
        for (int i = 0; i < symptomNumberArray.Length; i++)
        {
            symptomNumberArray[i] = 0;
        }
        int symptomNumber = Random.Range(1, 4);
        //int symptomNumber = 3;
        if (symptomNumber > ownedMedicineList.Count)
        {
            symptomNumber = ownedMedicineList.Count;
        }
        //int symptomNumber = 1;
        int nowMedicineNumber = 0;
        //MedicineClass firstMedicine = ownedMedicineList[firstMedicineIndex];
        //first number 는 -1, 1, SecondNumber 은 2, -2
        answerMedicineList = new List<MedicineClass>();


        //첫번쨰 약재 정해줌. 이제부터 가능한 약재 리스트가 들어감.
        /* 첫번째 루프는 약재를 정하고, 그 약재의 + - 값을 고려해서 가능한 약재의 리스트를 뽑아낸다
         * 예를들어 물+ 불--라면, 불- 나무++은 제외, 나무+ 빛--는 넣는 식. +2 위 -2아래로는 다 재낀다.
         * 그 리스트들을 뽑았다 치면 그 다음 루프에서 증상어레이에 값을 다시 더해서 누적계산하는방식.
         */
        while (symptomNumber > nowMedicineNumber || nowMedicineNumber == 0)
        {

            //가지고있는 약재 한바퀴 돌면서 가능한 약재 찾기.
            for (int i = 0; i < ownedMedicineList.Count; i++)
            {
                bool answerCheck = false;
                for (int j = 0; j < answerMedicineList.Count; j++)
                {
                    if (ownedMedicineList[i].GetIndex() == answerMedicineList[j].GetIndex())
                    {
                        answerCheck = true;
                        break;
                    }
                }
                if (answerCheck)
                {
                    continue;
                }
                MedicineClass medicine = ownedMedicineList[i];
                bool available = true;
                //if (medicine.firstSymptom == Symptom.none)
                //{
                //    continue;
                //}
                /*
                if(medicine.firstSymptom == earSymptom || medicine.secondSymptom == earSymptom 
                    || medicine.firstSymptom == hornSymptom || medicine.secondSymptom == hornSymptom)
                {
                    //귀랑 뿔 겹치는거 빼주고
                    continue;
                }*/
                //만약 증상 합이 2나 -2 넘어가면 그 약은 안되는거니까 다시돌려
                //여기가 누적계산하는 곳
                int amount = symptomNumberArray[(int)medicine.GetFirstSymptom()] + medicine.firstNumber;
                if (amount < -2 || amount > 2)
                {
                    available = false;
                }
                amount = symptomNumberArray[(int)medicine.GetSecondSymptom()] + medicine.secondNumber;
                if (amount < -2 || amount > 2)
                {
                    available = false;
                }
                if (available)
                {
                    //가능리스트 만들기.
                    availableMedicineList.Add(medicine);
                }
            }
            //Debug.Log(availableMedicineList.Count +"이고 " + forIndex.ToString() +  "번째"); ;
            if (availableMedicineList.Count == 0)
            {
                if (nowMedicineNumber == 0)
                    Debug.Log("좃됐따");
                //되는게 하나도 없을 때는 약이 0개다. 맹물.
                //이건 첫번째 루프 돌 때는 절대 안일어난다. 약종류가 기본 3개니까.
                break;
            }
            //하나 골라서 심텀에 추가
            int randomIndex = Random.Range(0, availableMedicineList.Count);
            MedicineClass answerMedicine = availableMedicineList[randomIndex];
            bool reChoice = true;
            bool justGoForOneMedicine = false;
            while (reChoice)
            {
                if (symptomNumberArray[(int)answerMedicine.GetFirstSymptom()] + answerMedicine.firstNumber == 0 &&
                symptomNumberArray[(int)answerMedicine.GetSecondSymptom()] + answerMedicine.secondNumber == 0)
                {
                    if (availableMedicineList.Count == 1)
                    {
                        justGoForOneMedicine = true;
                        Debug.LogError("야 진짜진짜 졷됀거니까 조심해라");
                        break;
                    }
                    else
                    {
                        randomIndex = Random.Range(0, availableMedicineList.Count);
                        answerMedicine = availableMedicineList[randomIndex];
                    }

                }
                else
                {
                    reChoice = false;
                }

            }

            if (!justGoForOneMedicine)
            {
                symptomNumberArray[(int)answerMedicine.GetFirstSymptom()] += answerMedicine.firstNumber;
                symptomNumberArray[(int)answerMedicine.GetSecondSymptom()] += answerMedicine.secondNumber;
                answerMedicineList.Add(answerMedicine);
                nowMedicineNumber++;
                //  break;
            }

            availableMedicineList.Clear();
        }
        //여기 위까지가 판정, 정답약 넣는거.
        for (int i = 0; i < symptomNumberArray.Length; i++)
        {
            if (symptomNumberArray[i] != 0)
            {
                symptomList.Add((Symptom)i);
                symptomAmountList.Add(-1 * symptomNumberArray[i]);

            }
            symptomAmountArray[i] = (-1 * symptomNumberArray[i]);
        }
    }






}
