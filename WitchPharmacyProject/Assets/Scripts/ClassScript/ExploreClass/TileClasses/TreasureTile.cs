﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//json
[System.Serializable]
public class TreasureTile : Tile
{
    public bool boxOpened;
    public TreasureTile(int i) : base(i)
    {
        index = i;
        boxOpened = false;
        tileType = TileType.TreasureTile;
    }
}
