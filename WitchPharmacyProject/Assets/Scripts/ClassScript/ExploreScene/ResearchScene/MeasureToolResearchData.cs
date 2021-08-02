using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class MeasureToolResearchData : ResearchData
{

    public string symptomString;
    [NonSerialized]
    public Symptom symptom;


    public MeasureToolResearchData()
    {
        fileName = "water";
        ingameName = "측정도구(물)";
        symptomString = "water";
        explain = "도구설명";
        researchEndTime = 10;
        neededResearchList = new List<string>();
    }

    public Sprite LoadImage()
    {
        if(image!= null)
        {
            return image;
        }

        image = Resources.Load<Sprite>("MeasureToolImage/" + fileName);
        return image;
    }

    public void ParseSymptomString()
    {
        symptom = (Symptom)Enum.Parse(typeof(Symptom), symptomString);
    }
}
