using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OddVisitorDialogBundle
{
    public string rewardSpecialMedicine;
    public List<OddVisitorDialog> startDialogList;
    public List<OddVisitorDialog> wrongDialogList;
    public List<OddVisitorDialog> answerDialogList;

    public OddVisitorDialogBundle()
    {
        rewardSpecialMedicine = "meltfire";
        startDialogList = new List<OddVisitorDialog>();
        startDialogList.Add(new OddVisitorDialog());
        startDialogList.Add(new OddVisitorDialog());
        startDialogList.Add(new OddVisitorDialog());

        wrongDialogList = new List<OddVisitorDialog>();
        wrongDialogList.Add(new OddVisitorDialog());
        wrongDialogList.Add(new OddVisitorDialog());
        wrongDialogList.Add(new OddVisitorDialog());

        answerDialogList = new List<OddVisitorDialog>();
        answerDialogList.Add(new OddVisitorDialog());
        answerDialogList.Add(new OddVisitorDialog());
        answerDialogList.Add(new OddVisitorDialog());


    }
}
