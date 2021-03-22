using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//모든 증상에서 나올 수 있는 모든 다이얼로그를 저장하는 클래스
[System.Serializable]
public class SymptomDialog  //SH
{
    public DialogBundle[] symptomDialogArray;
    public string[] middleDialog;

    public SymptomDialog()
    {
        symptomDialogArray = new DialogBundle[6];
        middleDialog = new string[6];
        middleDialog[0] = ", 그리고,,,";
        middleDialog[1] = ", 아맞어 또,,,";
        middleDialog[2] = ", 더 아픈데는,,,";
        middleDialog[3] = ", 다음으로는,,,";
        middleDialog[4] = ", 또한,,,";
        middleDialog[5] = ", 그리구요,,,";

        for (int i = 0; i < symptomDialogArray.Length; i++)
        {
            symptomDialogArray[i] = new DialogBundle();
            
        }

    }
}
