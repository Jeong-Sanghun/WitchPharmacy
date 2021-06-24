using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileButtonAdjacent
{
    public TileButtonClass adjacentTileButton;
    public bool isOpened;

    public TileButtonAdjacent(TileButtonClass tile)
    {
        isOpened = false;
        adjacentTileButton = tile;
    }
}
