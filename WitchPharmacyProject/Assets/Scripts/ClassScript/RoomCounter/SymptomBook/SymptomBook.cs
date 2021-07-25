using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;

[System.Serializable]
public class SymptomBook
{
    //public string symptomString;
    //[System.NonSerialized]
    //public Symptom symptom;
    public int symptomAmountNumber;
    public int symptomIndexNumber;

    public string title;

    [System.NonSerialized]
    public Sprite symptomSprite;

    public string explain;

    public SymptomBook()
    {
        //symptomString = "water";
        //symptom = Symptom.water;
        symptomAmountNumber = 1;
        symptomIndexNumber = 0;
    }

    //public void ParseSymptomString()
    //{
    //    symptom = (Symptom)Enum.Parse(typeof(Symptom), symptomString);
    //}

    public Sprite LoadImage(string symptomString)
    {
        if(symptomSprite != null)
        {
            return symptomSprite;
        }
        StringBuilder builder = new StringBuilder("SymptomBook/");
        builder.Append(symptomString);
        switch (symptomAmountNumber)
        {
            case -2:
                builder.Append("--");
                break;
            case -1:
                builder.Append("-");
                break;
            case 1:
                builder.Append("+");
                break;
            case 2:
                builder.Append("++");
                break;
        }
        builder.Append(symptomIndexNumber.ToString());
        symptomSprite = Resources.Load<Sprite>(builder.ToString());
        return symptomSprite;
    }

}
