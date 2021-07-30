using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeasureToolExplain
{
    public string fileName;
    public string title;
    public string explain;
    Sprite image;

    public MeasureToolExplain()
    {
        fileName = "Fire";
        title = "측정도구(불) 사용 설명서";
        explain = "이케이케 저케저케 잘 하란 말이야~!";
    }

    public Sprite LoadImage()
    {
        if (image != null)
        {
            return image;
        }

        image = Resources.Load<Sprite>("MeasureToolExplain/" + fileName);
        return image;
    }
}
