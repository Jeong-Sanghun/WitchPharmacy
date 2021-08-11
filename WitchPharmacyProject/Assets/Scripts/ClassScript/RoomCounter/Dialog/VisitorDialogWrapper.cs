using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisitorDialogWrapper
{
    public DialogFX dialogFX;
    public bool forceEnd;
    public string characterName;
    public string characterFeeling;
    public List<VisitorDialog> dialogList;

    public VisitorDialogWrapper()
    {
        dialogFX = DialogFX.Null;
        characterFeeling = "nothing";
        characterName = null;
        forceEnd = false;
        dialogList = new List<VisitorDialog>();
    }
}
