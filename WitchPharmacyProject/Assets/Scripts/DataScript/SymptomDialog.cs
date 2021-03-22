using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//1개의 증상에서 나올 수 있는 모든 다이얼로그를 저장하는 클래스
[System.Serializable]
public class SymptomDialog  //SH
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
