using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;


public enum Symptom
{
    water, fire, dirt, wood, metal, light
}

//이거 제이슨으로 저장하는게 아님
//랜덤손님 1명이 가지고 있는 클래스
[System.Serializable]
public class RandomVisitorClass //SH
{
    public string name;
    public string fullDialog;
    //물 불 흙 나무 금속 빛

    public List<Symptom> symptomList;
    public List<int> symptomAmountList;
    //증상은 무조건 두 개    
    public List<MedicineClass> answerMedicineList;

    public Symptom earSymptom;
    public Symptom hornSymptom;

    /*
     * ownedMedicineList
1. ownedMedicineList 인덱스에서 랜덤으로 1~3개를 뽑는다(종류뽑기)
2. (약의 종류수) ~ (약의 종류수 + 1)랜덤 돌린다.(약의 개수 결정)
3. 만약 약의 종류수 +1 이 나왔으면 약의 종류 중 랜덤으로 하나 뽑는다(어느 약이 2개가 들어갈지 결정)
4. 약의 종류수 +0이 나왔으면 모든 약재가 1개씩 들어간다
    약재 한개도 되게 하고 기본 베이스(무속성 물)줄수있게
*/

    //약재의 종류는 최대 3개
    //약재 개수는 같은거는 최대 2개.
    public RandomVisitorClass(SymptomDialog dialog, List<MedicineClass> ownedMedicineList)
    {
        earSymptom = (Symptom)Random.Range(0, 6);
        hornSymptom = (Symptom)Random.Range(0, 6);
        symptomList = new List<Symptom>();
        symptomAmountList = new List<int>();
        List<MedicineClass> availableMedicineList = new List<MedicineClass>();
        int[] symptomNumberArray = new int[6];
        for(int i = 0; i < symptomNumberArray.Length; i++)
        {
            symptomNumberArray[i] = 0;
        }
        //int symptomNumber = Random.Range(2, 5);
        int symptomNumber = 6;
        int nowMedicineNumber = 0;
        //MedicineClass firstMedicine = ownedMedicineList[firstMedicineIndex];
        //first number 는 -1, 1, SecondNumber 은 2, -2
        answerMedicineList = new List<MedicineClass>();
        //첫번째 약재 정해줌

        //두번쨰 약재 정해줌. 이제부터 가능한 약재 리스트가 들어감.
        int forIndex = 0;
        while (symptomNumber > nowMedicineNumber)
        {

            //가지고있는 약재 한바퀴 돌면서 가능한 약재 찾기.
            for (int i = 0; i < ownedMedicineList.Count; i++)
            {
                MedicineClass medicine = ownedMedicineList[i];
                bool available = false;
                if(medicine.firstSymptom == earSymptom || medicine.secondSymptom == earSymptom 
                    || medicine.firstSymptom == hornSymptom || medicine.secondSymptom == hornSymptom)
                {
                    //귀랑 뿔 겹치는거 빼주고
                    continue;
                }
                available = true;
                //만약 증상 합이 2나 -2 넘어가면 그 약은 안되는거니까 다시돌려
                int amount = symptomNumberArray[(int)medicine.firstSymptom] + medicine.firstNumber;
                if (amount < -2 || amount > 2)
                {
                    available = false;
                }
                amount = symptomNumberArray[(int)medicine.secondSymptom] + medicine.secondNumber;
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
                if(nowMedicineNumber==0)
                Debug.Log("좃됐따");
                //되는게 하나도 없을 때는 약이 0개다. 맹물.
                //이건 첫번째 루프 돌 때는 절대 안일어난다. 약종류가 기본 3개니까.
                break;
            }
            //하나 골라서 심텀에 추가
            int randomIndex = Random.Range(0, availableMedicineList.Count);

            MedicineClass answerMedicine = availableMedicineList[randomIndex];
            symptomNumberArray[(int)answerMedicine.firstSymptom] += answerMedicine.firstNumber;
            symptomNumberArray[(int)answerMedicine.secondSymptom] += answerMedicine.secondNumber;
            answerMedicineList.Add(answerMedicine);
            nowMedicineNumber++;
            availableMedicineList.Clear();
            forIndex++;
        }
        //여기 위까지가 판정, 정답약 넣는거.
        for(int i = 0; i < symptomNumberArray.Length; i++)
        {
            if (symptomNumberArray[i] != 0)
            {
                symptomList.Add((Symptom)i);
                symptomAmountList.Add(-1 * symptomNumberArray[i]);
            }
        }
        if(nowMedicineNumber == 0)
        {
            fullDialog = "저는 아픈데가 없는데 왜온거죠";
            return;
        }

        
        string iamString = "저는 ";
        string andString = ", 하고";
        string problemString = " 에 문제가 있는거 같아요. \n";

        StringBuilder build = new StringBuilder(iamString);
        if (symptomList.Count == 0)
        {
            fullDialog = "저는 아픈데가 없는데 왜온거죠";
            return;
        }
        build.Append(symptomList[0].ToString());
        for(int i = 1; i < symptomList.Count; i++)
        {
            build.Append(andString);
            build.Append(symptomList[i]);
        }
        build.Append(problemString);

        int symptomIndex = 0;
        if (symptomAmountList[0] > 0)
        {
            symptomIndex = symptomAmountList[0] + 1;
        }
        else
        {
            symptomIndex = symptomAmountList[0] + 2;
        }
        string firstDialog = dialog.symptomDialogArray[(int)symptomList[0]].dialogArray[symptomIndex];
        build.Append(symptomList[0]);
        build.Append("의 증상은 ");
        build.Append(firstDialog);
        for (int i = 1; i < symptomList.Count; i++)
        {
            build.Append(dialog.middleDialog[Random.Range(0, 6)]);
            build.Append(symptomList[i]);
            build.Append("의 증상은 ");
            symptomIndex = 0;
            if (symptomAmountList[i] > 0)
            {
                symptomIndex = symptomAmountList[i] + 1;
            }
            else
            {
                symptomIndex = symptomAmountList[i] + 2;
            }
            build.Append(dialog.symptomDialogArray[(int)symptomList[i]]
            .dialogArray[symptomIndex]);
        }

        fullDialog = build.ToString();

    }


}
