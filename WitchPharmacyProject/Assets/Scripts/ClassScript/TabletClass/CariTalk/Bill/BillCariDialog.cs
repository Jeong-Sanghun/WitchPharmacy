using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BillCariDialog : CariDialog
{
    public int coinThreshold;

    public BillCariDialog()
    {
        coinThreshold = 0;
    }
}
