using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SymptomChartManager : MonoBehaviour
{

    [SerializeField]
    CounterManager counterManager;
    [SerializeField]
    MedicineManager medicineManager;

    [SerializeField]
    Text[] symptomChartTextArray;
    [SerializeField]
    GameObject symptomChartObject;

    [HideInInspector]
    public bool[] symptomMeasuredArray;

    List<MedicineButton> medicineInPotList;
    RandomVisitorClass nowVisitor;


    // Start is called before the first frame update
    void Start()
    {

        symptomMeasuredArray = new bool[5];
        for (int i = 0; i < 5; i++)
        {
            symptomMeasuredArray[i] = false;
        }
        medicineInPotList = medicineManager.medicineInPotList;
    }

    //증상기록 켜는 버튼, 끄는 버튼에서 여는거.
    public void SymptomChartButton(bool turnOn)
    {
        if (counterManager.endSales)
        {
            return;
        }
        if (turnOn)
        {
            symptomChartObject.SetActive(true);
        }
        else
        {
            symptomChartObject.SetActive(false);
        }
    }

    public void VisitorVisits(RandomVisitorClass visitor)
    {
        nowVisitor = visitor;
        for (int i = 0; i < 5; i++)
        {
            symptomMeasuredArray[i] = false;
        }
    }



    //차트가 변할 때 얘도 같이 변해야한다. counterManager의 symptomCheckToggle에서 호출
    public void ChangeSymptomChartText()
    {
        if (nowVisitor == null)
        {
            return;
        }
        int[] array = new int[6];
        for (int i = 0; i < 5; i++)
        {
            //if (i == 5)
            //{
            //    array[i] = counterManager.symptomCheckArray[i];
            //}
            //else
            //{
            if (symptomMeasuredArray[i] == true)
            {
                array[i] = nowVisitor.symptomAmountArray[i];
            }
            else
            {
                array[i] = counterManager.symptomCheckArray[i];
            }
            //}


        }
        for (int i = 0; i < medicineInPotList.Count; i++)
        {
            int firstSymtpom = (int)medicineInPotList[i].medicineClass.GetFirstSymptom();
            array[firstSymtpom] += medicineInPotList[i].medicineClass.firstNumber;

            int secondSymtpom = (int)medicineInPotList[i].medicineClass.GetSecondSymptom();
            array[secondSymtpom] += medicineInPotList[i].medicineClass.secondNumber;
        }

        for (int i = 0; i < 5; i++)
        {
            if (counterManager.symptomCheckedArray[i])
            {
                symptomChartTextArray[i].text = array[i].ToString();
            }
            else
            {
                symptomChartTextArray[i].text = "???";
            }



        }

    }

    //이거 measureTool에서 불러옴. 오버라이드 된 그거.
    public void SymptomMeasured(int index)
    {
        symptomMeasuredArray[index] = true;
        ChangeSymptomChartText();
    }
}
