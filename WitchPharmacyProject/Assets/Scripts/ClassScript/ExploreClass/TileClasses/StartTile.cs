using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTile : Tile
{
    public StartTile(int i) : base(i)
    {
        index = i;
        tileType = TileType.StartTile;
    }
}
