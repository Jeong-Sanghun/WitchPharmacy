using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    StartTile, MedicineTile, StoreTile, StoryTile, 
    TreasureTile, TrapTile, BarrierTile, SpecialStoreTile
}

/*
public enum Edge
{
    Up,Down,Left,Right ,UpLeft, DownRight, UpRight, DownLeft
}*/
public class TileButtonClass
{
    public static int edgeNumber = 8;
    public static int maxCost = 4;
    public GameObject tileButtonObject;

    [HideInInspector]
    public List<TileButtonClass> adjacentTileList;
    public Tile tileClass;
    //public bool adjacentBoolArray;
    public List<float> adjacentCostList;
    public bool isStartTile;
    public bool isSetUp;
    public int nowEdgeNumber;
    public float xPos;
    public float yPos;

    public TileButtonClass(GameObject button, Tile tile,bool start)
    {
        tileButtonObject = button;
        adjacentTileList = new List<TileButtonClass>();
        tileClass = tile;
        /*
        adjacentBoolArray = new bool[edgeNumber];
        for(int i = 0; i < edgeNumber; i++)
        {
            adjacentBoolArray[i] = false;
        }*/

        adjacentCostList = new List<float>();
        //for (int i = 0; i < edgeNumber; i++)
        //{
        //    adjacentCostArray[i] = 0;
        //}

        isStartTile = start;
        isSetUp = start;
        nowEdgeNumber = 0;
    }

    //디버그용
    public TileButtonClass(bool start)
    {
        tileButtonObject = new GameObject();
        adjacentTileList = new List<TileButtonClass>();
        //adjacentBoolArray = new bool[edgeNumber];
        //for (int i = 0; i < edgeNumber; i++)
        //{
        //    adjacentBoolArray[i] = false;
        //}

        adjacentCostList = new List<float>();
 
        isStartTile = start;
        isSetUp = start;
        nowEdgeNumber = 0;
    }

    public void SetPosition(float x, float y)
    {
        xPos = x;
        yPos = y;
    }

    public void SetEdge(TileButtonClass otherTileButton)
    {
        if (adjacentTileList.Contains(otherTileButton))
        {
            Debug.Log("진짜좆됐음");
        }
        float cost = Mathf.Sqrt((otherTileButton.xPos - xPos) * (otherTileButton.xPos - xPos)
           + (otherTileButton.yPos - yPos) * (otherTileButton.yPos - yPos));
        adjacentTileList.Add(otherTileButton);
        adjacentCostList.Add(cost) ;
        //        adjacentBoolArray[(int)edgeWay] = true;
        //Edge oppositeWay;
        ////반대방향
        //if ((int)edgeWay % 2 == 0)
        //{
        //    oppositeWay = edgeWay + 1;
        //}
        //else
        //{
        //    oppositeWay = edgeWay - 1;
        //}
        otherTileButton.adjacentTileList.Add(this);
        otherTileButton.adjacentCostList.Add(cost);
        otherTileButton.nowEdgeNumber++;
        nowEdgeNumber++;

    }


}
