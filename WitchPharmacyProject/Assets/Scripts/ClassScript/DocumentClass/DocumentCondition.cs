using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

[System.Serializable]
public class DocumentCondition
{
    [System.NonSerialized]
    public int index;
    public string folderIngameName;
    public string folderFileName;

    public string fileName;
    public string ingameName;
    public bool printSprite;
    
    public string storyGainCondition;
    public string questGainCondition;
    public string documentGainCondition;
    public string regionCondition;

    Sprite documentSprite;

    //[System.NonSerialized]
    //public List<string> storyGainConditionList;
    //[System.NonSerialized]
    //public List<string> documentGainConditionList;
    //[System.NonSerialized]
    //public List<string> questGainConditionList;
    //[System.NonSerialized]
    //public List<string> regionGainConditionList;

    public DocumentCondition()
    {
        index = 0;
        fileName = "testDocument";
        ingameName = "테스투";
        storyGainCondition = null;
        questGainCondition = "Lily";
        documentGainCondition = null;
        printSprite = true;

    }


    public Sprite LoadSprite()
    {
        if (documentSprite != null)
        {
            return documentSprite;
        }
        StringBuilder builder = new StringBuilder("DocumentSprite/");
        builder.Append(fileName);
        documentSprite = Resources.Load<Sprite>(builder.ToString());
        return documentSprite;
    }


    //public void ConditionStringParse()
    //{
    //    storyGainConditionList = new List<string>();
    //    documentGainConditionList = new List<string>();
    //    questGainConditionList = new List<string>();
    //    regionGainConditionList = new List<string>();

    //    StringBuilder builder = new StringBuilder();
    //    for(int i = 0; i < regionCondition.Length; i++)
    //    {
    //        if(regionCondition[i] == '/')
    //        {
    //            regionGainConditionList.Add(builder.ToString());
    //            builder = new StringBuilder();
    //        }
    //        else if(i == regionCondition.Length - 1)
    //        {
    //            builder.Append(regionCondition[i]);
    //            regionGainConditionList.Add(builder.ToString());
    //            builder = new StringBuilder();
    //        }
    //        else
    //        {
    //            builder.Append(regionCondition[i]);
    //        }
    //    }
    //    for (int i = 0; i < documentGainCondition.Length; i++)
    //    {
    //        if (documentGainCondition[i] == '/' )
    //        {
    //            documentGainConditionList.Add(builder.ToString());
    //            Debug.Log(builder.ToString());
    //            builder = new StringBuilder();
    //        }else if (i == documentGainCondition.Length - 1)
    //        {
    //            builder.Append(documentGainCondition[i]);
    //            documentGainConditionList.Add(builder.ToString());
    //            Debug.Log(builder.ToString());
    //            builder = new StringBuilder();
    //        }
    //        else
    //        {
    //            builder.Append(documentGainCondition[i]);
    //        }
    //    }
    //    for (int i = 0; i < questGainCondition.Length; i++)
    //    {
    //        if (questGainCondition[i] == '/')
    //        {
    //            questGainConditionList.Add(builder.ToString());
    //            builder = new StringBuilder();
    //        }
    //        else if(i == questGainCondition.Length - 1)
    //        {
    //            builder.Append(questGainCondition[i]);
    //            questGainConditionList.Add(builder.ToString());
             
    //            builder = new StringBuilder();
    //        }
    //        else
    //        {
    //            builder.Append(questGainCondition[i]);
    //        }
    //    }
    //    for (int i = 0; i < storyGainCondition.Length; i++)
    //    {
    //        if (storyGainCondition[i] == '/')
    //        {
    //            storyGainConditionList.Add(builder.ToString());
    //            Debug.Log(builder.ToString());
    //            builder = new StringBuilder();
    //        }else if (i == storyGainCondition.Length - 1)
    //        {
    //            builder.Append(storyGainCondition[i]);
    //            storyGainConditionList.Add(builder.ToString());
    //            Debug.Log(builder.ToString());
    //            builder = new StringBuilder();
    //        }
    //        else
    //        {
    //            builder.Append(storyGainCondition[i]);
    //        }
    //    }
    //}
}
