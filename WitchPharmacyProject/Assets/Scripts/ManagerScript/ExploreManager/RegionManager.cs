using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionManager : MonoBehaviour
{
    SceneManager sceneManager;
    ExploreManager exploreManager;
    [SerializeField]
    GameObject tileMapParent;
    [SerializeField]
    GameObject[] canvasArray;
    [SerializeField]
    GameObject clockCanvas;
    [SerializeField]
    GameObject tabletButton;
    //            //StartTile, MedicineTile, StoreTile, StoryTile,
    //            //TreasureTile, TrapTile, BarrierTile, SpecialStoreTile
    [SerializeField]
    GameObject[] tileManagerObjectArray;
    [SerializeField]
    TileManager[] tileManagerArray;
    [SerializeField]
    WitchMover witchMover;

    

    // Start is called before the first frame update
    void Start()
    {
        sceneManager = SceneManager.inst;
        exploreManager = ExploreManager.inst;
        tileManagerArray = new TileManager[tileManagerObjectArray.Length];
        for(int i = 1; i < tileManagerObjectArray.Length; i++)
        {
            switch (i)
            {
                //StartTile, MedicineTile, StoreTile, StoryTile,
                //TreasureTile, TrapTile, BarrierTile, SpecialStoreTile
                case 1:
                    tileManagerArray[i] = tileManagerObjectArray[i].GetComponent<MedicineTileManager>();
                    break;
                //case 2:
                //    tile = new StoreTile(nowFullArrayIndex);
                //    tileManagerArray[i] = tileManagerObjectArray[i].GetComponent<TileManager>();
                //    break;
                //case 3:
                //    tile = new StoryTile(nowFullArrayIndex);
                //    tileManagerArray[i] = tileManagerObjectArray[i].GetComponent<TileManager>();
                //    break;
                //case 4:
                //    tile = new TreasureTile(nowFullArrayIndex);
                //    tileManagerArray[i] = tileManagerObjectArray[i].GetComponent<TileManager>();
                //    break;
                //case 5:
                //    tile = new TrapTile(nowFullArrayIndex);
                //    tileManagerArray[i] = tileManagerObjectArray[i].GetComponent<TileManager>();
                //    break;
                //case 6:
                //    tile = new BarrierTile(nowFullArrayIndex);
                //    tileManagerArray[i] = tileManagerObjectArray[i].GetComponent<TileManager>();
                //    break;
                //case 7:
                //    tile = new SpecialStoreTile(nowFullArrayIndex);
                //    tileManagerArray[i] = tileManagerObjectArray[i].GetComponent<TileManager>();
                //    break;
                default:
                    tileManagerArray[i] = tileManagerObjectArray[i].GetComponent<MedicineTileManager>();
                    break;
            }

            tileManagerArray[i].Initialize(exploreManager.nowProperty);
        }
    }

    //버튼에서 불러옴.
    public void BackToExploreButton()
    {
        sceneManager.LoadScene("ExploreScene");
    }



    public void OnTileOpenButton()
    {
        TileButtonClass nowTile = witchMover.nowTileButton;
        TileType tileType = nowTile.tileClass.tileType;
        if (tileType == TileType.StartTile)
        {
            return;
        }
        int index = (int)tileType;
        tileMapParent.SetActive(false);

        canvasArray[index].SetActive(true);
        for(int i = 1; i < canvasArray.Length; i++)
        {
            if (index == i)
            {
                continue;
            }
            canvasArray[i].SetActive(false);
        }
        //            //StartTile, MedicineTile, StoreTile, StoryTile,
        //            //TreasureTile, TrapTile, BarrierTile, SpecialStoreTile

        switch (tileType)
        {
            case TileType.StoryTile:
                clockCanvas.SetActive(false);
                break;
            default:
                clockCanvas.SetActive(true);
                break;

        }

        tileManagerArray[(int)tileType].TileOpen(nowTile);

    }

    public void BackToTileMap()
    {
        tileMapParent.SetActive(true);
        for (int i = 0; i < canvasArray.Length; i++)
        {
            canvasArray[i].SetActive(false);
        }
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
