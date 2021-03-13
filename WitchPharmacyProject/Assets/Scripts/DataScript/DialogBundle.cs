using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogBundle
{
    public string[] dialogArray;
    //이거 4개로 고정할거임

    public DialogBundle()
    {
        dialogArray = new string[4];
        for (int i = 0; i < dialogArray.Length; i++)
        {
            dialogArray[i] = (i - 2).ToString() + "번째 증상";
        }
    }
}
