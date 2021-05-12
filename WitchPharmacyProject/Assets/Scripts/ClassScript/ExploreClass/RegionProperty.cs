using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//json
[System.Serializable]
public class RegionProperty
{
    public int index;
    public string regionName;
    //이거 상당히 실험적임. Tile로 받아오는거.
    public List<int> tileTypeList;
    public RegionAvailableMedicine regionAvailableMedicine;
    
    public RegionProperty()
    {
        index = 0;
        regionName = "testRegion";
        tileTypeList = new List<int>();
        tileTypeList.Add(0);
        for (int i = 1; i<10; i++)
        {
            tileTypeList.Add(Random.Range(1, 8));
        }
        regionAvailableMedicine = new RegionAvailableMedicine();
    }


}
