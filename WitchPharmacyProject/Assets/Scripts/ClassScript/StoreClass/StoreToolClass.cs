using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

[System.Serializable]
public class StoreToolClass
{
    public string name;
    public string toolTip;
    public int cost;
    //1회용
    public bool usedOnce;
    public Sprite toolImage;

    public void LoadImage()
    {
        if (toolImage != null)
        {
            return;
        }
        StringBuilder nameBuilder = new StringBuilder(name);
        if (toolImage == null)
        {
            StringBuilder builder = new StringBuilder("Tools/");
            builder.Append(nameBuilder.ToString());
            toolImage = Resources.Load<Sprite>(builder.ToString());
        }
    }
}
