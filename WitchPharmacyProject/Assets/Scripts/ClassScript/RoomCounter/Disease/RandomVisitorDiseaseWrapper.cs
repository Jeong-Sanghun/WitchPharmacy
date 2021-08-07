using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RandomVisitorDiseaseWrapper
{
    public RandomVisitorDisease[] randomVisitorDiseaseArray;

    public RandomVisitorDiseaseWrapper()
    {
        randomVisitorDiseaseArray = new RandomVisitorDisease[16];
        for(int i = 0; i < 16; i++)
        {
            randomVisitorDiseaseArray[i] = new RandomVisitorDisease();
        }
    }
}
