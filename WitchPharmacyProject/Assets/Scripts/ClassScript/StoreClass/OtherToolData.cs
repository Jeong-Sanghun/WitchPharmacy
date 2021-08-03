using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OtherToolData
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

        toolImage = Resources.Load<Sprite>("OtherToolImage/" + fileName);
        return toolImage;
    }

    public OtherToolData()
    {
        fileName = "improvement";
        ingameName = "연구 설비 개선";
        toolTip = "이거만 있으면 돼~";
        cost = 100;
    }
}
