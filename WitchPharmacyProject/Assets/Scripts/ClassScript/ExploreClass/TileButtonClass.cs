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
    public static float maxCost = 0.5f;
    public GameObject tileButtonObject;
    public List<GameObject> adjacentLineList;

    [HideInInspector]
    public List<TileButtonAdjacent> adjacentTileList;
    public Tile tileClass;
    //public bool adjacentBoolArray;
    public List<float> adjacentCostList;
    public bool isStartTile;
    public bool isSetUp;
    public int nowEdgeNumber;
    public float xPos;
    public float yPos;
    //들어갔는지
    public bool opened;
    //밝혀졌는지
    public bool awared;

    public TileButtonClass(GameObject button, Tile tile,bool start)
    {
        tileButtonObject = button;
        adjacentTileList = new List<TileButtonAdjacent>();
        adjacentLineList = new List<GameObject>();
        tileClass = tile;
        opened = false;
        awared = false;
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
        adjacentTileList = new List<TileButtonAdjacent>();
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

    public void SetEdge(TileButtonClass otherTileButton,GameObject adjacentLine)
    {
        for(int i = 0; i < adjacentTileList.Count; i++)
        {
            if (adjacentTileList[i].adjacentTileButton ==(otherTileButton))
            {
                Debug.Log("진짜좆됐음");
                return;
            }
        }
        
        float cost = Mathf.Sqrt((otherTileButton.xPos - xPos) * (otherTileButton.xPos - xPos)
           + (otherTileButton.yPos - yPos) * (otherTileButton.yPos - yPos));
        TileButtonAdjacent adjacentTile = new TileButtonAdjacent(otherTileButton);
        adjacentTileList.Add(adjacentTile);
        adjacentCostList.Add(cost) ;
        adjacentLineList.Add(adjacentLine);
        nowEdgeNumber++;

        TileButtonAdjacent thisTile = new TileButtonAdjacent(this);

        otherTileButton.adjacentTileList.Add(thisTile);
        otherTileButton.adjacentCostList.Add(cost);
        otherTileButton.adjacentLineList.Add(adjacentLine);
        otherTileButton.nowEdgeNumber++;


    }


}
