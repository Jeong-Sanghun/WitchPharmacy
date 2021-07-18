using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[System.Serializable]
public class SaveDataTimeWrapper
{
    public SaveDataTime[] saveDataTimeList;
    public string nowLanguageDirectory;
    public SaveDataTimeWrapper()
    {
        saveDataTimeList = new SaveDataTime[4];
        nowLanguageDirectory = "Korean/";
        for(int i = 0; i < 4; i++)
        {
            saveDataTimeList[i] = new SaveDataTime();
        }
    }
}
