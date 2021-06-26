using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

[System.Serializable]
public class StoreToolClass
{
    public int index;
    public string name;
    public string toolTip;
    public int cost;
    //1회용
    public bool usedOnce;
    public bool usedForExplore;
    public bool isQuestMedicine;
    public Sprite toolImage;


    public Sprite LoadImage()
    {
        if (toolImage != null)
        {
            return toolImage;
        }
        StringBuilder nameBuilder = new StringBuilder(name);
        if (toolImage == null)
        {
            StringBuilder builder = new StringBuilder("Tools/");
            builder.Append(nameBuilder.ToString());
            toolImage = Resources.Load<Sprite>(builder.ToString());
        }
        return toolImage;
    }
}
