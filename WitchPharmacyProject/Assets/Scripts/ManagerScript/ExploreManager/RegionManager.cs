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
                case 2:
                    tileManagerArray[i] = tileManagerObjectArray[i].GetComponent<StoreTileManager>();
                    break;
                case 3:
                    tileManagerArray[i] = tileManagerObjectArray[i].GetComponent<StoryTileManager>();
                    break;
                case 4:
                case 5:
                    tileManagerArray[i] = tileManagerObjectArray[i].GetComponent<EventTileManager>();
                    break;
                case 6:
                    tileManagerArray[i] = tileManagerObjectArray[i].GetComponent<BarrierTileManager>();
                    break;
                case 7:
                    tileManagerArray[i] = tileManagerObjectArray[i].GetComponent<SpecialStoreTileManager>();
                    break;
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


        for(int i = 1; i < canvasArray.Length; i++)
        {
            canvasArray[i].SetActive(false);

        }
        canvasArray[index].SetActive(true);
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

    //각종 뒤로가기 버튼
    public void BackToTileMap()
    {
        TileType tileType = witchMover.nowTileButton.tileClass.tileType;
        tileMapParent.SetActive(true);
        for (int i = 0; i < canvasArray.Length; i++)
        {
            canvasArray[i].SetActive(false);
        }
        witchMover.AwareTile(witchMover.nowTileButton);

        if (tileType == TileType.StoryTile)
        {
            clockCanvas.SetActive(true);

        }

        witchMover.TileOpen();
    }

    //이거는 배리어에서 뒤로가기 했을 때 나옴.
    public void BackToTileMapFromBarrier()
    {
        tileMapParent.SetActive(true);
        for (int i = 0; i < canvasArray.Length; i++)
        {
            canvasArray[i].SetActive(false);
        }
        witchMover.TileOpen();
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
