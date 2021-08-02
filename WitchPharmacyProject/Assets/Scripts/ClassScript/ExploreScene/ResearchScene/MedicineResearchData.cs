using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class MedicineResearchData : ResearchData
{
    public string firstSymptomString;
    public string secondSymptomString;
    [System.NonSerialized]
    public Symptom firstSymptom;
    [System.NonSerialized]
    public Symptom secondSymptom;

    public MedicineResearchData()
    {
        firstSymptomString = "earth";
        secondSymptomString = "water";
        fileName = "earthwater";
        ingameName = "대지, 물 약재연구";
        explain = "대지 물 약재 연구입니다.";
        researchEndTime = 10;
        neededResearchList = new List<string>();
    }


    public Sprite LoadImage()
    {
        if (image != null)
        {
            return image;
        }

        image = Resources.Load<Sprite>("MedicineResearchImage/" + fileName);
        return image;
    }


    public void ParseSymptomString()
    {
        firstSymptom = (Symptom)Enum.Parse(typeof(Symptom), firstSymptomString);
        secondSymptom = (Symptom)Enum.Parse(typeof(Symptom), secondSymptomString);
    }
}
