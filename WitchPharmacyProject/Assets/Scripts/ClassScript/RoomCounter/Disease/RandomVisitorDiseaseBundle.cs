using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomVisitorDiseaseBundle
{
    public RandomVisitorDiseaseWrapper[] wrapperList;

    public RandomVisitorDiseaseBundle()
    {
        wrapperList = new RandomVisitorDiseaseWrapper[5];
    }

    public void LoadWrapper(JsonManager jsonManager)
    {
        for(int i = 0; i < 5; i++)
        {
            wrapperList[i] = jsonManager.ResourceDataLoad<RandomVisitorDiseaseWrapper>("RandomVisitorDisease/"
                + (Symptom)i);
            for(int j = 0; j < wrapperList[i].randomVisitorDiseaseArray.Length; j++)
            {
                wrapperList[i].randomVisitorDiseaseArray[j].symptom = (Symptom)i;
            }
        }
    }
}
