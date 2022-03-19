using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

[System.Serializable]
public class DocumentBundle
{
    [System.NonSerialized]
    public int index;
    public string name;
    public string document;
    public string cariTalk;
    Sprite documentSprite;

    public DocumentBundle()
    {
        index = 0;
        cariTalk = "애애앵";

    }

    public Sprite LoadSprite()
    {
        if (documentSprite != null)
        {
            return documentSprite;
        }
        StringBuilder builder = new StringBuilder("DocumentSprite/");
        builder.Append(name);
        documentSprite = Resources.Load<Sprite>(builder.ToString());
        return documentSprite;
    }

}
