using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymptomParts
{
    public int[] symptomIndex;

    public SymptomParts()
    {
        symptomIndex = new int[5];
        for(int  i = 0; i < symptomIndex.Length; i++)
        {
            symptomIndex[i] = -1;
        }
    }
}
