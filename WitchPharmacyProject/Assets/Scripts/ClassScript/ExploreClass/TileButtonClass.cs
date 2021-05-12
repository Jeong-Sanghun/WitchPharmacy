using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    StartTile, MedicineTile, StoreTile, StoryTile, 
    TreasureTile, TrapTile, BarrierTile, SpecialStoreTile
}

public enum Edge
{
    Up,Down,Left,Right
}

public class TileButtonClass
{
    static int edgeNumber = 4;
    public GameObject tileButtonObject;
    public TileButtonClass[] adjacentTileArray;
    public Tile tileClass;
    public bool[] adjacentBoolArray;
    public int[] adjacentCostArray;
    public bool isStartTile;
    public bool isSetUp;

    public TileButtonClass(GameObject button, Tile tile,bool start)
    {
        tileButtonObject = button;
        adjacentTileArray = new TileButtonClass[edgeNumber];
        tileClass = tile;
        adjacentBoolArray = new bool[edgeNumber];
        for(int i = 0; i < edgeNumber; i++)
        {
            adjacentBoolArray[i] = false;
        }

        adjacentCostArray = new int[edgeNumber];
        for (int i = 0; i < edgeNumber; i++)
        {
            adjacentCostArray[i] = 0;
        }

        isStartTile = start;
        isSetUp = start;

    }

    //디버그용
    public TileButtonClass(bool start)
    {
        tileButtonObject = new GameObject();
        adjacentTileArray = new TileButtonClass[edgeNumber];
        adjacentBoolArray = new bool[edgeNumber];
        for (int i = 0; i < edgeNumber; i++)
        {
            adjacentBoolArray[i] = false;
        }

        adjacentCostArray = new int[edgeNumber];
        for (int i = 0; i < edgeNumber; i++)
        {
            adjacentCostArray[i] = 0;
        }

        isStartTile = start;
    }

    public void SetEdge(TileButtonClass otherTileButton, int cost, Edge edgeWay)
    {
        adjacentTileArray[(int)edgeWay] = otherTileButton;
        adjacentCostArray[(int)edgeWay] = cost;
        adjacentBoolArray[(int)edgeWay] = true;
        Edge oppositeWay;
        if ((int)edgeWay % 2 == 0)
        {
            oppositeWay = edgeWay + 1;
        }
        else
        {
            oppositeWay = edgeWay - 1;
        }
        otherTileButton.adjacentTileArray[(int)oppositeWay] = this;
        otherTileButton.adjacentCostArray[(int)oppositeWay] = cost;
        otherTileButton.adjacentBoolArray[(int)oppositeWay] = true;

    }


}
