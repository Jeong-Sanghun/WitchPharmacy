using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpecialVisitorCondition
{
    public int appearingSequence;
    public string bundleName;
    //public string specialMedicine;
    public bool isOdd;
    public int appearingDay;


    public SpecialVisitorCondition()
    {
        appearingSequence = 0;
        bundleName = "Lily";
        appearingDay = 0;
        //appearingQuestBundleList = new List<string>();
        //appearingQuestBundleList.Add("none");
    }

    //[System.NonSerialized]
    //public List<string> storyGainConditionList;
    //[System.NonSerialized]
    //public List<string> documentGainConditionList;
    //[System.NonSerialized]
    //public List<string> questGainConditionList;
    //[System.NonSerialized]
    //public List<string> regionGainConditionList;

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
