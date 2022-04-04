using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;
using Coffee.UIEffects;

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

public class SymptomObject
{
    public GameObject obj;
    public Symptom symptom;
    public bool dissolve;
    public UIDissolve dissolveComponent;
    //public string disease;
    public int amount;

    public SymptomObject()
    {
        symptom = Symptom.water;
        dissolve = true;
    }
}

//이거 제이슨으로 저장하는게 아님
//랜덤손님 1명이 가지고 있는 클래스
[System.Serializable]
public class RandomVisitorClass : VisitorClass
{

    public string name;
    //public string fullDialog;
    //물 불 흙 나무 금속 빛

    
    public List<Symptom> symptomList;
    public List<int> symptomAmountList;
    //증상은 무조건 두 개    
    public List<MedicineClass> answerMedicineList;
    

    public static int gainCoin = 40;


    /*
    public Symptom earSymptom;
    public Symptom hornSymptom;
    */
    //body
    // head face hair body 
    // ear horn
    //인덱스별로 레이어가 있음. 각 파츠가 몇개까지 있는지 미리 저장해두고 그 인덱스에서 뽑아옴.

    //public GameObjectWrapper[] partsWrapperArray;


    bool childSetParented;



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
    public RandomVisitorClass(GameObject parent,StoryRegion region)
    {
        //earSymptom = (Symptom)Random.Range(0, 6);
        visitorType = VisitorType.Random;
        //hornSymptom = (Symptom)Random.Range(0, 6);
        symptomList = new List<Symptom>();
        symptomAmountList = new List<int>();
        symptomAmountArray = new int[6];
        diseaseList = new List<RandomVisitorDisease>();
        symptomObjectList = new List<SymptomObject>();
        finalSymptomObjectList = new List<SymptomObject>();
        List<MedicineClass> availableMedicineList = new List<MedicineClass>();
        int[] symptomNumberArray = new int[5];
        for(int i = 0; i < symptomNumberArray.Length; i++)
        {
            symptomNumberArray[i] = 0;
        }
        int symptomNumber = Random.Range(1, 4);
        //int symptomNumber = 3;
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
            bool justGoForOneMedicine = false;
            while (reChoice)
            {
                if (symptomNumberArray[(int)answerMedicine.GetFirstSymptom()] + answerMedicine.firstNumber == 0 &&
                symptomNumberArray[(int)answerMedicine.GetSecondSymptom()] + answerMedicine.secondNumber == 0)
                {
                    if(availableMedicineList.Count == 1)
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
        SetDiseaseList();
        RandomPartsGenerator(parent,region);
        StartSymptomSpriteUpdate();
    }



    //랜덤캐릭터 만드는 함수
   

    public override void StartSymptomSpriteUpdate()
    {
        childSetParented = false;
        //partsWrapperArray[2].partsArray[0] 이게 헤드임.
        for (int i = 0; i < diseaseList.Count; i++)
        {

            if (diseaseList[i].firstSpriteName != null)
            {
                GameObject obj = diseaseList[i].LoadObject(true, visitorObject.transform).transform.GetChild(0).gameObject;
                UIDissolve dissolve = obj.GetComponent<UIDissolve>();
                dissolve.effectFactor = 0;
                SymptomObject symptomObject = new SymptomObject();
                symptomObject.obj = obj;
                symptomObject.dissolve = true;
                //symptomObject.disease = diseaseList[i].sympotmString;
                symptomObject.amount = diseaseList[i].symptomNumber;
                symptomObject.dissolveComponent = dissolve;
                symptomObjectList.Add(symptomObject);
                if (diseaseList[i].firstSpriteName.Contains("face") && headPart != null)
                {
                    if (childSetParented == false)
                    {
                        for (int j = 0; j < facePart.Length; j++)
                        {
                            facePart[j].transform.GetChild(0).SetParent(headPart.transform.GetChild(0));
                        }
                    }
                    childSetParented = true;
                    obj.transform.SetParent(headPart.transform.GetChild(0));
                    obj.transform.SetAsLastSibling();
                    obj.transform.localScale = Vector3.one;
                    obj.transform.localPosition = Vector3.zero;


                }
                else
                {
                    obj.transform.parent.SetParent(visitorObject.transform);
                    obj.transform.localPosition = new Vector3(0, 0, diseaseList[i].GetFirstLayer());

                }


            }
            if (diseaseList[i].secondSpriteName != null)
            {
                GameObject obj = diseaseList[i].LoadObject(false, visitorObject.transform).transform.GetChild(0).gameObject;
                UIDissolve dissolve = obj.GetComponent<UIDissolve>();
                dissolve.effectFactor = 0;
                SymptomObject symptomObject = new SymptomObject();
                symptomObject.obj = obj;
                symptomObject.dissolve = true;
                symptomObject.dissolveComponent = dissolve;
                //symptomObject.disease = diseaseList[i].sympotmString;
                symptomObject.amount = diseaseList[i].symptomNumber;
                symptomObjectList.Add(symptomObject);
                if (diseaseList[i].secondSpriteName.Contains("face") && headPart != null)
                {
                    for (int j = 0; j < facePart.Length; j++)
                    {
                        facePart[j].transform.GetChild(0).SetParent(headPart.transform.GetChild(0));
                    }
                    obj.transform.SetParent(headPart.transform.GetChild(0).transform);
                    obj.transform.SetAsLastSibling();
                    obj.transform.localScale = Vector3.one;
                    obj.transform.localPosition = Vector3.zero;
                }
                else
                {
                    obj.transform.parent.SetParent(visitorObject.transform);
                    obj.transform.localPosition = new Vector3(0, 0, diseaseList[i].GetSecondLayer());

                }

            }

        }
    }

    public override void FinalSymptomSpriteUpdate(int[] finalSymptomArray)
    {
        List<RandomVisitorDisease> finalDiseaseList = new List<RandomVisitorDisease>();
        //for(int i = 0; i < symptomObjectList.Count; i++)
        //{
        //    symptomObjectList[i].SetActive(false);
        //}
        for (int i = 0; i < finalSymptomArray.Length; i++)
        {
            int amount = finalSymptomArray[i];
            if (amount == 0)
            {
                continue;
            }
            if (amount < -2)
            {
                amount = -2;
            }
            else if (amount > 2)
            {
                amount = 2;
            }

            if (amount == symptomAmountArray[i])
            {
                for (int j = 0; j < symptomObjectList.Count; j++)
                {
                    if(symptomObjectList[j].symptom== (Symptom)i)
                    {
                        symptomObjectList[j].dissolve = false;
                    }
                }
                continue;
            }

            List<int> diseaseIndexList = new List<int>();
            for (int j = 0; j < diseaseBundle.wrapperList[i].randomVisitorDiseaseArray.Length; j++)
            {
                if (amount == diseaseBundle.wrapperList[i].randomVisitorDiseaseArray[j].symptomNumber)
                {
                    
                    diseaseIndexList.Add(j);
                }
            }
            if(diseaseIndexList.Count == 0)
            {
                return;
            }
            int index = diseaseIndexList[Random.Range(0, diseaseIndexList.Count)];
            finalDiseaseList.Add(diseaseBundle.wrapperList[i].randomVisitorDiseaseArray[index]);
        }

        
        for (int i = 0; i < finalDiseaseList.Count; i++)
        {

            if (finalDiseaseList[i].firstSpriteName != null)
            {
                GameObject obj = finalDiseaseList[i].LoadObject(true, visitorObject.transform).transform.GetChild(0).gameObject;
                UIDissolve dissolve = obj.GetComponent<UIDissolve>();
                dissolve.effectFactor = 1;
                SymptomObject symptomObject = new SymptomObject();
                symptomObject.obj = obj;
                symptomObject.dissolve = true;
                symptomObject.dissolveComponent = dissolve;
                finalSymptomObjectList.Add(symptomObject);
                if (finalDiseaseList[i].firstSpriteName.Contains(faceString) && headPart != null)
                {
                    if (childSetParented == false)
                    {
                        for (int j = 0; j < facePart.Length; j++)
                        {

                            facePart[j].transform.GetChild(0).SetParent(headPart.transform.GetChild(0));
                        }
                    }
                    childSetParented = true;
                    obj.transform.SetParent(headPart.transform.GetChild(0));
                    obj.transform.SetAsLastSibling();
                    obj.transform.localScale = Vector3.one;
                    obj.transform.localPosition = Vector3.zero;


                }
                else
                {
                    obj.transform.parent.SetParent(visitorObject.transform);
                    obj.transform.localPosition = new Vector3(0, 0, finalDiseaseList[i].GetFirstLayer());

                }


            }
            if (finalDiseaseList[i].secondSpriteName != null)
            {
                GameObject obj = finalDiseaseList[i].LoadObject(false, visitorObject.transform).transform.GetChild(0).gameObject;
                UIDissolve dissolve = obj.GetComponent<UIDissolve>();
                dissolve.effectFactor = 1;
                SymptomObject symptomObject = new SymptomObject();
                symptomObject.obj = obj;
                symptomObject.dissolveComponent = dissolve;
                symptomObject.dissolve = true;
                finalSymptomObjectList.Add(symptomObject);
                if (finalDiseaseList[i].secondSpriteName.Contains(faceString) && headPart != null)
                {
                    for (int j = 0; j < facePart.Length; j++)
                    {
                        facePart[j].transform.GetChild(0).SetParent(headPart.transform.GetChild(0));
                    }
                    obj.transform.SetParent(headPart.transform.GetChild(0).transform);
                    obj.transform.SetAsLastSibling();
                    obj.transform.localScale = Vector3.one;
                    obj.transform.localPosition = Vector3.zero;
                }
                else
                {
                    obj.transform.parent.SetParent(visitorObject.transform);
                    obj.transform.localPosition = new Vector3(0, 0, finalDiseaseList[i].GetSecondLayer());

                }

            }

        }
    }

    public override void FaceShifter(Feeling feeling)
    {
        base.FaceShifter(feeling);
    }


    //카운터매니저에서 불러옴.134줄


}
