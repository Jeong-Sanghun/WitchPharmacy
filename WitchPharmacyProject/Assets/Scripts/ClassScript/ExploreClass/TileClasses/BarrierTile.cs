using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierTile : Tile
{
    public bool isRock;
    public bool isUnlocked;
    public BarrierTile(int i) : base(i)
    {
        index = i;
        tileType = TileType.BarrierTile;
        if (Random.Range(0, 2) == 0)
        {
            isRock = true;
        }
        else
        {
            isRock = false;
        }
        isUnlocked = false;
    }
}
