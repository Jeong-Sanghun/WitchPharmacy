using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MainCariDialogWrapper
{
    public MainCariDialog[] mainCariDialogArray;

    public void Parse()
    {
        for(int i=0;i< mainCariDialogArray.Length; i++)
        {
            mainCariDialogArray[i].Parse();
        }
    }
}
