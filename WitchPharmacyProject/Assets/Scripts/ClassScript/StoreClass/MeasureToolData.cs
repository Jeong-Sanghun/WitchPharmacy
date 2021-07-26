using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MeasureToolData
{
    public string fileName;
    public string ingameName;
    public string toolTip;
    public int cost;
    Sprite toolImage;

    public Sprite LoadImage()
    {
        if (toolImage != null)
        {
            return toolImage;
        }

        toolImage = Resources.Load<Sprite>("MeasureToolImage/" + fileName);
        return toolImage;
    }

    public MeasureToolData()
    {
        fileName = "water";
        ingameName = "물 측정도구";
        toolTip = "물이얏~~~~";
        cost = 100;
    }
}
