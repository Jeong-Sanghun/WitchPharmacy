using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

//약 1개가 가지고 있는 약 클래스 
//게임 내에서도 쓰일거고, 제이슨 저장용으로도 쓰일것이다.
[System.Serializable]
public class MedicineClass  //SH
{
    public string firstName;
    public string secondName;
    public Symptom firstSymptom;
    public Symptom secondSymptom;
    public int firstNumber;
    public int secondNumber;

    public Sprite medicineImage;

    //public GameObject realGameobject;

    public MedicineClass()
    {
        firstName = "null";
        secondName = "null";
        firstSymptom = Symptom.water;
        secondSymptom = Symptom.fire;
        firstNumber = 1;
        secondNumber = 2;
        medicineImage = null;
    }

    public void LoadImage()
    {
        if(medicineImage != null)
        {
            return;
        }
        StringBuilder nameBuilder = new StringBuilder(firstName);
        nameBuilder.Append(" ");
        nameBuilder.Append(secondName);
        if (medicineImage == null)
        {
            StringBuilder builder = new StringBuilder("Items/");
            builder.Append(nameBuilder.ToString());
            medicineImage = Resources.Load<Sprite>(builder.ToString());
        }
    }
}
