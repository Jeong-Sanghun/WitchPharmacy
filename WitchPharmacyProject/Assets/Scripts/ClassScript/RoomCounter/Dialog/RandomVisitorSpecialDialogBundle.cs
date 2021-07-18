using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RandomVisitorSpecialDialogBundle
{
    public string rewardSpecialMedicine;
    public List<RandomVisitorSpecialDialog> dialogList;

    public RandomVisitorSpecialDialogBundle()
    {
        rewardSpecialMedicine = "meltfire";
    }
}
