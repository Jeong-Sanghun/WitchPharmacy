using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TreasureType
{
    Story, Coin, Pickaxe, Shovel
}
//json
[System.Serializable]
public class TreasureTile : Tile
{
    public bool boxOpened;
    public int clickedTime;
    public TreasureType treasureType;
    public TreasureTile(int i) : base(i)
    {
        index = i;
        boxOpened = false;
        tileType = TileType.TreasureTile;
        clickedTime = 0;
    }
}
