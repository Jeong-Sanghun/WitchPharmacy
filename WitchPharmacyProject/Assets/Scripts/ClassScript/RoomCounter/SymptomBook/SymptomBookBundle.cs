using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class SymptomBookBundle
{
    public string symptomString;
    [System.NonSerialized]
    public Symptom symptom;
    public List<SymptomBook> oneSymptomBookList;
    public List<SymptomBook> twoSymptomBookList;

    public SymptomBookBundle()
    {
        symptomString = "water";
        symptom = Symptom.water;
        oneSymptomBookList = new List<SymptomBook>();
        oneSymptomBookList.Add(new SymptomBook());
        oneSymptomBookList.Add(new SymptomBook());
        oneSymptomBookList.Add(new SymptomBook());

        twoSymptomBookList = new List<SymptomBook>();

        twoSymptomBookList.Add(new SymptomBook());
        twoSymptomBookList.Add(new SymptomBook());
        twoSymptomBookList.Add(new SymptomBook());
    }

    public void ParseSymptomString()
    {
        symptom = (Symptom)Enum.Parse(typeof(Symptom), symptomString);
    }
}
