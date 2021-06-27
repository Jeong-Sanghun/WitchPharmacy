using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTile : Tile
{
    public bool boxOpened;
    public int clickedTime;
    public TrapTile(int i) : base(i)
    {
        index = i;
        boxOpened = false;
        tileType = TileType.TrapTile;
        clickedTime = 0;

    }
}
