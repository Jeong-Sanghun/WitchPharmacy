using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum CariFeeling
{
    angry1,angry2,doubting,grin,hugesmile,notTalk,surprised,talk
}

[System.Serializable]
public class CariDialog
{
    public string feelingString;
    public string dialog;

    [System.NonSerialized]
    public CariFeeling feeling;

    public CariDialog()
    {
        feelingString = null;
        dialog = null;
        feeling = CariFeeling.notTalk;
    }

    public virtual void Parse()
    {
        if(feelingString != null)
            feeling = (CariFeeling)Enum.Parse(typeof(CariFeeling), feelingString);
    }

}
