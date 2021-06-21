using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StoreTile : Tile
{
    public StoreTile(int i) : base(i)
    {
        index = i;
        tileType = TileType.StoreTile;
    }
}
