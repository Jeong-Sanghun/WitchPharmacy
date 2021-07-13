using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BillReason
{
   medicineSell,medicineBuy, toolBuy, mainteneceFee
}

[System.Serializable]
public class OneBillClass
{
    public bool isPlus;
    //이거 랭귀지 팩에서 받아올거임.
    public BillReason reason;
    public int changedCoin;

    public OneBillClass()
    {
        isPlus = true;
        reason = BillReason.medicineBuy;
        changedCoin = 0;
    }
}
