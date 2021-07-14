using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OneBillLineClass
{
    public BillReason reason;
    public bool isPlus;
    public int coin;
    public Text oneLineReasonText;
    public Text oneLineCoinText;
    public GameObject lineObject;

    public OneBillLineClass(BillReason _reason, bool _isPlus,int _coin, Text reasonText, Text coinText, GameObject obj)
    {
        reason = _reason;
        isPlus = _isPlus;
        coin = _coin;
        oneLineReasonText = reasonText;
        oneLineCoinText = coinText;
        lineObject = obj;
    }
}
