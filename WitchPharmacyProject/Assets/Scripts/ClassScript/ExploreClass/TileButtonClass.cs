using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    StartTile, MedicineTile, StoreTile, StoryTile, TreasureTile, TrapTile, BarrierTile, SpecialStoreTile
}

public enum Edge
{
    Up,Down,Left,Right
}

public class TileButtonClass
{
    public int tileIndex;
    public GameObject tileButtonObject;
    public TileButtonClass[] adjacentTileArray;
    public Tile nowTile;
    public bool[] adjacentBoolArray;
    public int[] adjacentCostArray;
    public bool isStartTile;

}
