using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTile : Tile
{

    public TrapTile(int i) : base(i)
    {
        index = i;
        tileType = TileType.TrapTile;
        

    }
}
