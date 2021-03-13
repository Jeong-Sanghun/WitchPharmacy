using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SymptomDialog
{
    public DialogBundle[] symptomDialogArray;
    public string[] middleDialog;

    public SymptomDialog()
    {
        symptomDialogArray = new DialogBundle[6];
        middleDialog = new string[6];
        
        for(int i = 0; i < symptomDialogArray.Length; i++)
        {
            symptomDialogArray[i] = new DialogBundle();
            middleDialog[i] = i.ToString() + ", 그리고,,,";
        }

    }
}
