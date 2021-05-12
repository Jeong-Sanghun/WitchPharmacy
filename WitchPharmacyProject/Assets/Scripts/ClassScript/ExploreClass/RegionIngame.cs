using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionIngame
{
    public RegionProperty regionProperty;
    public List<TileButtonClass> tileButtonList;

    public RegionIngame()
    {
        regionProperty = new RegionProperty();
        tileButtonList = new List<TileButtonClass>();
        tileButtonList.Add(new TileButtonClass(true));
        for (int i = 0; i < 10; i++)
        {
            tileButtonList.Add(new TileButtonClass(false));
        }
    }

    //dumy생성자. 나중에 제이슨으로 받아올거임.
    public RegionIngame(GameObject button,Transform canvas)
    {
        regionProperty = new RegionProperty();
        tileButtonList = new List<TileButtonClass>();
        GameObject inst = GameObject.Instantiate(button,canvas);
        Tile tile = new StartTile(0);
        tileButtonList.Add(new TileButtonClass(inst,tile,true));
        for (int i = 1; i < 10; i++)
        {
            inst = GameObject.Instantiate(button, canvas);
            switch (regionProperty.tileTypeList[i])
            {
                //StartTile, MedicineTile, StoreTile, StoryTile, TreasureTile, TrapTile, BarrierTile, SpecialStoreTile
                case 1:
                    tile = new MedicineTile(i);
                    break;
                case 2:
                    tile = new StoreTile(i);
                    break;
                case 3:
                    tile = new StoryTile(i);
                    break;
                case 4:
                    tile = new TreasureTile(i);
                    break;
                case 5:
                    tile = new TrapTile(i);
                    break;
                case 6:
                    tile = new BarrierTile(i);
                    break;
                case 7:
                    tile = new SpecialStoreTile(i);
                    break;
                default:
                    tile = new MedicineTile(i);
                    break;
            }
            tileButtonList.Add(new TileButtonClass(inst, tile, false));
        }
    }
}
