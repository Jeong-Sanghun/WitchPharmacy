using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

[System.Serializable]
public class StoreToolClass
{
    int index;
    public string fileName;
    public string name;
    public string toolTip;
    public int cost;
    //1회용
    public bool usedOnce;
    public bool usedForExplore;
    public bool isQuestMedicine;

    
    Sprite toolImage;

    public int GetIndex()
    {
        return index;
    }

    public void SetIndex(int _index)
    {
        index = _index;
    }


    public Sprite LoadImage()
    {
        if (toolImage != null)
        {
            return toolImage;
        }
        StringBuilder nameBuilder = new StringBuilder(fileName);
        if (toolImage == null)
        {
            StringBuilder builder = new StringBuilder("Tools/");
            builder.Append(nameBuilder.ToString());
            toolImage = Resources.Load<Sprite>(builder.ToString());
        }
        return toolImage;
    }
}
