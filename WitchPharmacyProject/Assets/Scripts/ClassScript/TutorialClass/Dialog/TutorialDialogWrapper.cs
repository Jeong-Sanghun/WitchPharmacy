using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TutorialDialogWrapper
{
    public TutorialDialog[] dialogArray;
    
    public TutorialDialogWrapper()

    {
        
    }

    public void Parse()
    {
        for(int i = 0; i < dialogArray.Length; i++)
        {
            dialogArray[i].Parse();
        }
    }
}
