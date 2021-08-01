using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

[System.Serializable]
public class SpecialMedicineClass
{
    int index;
    [System.NonSerialized]
    public string fileName;
    public string firstName;
    public string secondName;
    public string toolTip;
    public int cost;
    protected Sprite medicineImage;

    public void SetIndex(int num)
    {
        index = num;
    }

    public int GetIndex()
    {
        return index;
    }


    public Sprite LoadImage()
    {
        if (medicineImage != null)
        {
            return medicineImage;
        }
        StringBuilder nameBuilder = new StringBuilder(fileName);

        //nameBuilder.Append(" ");
        //nameBuilder.Append(secondName);
        if (medicineImage == null)
        {
            StringBuilder builder = new StringBuilder("SpecialItems/");
            builder.Append(nameBuilder.ToString());
            medicineImage = Resources.Load<Sprite>(builder.ToString());
        }
        return medicineImage;
    }

    public SpecialMedicineClass()
    {
        index = 0;
        fileName = "파일";
        firstName = "퍼스트";
        secondName = "세컨드";
        toolTip = "툴팁";
        cost = 3;
    }
}
