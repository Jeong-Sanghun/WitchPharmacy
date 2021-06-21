using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//json
[System.Serializable]
public class TreasureTile : Tile
{
    public TreasureTile(int i) : base(i)
    {
        index = i;
        tileType = TileType.TreasureTile;
    }
}
