using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpecialVisitorDialog
{
    public bool ruelliaTalking;
    public string characterFeeling;
    public string dialog;


    public SpecialVisitorDialog()
    {
        ruelliaTalking = false;
        characterFeeling = "nothing";
        dialog = "다이얼로그";
    }
}
