using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//약의 전체 데이터
[System.Serializable]
public class MedicineDictionary //SH
{
    //이거 영표형이 만들어준 제이슨으로 불러오는 기능만 하는거
    public List<MedicineClass> medicineList;

    public MedicineDictionary(List<int> random)
    {
        medicineList = new List<MedicineClass>();

        //여기아래부터 디버그용임
        MedicineClass medicine = new MedicineClass();
        medicine.name = "싸늘한 열매";
        medicine.firstSymptom = Symptom.fire;
        medicine.secondSymptom = Symptom.dirt;
        medicine.firstNumber = -1;
        medicine.secondNumber = 2;
        medicineList.Add(medicine);

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
                        medicineList.Add(medicine);

                    }

                }
            }
        }

        /*
        for(int i = 0; i < 200; i+=2)
        {
            medicine = new MedicineClass();
            medicine.firstSymptom = (Symptom)Random.Range(0, 6);
            medicine.secondSymptom = (Symptom)Random.Range(0, 6);
            while(medicine.secondSymptom == medicine.firstSymptom)
            {
                medicine.secondSymptom = (Symptom)Random.Range(0, 6);
            }

           
            if(random[i] == 0)
            {
                medicine.firstNumber = -1;
            }
            else
            {
                medicine.firstNumber = 1;
            }
            if (random[i+1] == 0)
            {
                medicine.secondNumber = -2;
            }
            else
            {
                medicine.secondNumber = 2;
            }
            medicineList.Add(medicine);
        }*/

        /*
        medicine = new MedicineClass();
        medicine.name = "흐르는 불꽃";
        medicine.firstSymptom = Symptom.water;
        medicine.secondSymptom = Symptom.light;
        medicine.firstNumber = 1;
        medicine.secondNumber = 2;
        medicineList.Add(medicine);

        medicine = new MedicineClass();
        medicine.name = "시든 열매";
        medicine.firstSymptom = Symptom.wood;
        medicine.secondSymptom = Symptom.dirt;
        medicine.firstNumber = -1;
        medicine.secondNumber = -2;
        medicineList.Add(medicine);

        medicine = new MedicineClass();
        medicine.name = "싸늘한 열매";
        medicine.firstSymptom = Symptom.fire;
        medicine.secondSymptom = Symptom.dirt;
        medicine.firstNumber = 1;
        medicine.secondNumber = -2;
        medicineList.Add(medicine);

        medicine = new MedicineClass();
        medicine.name = "흐르는 불꽃";
        medicine.firstSymptom = Symptom.water;
        medicine.secondSymptom = Symptom.fire;
        medicine.firstNumber = 1;
        medicine.secondNumber = 2;
        medicineList.Add(medicine);

        medicine = new MedicineClass();
        medicine.name = "시든 열매";
        medicine.firstSymptom = Symptom.wood;
        medicine.secondSymptom = Symptom.metal;
        medicine.firstNumber = 1;
        medicine.secondNumber = 2;
        medicineList.Add(medicine);

        medicine = new MedicineClass();
        medicine.name = "싸늘한 열매";
        medicine.firstSymptom = Symptom.wood;
        medicine.secondSymptom = Symptom.dirt;
        medicine.firstNumber = 11;
        medicine.secondNumber = 2;
        medicineList.Add(medicine);

        medicine = new MedicineClass();
        medicine.name = "흐르는 불꽃";
        medicine.firstSymptom = Symptom.metal;
        medicine.secondSymptom = Symptom.light;
        medicine.firstNumber = 1;
        medicine.secondNumber = 2;
        medicineList.Add(medicine);

        medicine = new MedicineClass();
        medicine.name = "시든 열매";
        medicine.firstSymptom = Symptom.wood;
        medicine.secondSymptom = Symptom.metal;
        medicine.firstNumber = -1;
        medicine.secondNumber = -2;
        medicineList.Add(medicine);

        medicine = new MedicineClass();
        medicine.name = "싸늘한 열매";
        medicine.firstSymptom = Symptom.light;
        medicine.secondSymptom = Symptom.dirt;
        medicine.firstNumber = 1;
        medicine.secondNumber = -2;
        medicineList.Add(medicine);

        medicine = new MedicineClass();
        medicine.name = "흐르는 불꽃";
        medicine.firstSymptom = Symptom.dirt;
        medicine.secondSymptom = Symptom.fire;
        medicine.firstNumber = 1;
        medicine.secondNumber = 2;
        medicineList.Add(medicine);

        medicine = new MedicineClass();
        medicine.name = "시든 열매";
        medicine.firstSymptom = Symptom.fire;
        medicine.secondSymptom = Symptom.metal;
        medicine.firstNumber = 1;
        medicine.secondNumber = 2;
        medicineList.Add(medicine);
        */

    }
}
