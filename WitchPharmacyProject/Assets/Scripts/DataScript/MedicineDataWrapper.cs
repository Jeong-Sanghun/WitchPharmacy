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
        medicine.name = "싸늘한 열매";
        medicine.firstSymptom = Symptom.fire;
        medicine.secondSymptom = Symptom.dirt;
        medicine.firstNumber = -1;
        medicine.secondNumber = 2;
        medicineDataList.Add(medicine);

        //6 * 6 * 2 * 2


        for(int i = 0; i < 6; i++)
        {
            for(int j = 0; j < 6; j++)
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
                            medicine.name = "싸늘한 열매";
                        }
                        else if(num == 1)
                        {
                            medicine.name = "축축한 원석";
                        }
                        else if (num == 2)
                        {
                            medicine.name = "흐르는 불꽃";
                        }
                        else if (num == 3)
                        {
                            medicine.name = "시든 이슬";
                        }
                        medicine.firstSymptom = (Symptom)i;
                        medicine.secondSymptom = (Symptom)j;
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
