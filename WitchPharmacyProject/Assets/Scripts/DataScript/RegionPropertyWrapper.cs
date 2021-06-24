using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//json
[System.Serializable]
public class RegionPropertyWrapper
{
    public RegionProperty[] regionPropertyArray;

    //dumy생성자
    public RegionPropertyWrapper()
    {
        regionPropertyArray = new RegionProperty[10];
        for(int i = 0; i< regionPropertyArray.Length; i++)
        {
            regionPropertyArray[i] = new RegionProperty(i);
        }
    }
}
