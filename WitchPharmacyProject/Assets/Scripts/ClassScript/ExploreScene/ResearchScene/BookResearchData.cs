using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class BookResearchData : ResearchData
{
    public string symptomString;
    [System.NonSerialized]
    public Symptom symptom;


    public BookResearchData()
    {
        
        fileName = "water";
        symptomString = fileName;
        ingameName = "물 증상 책자";
        explain = "물이물물";
        researchEndTime = 2;
        neededResearchList = new List<string>();
    }

    public override Sprite LoadImage()
    {
        if (image != null)
        {
            return image;
        }

        image = Resources.Load<Sprite>("BookImage/" + fileName);
        return image;
    }

    public void ParseSymptomString()
    {
        symptom = (Symptom)Enum.Parse(typeof(Symptom), symptomString);
    }

}
