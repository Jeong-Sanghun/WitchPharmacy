using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SaveTime
{
     ExploreStart, DayStart
}
[System.Serializable]
public class SaveDataTime
{
    public int day;
    public int sceneIndex;

    public SaveDataTime()
    {
        day = -1;
        sceneIndex = 0;
    }
}
