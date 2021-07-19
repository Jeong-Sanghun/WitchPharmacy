using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OddVisitorDialogBundle
{
    public string rewardSpecialMedicine;
    public List<OddVisitorDialog> dialogList;

    public OddVisitorDialogBundle()
    {
        rewardSpecialMedicine = "meltfire";
        dialogList = new List<OddVisitorDialog>();
        dialogList.Add(new OddVisitorDialog());
        dialogList.Add(new OddVisitorDialog());
        dialogList.Add(new OddVisitorDialog());
    }
}
