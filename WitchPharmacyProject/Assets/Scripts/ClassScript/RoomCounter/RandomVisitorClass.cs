using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

[System.Serializable]
public enum Symptom
{
    //0     1     2     3    4 
    water, fire, earth, air, light, special, none
}

//랜덤캐릭터 할 때 한요소가 두개씩 있는경우가 있어서 그럼.
public class GameObjectWrapper
{
    public GameObject[] partsArray;
}

//이거 제이슨으로 저장하는게 아님
//랜덤손님 1명이 가지고 있는 클래스
[System.Serializable]
public class RandomVisitorClass //SH
{
    //이거 만들어줘야 카운터 클래스에서 setactiveFalse로 끌 수 있음
    public GameObject visitorObject;

    public string name;
    //public string fullDialog;
    //물 불 흙 나무 금속 빛

    public List<Symptom> symptomList;
    public List<int> symptomAmountList;
    public int[] symptomAmountArray;
    //증상은 무조건 두 개    
    public List<MedicineClass> answerMedicineList;
    static List<MedicineClass> ownedMedicineList;
    public static int gainCoin = 40;
    

    /*
    public Symptom earSymptom;
    public Symptom hornSymptom;
    */
    //body clothes head face hair ear horn
    //인덱스별로 레이어가 있음. 각 파츠가 몇개까지 있는지 미리 저장해두고 그 인덱스에서 뽑아옴.
    static int[] partsNum = { 1, 2, 2, 2, 2, 4, 4 };
    int[] partsIndex;
    public GameObjectWrapper[] partsWrapperArray;



    /*body	_00	_01    	
*1. 머리카락 	hair	_00	_02 	(*hair_00_02 파츠가 있는 경우에만)
2. 얼굴 (얼굴형)	face	_00	_01
3. 얼굴 (이목구비)	face	_00	_02
4. 머리카락	hair	_00	_01
5. 귀		ear	_00	     	(단일 레이어)
6. 뿔		horn 


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
    public RandomVisitorClass(SymptomDialog dialog,GameObject parent)
    {
        //earSymptom = (Symptom)Random.Range(0, 6);
        //hornSymptom = (Symptom)Random.Range(0, 6);
        symptomList = new List<Symptom>();
        symptomAmountList = new List<int>();
        symptomAmountArray = new int[6];
        List<MedicineClass> availableMedicineList = new List<MedicineClass>();
        int[] symptomNumberArray = new int[5];
        for(int i = 0; i < symptomNumberArray.Length; i++)
        {
            symptomNumberArray[i] = 0;
        }
        int symptomNumber = Random.Range(1, 4);
        if(symptomNumber > ownedMedicineList.Count)
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
            while (reChoice)
            {
                if (symptomNumberArray[(int)answerMedicine.GetFirstSymptom()] + answerMedicine.firstNumber == 0 &&
                symptomNumberArray[(int)answerMedicine.GetSecondSymptom()] + answerMedicine.secondNumber == 0)
                {
                    randomIndex = Random.Range(0, availableMedicineList.Count);
                    answerMedicine = availableMedicineList[randomIndex];
                }
                else
                {
                    reChoice = false;
                }

            }

            symptomNumberArray[(int)answerMedicine.GetFirstSymptom()] += answerMedicine.firstNumber;
            symptomNumberArray[(int)answerMedicine.GetSecondSymptom()] += answerMedicine.secondNumber;
            answerMedicineList.Add(answerMedicine);
            nowMedicineNumber++;
            Debug.Log(nowMedicineNumber);
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

        //솔직히 이 아래 다 디버그용임. 실코드에 안쓸것
        //string iamString = "저는 ";
        //string andString = ", 하고";
        //string problemString = " 에 문제가 있는거 같아요. \n";

        //StringBuilder build = new StringBuilder(iamString);
        //build.Append(symptomList[0].ToString());
        //for(int i = 1; i < symptomList.Count; i++)
        //{
        //    build.Append(andString);
        //    build.Append(symptomList[i]);
        //}
        //build.Append(problemString);

        //int symptomIndex = 0;
        //if (symptomAmountList[0] > 0)
        //{
        //    symptomIndex = symptomAmountList[0] + 1;
        //}
        //else
        //{
        //    symptomIndex = symptomAmountList[0] + 2;
        //}
        //string firstDialog = dialog.symptomDialogArray[(int)symptomList[0]].dialogArray[symptomIndex];
        //build.Append(symptomList[0]);
        //build.Append("의 증상은 ");
        //build.Append(firstDialog);
        //for (int i = 1; i < symptomList.Count; i++)
        //{
        //    build.Append(dialog.middleDialog[Random.Range(0, 6)]);
        //    build.Append(symptomList[i]);
        //    build.Append("의 증상은 ");
        //    symptomIndex = 0;
        //    if (symptomAmountList[i] > 0)
        //    {
        //        symptomIndex = symptomAmountList[i] + 1;
        //    }
        //    else
        //    {
        //        symptomIndex = symptomAmountList[i] + 2;
        //    }
        //    build.Append(dialog.symptomDialogArray[(int)symptomList[i]]
        //    .dialogArray[symptomIndex]);
        //}

        //fullDialog = build.ToString();

        RandomPartsGenerator(parent);
    }


    //랜덤캐릭터 만드는 함수
    void RandomPartsGenerator(GameObject parent)
    {
        string path = "RandomCharacter/";
        Transform visitorParent = parent.transform;
        GameObject visitor = new GameObject();
        visitor.transform.SetParent(visitorParent);
        visitorObject = visitor;

        //if (Random.Range(0, 2) == 0)
        //{
        //    //선아누나가 릴리 나오게 해달래서 여기다가 구현.
        //    StringBuilder builder = new StringBuilder(path);
        //    builder.Append("Lily");
        //    GameObject lily = Resources.Load<GameObject>(builder.ToString());
        //    GameObject part = GameObject.Instantiate(lily, visitor.transform);
        //    part.transform.position = new Vector3(0, -11.91f, -1);
        //}
        //else
        //{

           
        //}

        visitorObject.transform.localPosition = Vector3.zero;

        //먼저 래퍼 7개를 만들고.
        partsIndex = new int[7];
        partsWrapperArray = new GameObjectWrapper[partsIndex.Length];

        for (int i = 0; i < partsIndex.Length; i++)
        {
            partsWrapperArray[i] = new GameObjectWrapper();
            partsIndex[i] = Random.Range(0, partsNum[i]);
            StringBuilder builder = new StringBuilder(path);
            switch (i)
            {
                case 0:
                    builder.Append("body/");
                    break;
                case 1:
                    builder.Append("clothes/");
                    break;
                case 2:
                    builder.Append("head/");
                    break;
                case 3:
                    builder.Append("face/");
                    break;
                case 4:
                    builder.Append("hair/");
                    break;
                case 5:
                    builder.Append("ear/");
                    break;
                case 6:
                    builder.Append("horn/");
                    break;
                default:
                    break;

            }
            builder.Append(partsIndex[i]);
            partsWrapperArray[i].partsArray = Resources.LoadAll<GameObject>(builder.ToString());
        }

        for (int i = 0; i < partsWrapperArray.Length; i++)
        {
            for (int j = 0; j < partsWrapperArray[i].partsArray.Length; j++)
            {
                GameObject part = GameObject.Instantiate(partsWrapperArray[i].partsArray[j], visitor.transform);
                if (i == 4 && j == 1)
                {
                    part.transform.localPosition = new Vector3(0, 0, 1.5f);
                }
                else
                {
                    part.transform.localPosition = new Vector3(0, 0, 1 - 0.1f * (i + 1) - 0.1f * (j + 1));
                }

            }

        }



    }

    //카운터매니저에서 불러옴.134줄
    public static void SetOwnedMedicineList(List<MedicineClass> ownedMedicineList)
    {
        RandomVisitorClass.ownedMedicineList = ownedMedicineList;
    }


}
