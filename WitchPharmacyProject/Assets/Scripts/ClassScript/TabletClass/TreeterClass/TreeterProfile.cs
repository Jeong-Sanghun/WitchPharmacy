using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

[System.Serializable]
public class TreeterProfile
{
    public string profileName;
    public string profileIngameName;
    [System.NonSerialized]
    Sprite profileSprite;

    public TreeterProfile()
    {
        profileName = null;
        profileIngameName = null;
        profileSprite = null;

    }

    public Sprite LoadSprite()
    {
        if (profileSprite != null)
        {
            return profileSprite;
        }
        StringBuilder builder = new StringBuilder("TreeterSprite/Profile/");
        builder.Append(profileName);
        profileSprite = Resources.Load<Sprite>(builder.ToString());
        return profileSprite;
    }
}
