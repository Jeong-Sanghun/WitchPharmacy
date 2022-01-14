using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class BossDataWrapper
{
    public List<BossData> bossDataList;

    public BossDataWrapper()
    {
        bossDataList = new List<BossData>();
    }
}
