using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//약의 전체 데이터
[System.Serializable]
public class MedicineDataWrapper //SH
{
    //이거 영표형이 만들어준 제이슨으로 불러오는 기능만 하는거
    //이거 나중에 어레이로 바꿀꺼임. 리스트로 쓸 이유가 없음 어차피 고정데이터야
    public List<MedicineClass> medicineDataList;

    public MedicineDataWrapper()
    {
        medicineDataList = new List<MedicineClass>();

        //여기아래부터 디버그용임
        MedicineClass medicine = new MedicineClass();
        medicine.firstName = "아무것도";
        medicine.secondName = "아님";
        medicine.firstSymptom = Symptom.none;
        medicine.secondSymptom = Symptom.none;
        medicine.firstNumber = 0;
        medicine.secondNumber = 0;
        medicineDataList.Add(medicine);

        //6 * 6 * 2 * 2


        for (int i = 0; i < 5; i++)
        {
            for(int j = 0; j < 5; j++)
            {
                if (i == j)
                {
                    continue;
                }
                for(int k = 0; k < 2; k++)
                {
                    for(int h = 0; h < 2; h++)
                    {
                        medicine = new MedicineClass();
                        int num = j % 4;
                        if(num == 0)
                        {
                            medicine.firstName = "싸늘한";
                                medicine.secondName =  "열매";
                        }
                        else if(num == 1)
                        {
                            medicine.firstName = "축축한";
                            medicine.secondName = "원석";
                        }
                        else if (num == 2)
                        {
                            medicine.firstName = "흐르는";
                            medicine.secondName = "불꽃";
                        }
                        else if (num == 3)
                        {
                            medicine.firstName = "시든";
                            medicine.secondName = "이슬";
                        }
                        medicine.firstSymptom = (Symptom)i;
                        medicine.secondSymptom = (Symptom)j;
                        medicine.cost = i + j + k + h + 1;
                        if (k == 0)
                        {
                            medicine.firstNumber = -1;
                        }
                        else
                        {
                            medicine.firstNumber = 1;
                        }
                        if (h == 0)
                        {
                            medicine.secondNumber = -2;
                        }
                        else
                        {
                            medicine.secondNumber = 2;
                        }
                        medicineDataList.Add(medicine);

                    }

                }
            }
        }

       
    }
}
