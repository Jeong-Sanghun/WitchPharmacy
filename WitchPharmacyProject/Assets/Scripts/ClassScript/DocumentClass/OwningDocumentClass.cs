using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OwningDocumentClass
{
    public string name;
    public string gainedRegion;
    public int gainedDay;
    public float gainedTime;

    public OwningDocumentClass()
    {
        name = null;
        gainedRegion = null;
        gainedDay = 0;
        gainedTime = 0;
    }
}
