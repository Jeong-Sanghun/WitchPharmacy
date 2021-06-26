using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//함정, 트레저
public class EventTileManager : TileManager
{
    SceneManager sceneManager;
    [SerializeField]
    GameObject rewardCanvas;
    //[SerializeField]
    //GameObject trapCanvas;

    //디버그용. 나중엔 이미지들어갈거임.
    [SerializeField]
    Image boxImageDebug;

    const int fullClickCount = 10;
    int nowClickCount;
    bool boxOpened;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        boxOpened = false;
        sceneManager = SceneManager.inst;
    }

    public override void TileOpen(TileButtonClass tile)
    {
        base.TileOpen(tile);
        

        switch (nowTileButton.tileClass.tileType)
        {
            case TileType.TrapTile:
                TrapTile trapTile = (TrapTile)nowTileButton.tileClass;
                if (trapTile.boxOpened)
                {
                    rewardCanvas.SetActive(true);
                }
                break;
            case TileType.TreasureTile:
                TreasureTile treasureTile = (TreasureTile)nowTileButton.tileClass;
                if (treasureTile.boxOpened)
                {
                    rewardCanvas.SetActive(true);
                }
                break;
            default:
                break;
        }
    }

    //트레저 클릭. EventTrigger
    public void OnBoxClick()
    {
        if (boxOpened)
        {
            return;
        }
        nowClickCount++;
        if(nowClickCount%3 == 0)
        {
            //1분늘어난다.
            exploreManager.TimeChange(60);
        }
        if (nowClickCount >= fullClickCount)
        {
            boxOpened = true;
            StartBoxCoroutine();
            nowClickCount = 0;
        }
    }

    void StartBoxCoroutine()
    {
        StartCoroutine(BoxAnimation());
    }

    IEnumerator BoxAnimation()
    {
        StartCoroutine(sceneManager.FadeModule_Image(boxImageDebug.gameObject, 0, 1, 0.5f));
        yield return new WaitForSeconds(1f);
        boxImageDebug.gameObject.SetActive(false);
        switch (nowTileButton.tileClass.tileType)
        {
            case TileType.TrapTile:
                TrapTile trapTile = (TrapTile)nowTileButton.tileClass;
                trapTile.boxOpened = true;
                RewardOpen();
                break;
            case TileType.TreasureTile:
                TreasureTile treasureTile = (TreasureTile)nowTileButton.tileClass;
                treasureTile.boxOpened = true;
                RewardOpen();
                break;
            default:
                RewardOpen();
                break;
        }
    }

    //void TreasureOpen()
    //{
    //    treasureCanvas.SetActive(true);
    //    //여기다가 데이터 추가해야함. 근데 희귀약재 아직 안나와서.
    //}

    //void TrapOpen()
    //{
    //    trapCanvas.SetActive(true);
    //}

    void RewardOpen()
    {
        rewardCanvas.SetActive(true);
    }

    public void OnBackButton()
    {
        boxImageDebug.gameObject.SetActive(true);
        rewardCanvas.SetActive(true);
        nowClickCount = 0;
        boxOpened = false;
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
