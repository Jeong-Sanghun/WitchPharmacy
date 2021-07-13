using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StartDialogClassWrapper
{
    public List<StartDialogClass> randomDialogList;

    public StartDialogClassWrapper()
    {
        randomDialogList = new List<StartDialogClass>();
        //StartDialogClass dialog = new StartDialogClass(0);
        //randomDialogList.Add(dialog);
        //StartDialogClass dialog1 = new StartDialogClass(1);
        //randomDialogList.Add(dialog1);
    }

}
