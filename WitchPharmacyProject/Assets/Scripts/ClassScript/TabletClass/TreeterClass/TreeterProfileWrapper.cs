using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeterProfileWrapper
{
    public List<TreeterProfile> treeterProfileList;

    public string LoadIngameName(string profileName)
    {
        for (int i = 0; i < treeterProfileList.Count; i++)
        {
            if (treeterProfileList[i].profileName == profileName)
            {
                return treeterProfileList[i].profileIngameName;
            }
        }

        return null;
    }

    public Sprite LoadSprite(string profileName)
    {
        for(int i = 0; i < treeterProfileList.Count; i++)
        {
            if(treeterProfileList[i].profileName == profileName)
            {
                return treeterProfileList[i].LoadSprite();
            }
        }

        return null;
    }
}
