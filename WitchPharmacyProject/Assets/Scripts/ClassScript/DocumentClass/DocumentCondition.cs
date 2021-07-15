using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DocumentCondition
{
    [System.NonSerialized]
    public int index;
    public string folderIngameName;
    public string folderFileName;

    public string fileName;
    public string ingameName;
    
    public string storyGainCondition;
    public string questGainCondition;
    public string documentGainCondition;
    public string regionCondition;

    [System.NonSerialized]
    public List<string> storyGainConditionList;
    [System.NonSerialized]
    public List<string> documentGainConditionList;
    [System.NonSerialized]
    public List<string> questGainConditionList;
    [System.NonSerialized]
    public List<string> regionGainConditionList;

    public DocumentCondition()
    {
        index = 0;
        fileName = "testDocument";
        ingameName = "테스투";
        storyGainCondition = null;
        questGainCondition = "Lily";
        documentGainCondition = null;

        storyGainConditionList = new List<string>();
        documentGainConditionList = new List<string>();
        questGainConditionList = new List<string>();
        regionGainConditionList = new List<string>();
    }
}
