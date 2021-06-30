using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RandomVisitorEndDialogWrapper
{
    public List<RandomVisitorEndDialog> rightDialogList;
    public List<RandomVisitorEndDialog> wrongDialogList;

    public RandomVisitorEndDialogWrapper()
    {
        rightDialogList = new List<RandomVisitorEndDialog>();
        wrongDialogList = new List<RandomVisitorEndDialog>();

        RandomVisitorEndDialog inst = new RandomVisitorEndDialog(1);
        rightDialogList.Add(inst);
        inst = new RandomVisitorEndDialog(3);
        rightDialogList.Add(inst);

        inst = new RandomVisitorEndDialog(0);
        wrongDialogList.Add(inst);
        inst = new RandomVisitorEndDialog(2);
        wrongDialogList.Add(inst);

    }

}
