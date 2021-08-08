using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

[System.Serializable]
public class TreeterCondition
{
    [System.NonSerialized]
    public int index;

    public string fileName;
    public bool printSprite;
    public int dayCondition;


    public TreeterCondition()
    {
        index = 0;
        fileName = "testDocument";
        printSprite = true;
        dayCondition = 0;
    }



}
