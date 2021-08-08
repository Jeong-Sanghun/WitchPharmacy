using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

[System.Serializable]
public class TreeterData
{
    public int index;
    public string fileName;
    public string titleIngameText;
    public string dialog;
    public string profileFileName;
    public string profileIngameName;
    public int likeNumber;

    public List<TreeterCommentData> commentDataList;

    Sprite treeterSprite;
    Sprite profileSprite;

    public TreeterData()
    {
        index = 0;
        fileName = null;
        titleIngameText = null;
        dialog = null;
        profileFileName = null;
        profileIngameName = null;
        likeNumber = 0;
        commentDataList = new List<TreeterCommentData>();
        commentDataList.Add(new TreeterCommentData());
        commentDataList.Add(new TreeterCommentData());
        commentDataList.Add(new TreeterCommentData());
    }

    public Sprite LoadSprite(bool isProfile)
    {
        Sprite nowSprite = null;
        string path = null;
        string name = null;
        if (isProfile)
        {
            nowSprite = profileSprite;
            name = profileFileName;
            path = "Profile/";
        }
        else
        {
            nowSprite = treeterSprite;
            name = fileName;
            path = "Treeter/";
        }
        if (nowSprite != null)
        {
            return nowSprite;
        }
        StringBuilder builder = new StringBuilder("TreeterSprite/");
        builder.Append(path);
        builder.Append(name);
        nowSprite = Resources.Load<Sprite>(builder.ToString());
        return nowSprite;
    }

}
