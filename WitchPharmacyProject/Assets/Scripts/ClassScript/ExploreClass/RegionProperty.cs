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
    public List<Tile> tileList;
    public RegionAvailableMedicine regionAvailableMedicine;
    
    public RegionProperty()
    {
        index = 0;
        regionName = "testRegion";
        tileList = new List<Tile>();
        for(int i = 0; i<10; i++)
        {
            tileList.Add(new Tile());
        }
        regionAvailableMedicine = new RegionAvailableMedicine();
    }


}
