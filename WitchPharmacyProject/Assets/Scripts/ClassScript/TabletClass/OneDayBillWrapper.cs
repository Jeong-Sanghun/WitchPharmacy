using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OneDayBillWrapper
{
    public int wholeGainCoin;
    public int wholeSpentCoin;
    public List<OneBillClass> billList;

    public OneDayBillWrapper()
    {
        wholeGainCoin = 0;
        wholeSpentCoin = 0;
        billList = new List<OneBillClass>();
        billList.Add(new OneBillClass());
    }
}
