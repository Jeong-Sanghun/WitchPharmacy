using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierTile : Tile
{
    public BarrierTile(int i) : base(i)
    {
        index = i;
        tileType = TileType.BarrierTile;
    }
}
