using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[System.Serializable]
public class SaveDataTimeWrapper
{
    public SaveDataTime[] saveDataTimeList;

    public SaveDataTimeWrapper()
    {
        saveDataTimeList = new SaveDataTime[4];

        for(int i = 0; i < 4; i++)
        {
            saveDataTimeList[i] = new SaveDataTime();
        }
    }
}
