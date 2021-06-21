using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicineTile : Tile
{
    public MedicineTile(int i) : base(i)
    {
        index = i;
        tileType = TileType.MedicineTile;
    }
}
