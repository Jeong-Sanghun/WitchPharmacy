using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RandomVisitorDiseaseDialogWrapper
{
    public RandomVisitorDiseaseDialogBundle[] diseaseDialogBundleArray;

    public RandomVisitorDiseaseDialogWrapper()
    {
        //증상마다 하나씩 있음
        diseaseDialogBundleArray = new RandomVisitorDiseaseDialogBundle[5];

        for(int i = 0; i < 5; i++)
        {
            diseaseDialogBundleArray[i] = new RandomVisitorDiseaseDialogBundle();
        }
    }
}
