using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisitorDialogWrapper
{
    public DialogFX dialogFX;
    public bool forceEnd;
    public bool giveCoin;
    public int coin;
    public string characterName;
    public string characterFeeling;
    public List<VisitorDialog> dialogList;

    public VisitorDialogWrapper()
    {
        dialogFX = DialogFX.Null;
        characterFeeling = "nothing";
        characterName = null;
        forceEnd = false;
        giveCoin = false;
        coin = 0;
        dialogList = new List<VisitorDialog>();
    }
}
