using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;

//약 1개가 가지고 있는 약 클래스 
//게임 내에서도 쓰일거고, 제이슨 저장용으로도 쓰일것이다.
[System.Serializable]
public class MedicineClass : SpecialMedicineClass  //SH
{
    //int index;
    //public string fileName;
    //public string firstName;
    //public string secondName;
    //public string toolTip;
    public static Dictionary<string, Sprite> symptomIconDictionary;

    public string firstSymptomText;
    public string secondSymptomText;
    Symptom firstSymptom;
    Symptom secondSymptom;
    public int firstNumber;
    public int secondNumber;
    //
    //public int cost;

    //Sprite medicineImage;
      
    //public GameObject realGameobject;


    //public int GetIndex()
    //{
    //    return index;
    //}

    //public void SetIndex(int _index)
    //{
    //    index = _index;
    //}

    public Symptom GetFirstSymptom()
    {
        return firstSymptom;
    }

    public void SetSymptom(Symptom first, Symptom second)
    {
        firstSymptom = first;
        secondSymptom = second;
    }

    public void ParseSymptom()
    {
        StringBuilder nameBuilder = new StringBuilder(firstSymptom.ToString());
        if (firstNumber == 1)
        {
            nameBuilder.Append("+");
        }
        else
        {
            nameBuilder.Append("-");
        }
        nameBuilder.Append(secondSymptom.ToString());
        if (secondNumber == 2)
        {
            nameBuilder.Append("++");
        }
        else
        {
            nameBuilder.Append("--");
        }
        fileName = nameBuilder.ToString();
        firstSymptom = (Symptom)Enum.Parse(typeof(Symptom), firstSymptomText);
        secondSymptom = (Symptom)Enum.Parse(typeof(Symptom), secondSymptomText);
    }

    public Symptom GetSecondSymptom()
    {
        return secondSymptom;
    }


    public MedicineClass()
    {
        firstName = "null";
        secondName = "null";
        firstSymptom = Symptom.water;
        secondSymptom = Symptom.fire;
        firstNumber = 1;
        secondNumber = 2;
        medicineImage = null;
        toolTip = "Tooltip missing";
        if(symptomIconDictionary == null)
        {
            symptomIconDictionary = new Dictionary<string, Sprite>();
        }

    }
    public MedicineClass(SpecialMedicineClass cast)
    {
        SetIndex(cast.GetIndex());
        fileName = cast.fileName;
        firstName = cast.firstName;
        secondName = cast.secondName;
        medicineImage = cast.LoadImage();
        firstSymptom = Symptom.special;
        secondSymptom = Symptom.special;
        firstNumber = 0;
        secondNumber = 0;
        toolTip = cast.toolTip;
    }

    public new Sprite LoadImage()
    {
        if(medicineImage != null)
        {
            return medicineImage;
        }
        //StringBuilder nameBuilder = new StringBuilder(firstSymptom.ToString());
        //if(firstNumber== 1)
        //{
        //    nameBuilder.Append("+");
        //}
        //else
        //{
        //    nameBuilder.Append("-");
        //}
        //nameBuilder.Append(secondSymptom.ToString());
        //if (secondNumber == 2)
        //{
        //    nameBuilder.Append("++");
        //}
        //else
        //{
        //    nameBuilder.Append("--");
        //}
        //fileName = nameBuilder.ToString();

        //nameBuilder.Append(" ");
        //nameBuilder.Append(secondName);
        if (medicineImage == null)
        {
            StringBuilder builder = new StringBuilder("Items/");
            builder.Append(fileName);
            medicineImage = Resources.Load<Sprite>(builder.ToString());
        }
        return medicineImage;
    }

    public Sprite GetIcon(string key)
    {
        
        if(!symptomIconDictionary.ContainsKey(key))
        {
            symptomIconDictionary.Add(key, Resources.Load<Sprite>("SymptomIcon/" + key));

        }
        return symptomIconDictionary[key];
    }
}
