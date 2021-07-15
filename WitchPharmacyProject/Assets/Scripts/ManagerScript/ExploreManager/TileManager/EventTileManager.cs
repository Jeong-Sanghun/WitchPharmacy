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

    //GameObject nowRewardCanvas;
    List<DocumentCondition> documentConditionList;
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
        documentConditionList = gameManager.documentConditionWrapper.documentConditionList;
    }

    public override void TileOpen(TileButtonClass tile)
    {
        base.TileOpen(tile);

        nowClickCount = 0;

        switch (nowTileButton.tileClass.tileType)
        {
            case TileType.TrapTile:
                TrapTile trapTile = (TrapTile)nowTileButton.tileClass;
                if (trapTile.boxOpened)
                {
                    TrapOpen();
                    boxImageDebug.gameObject.SetActive(false);
                    boxOpened = true;
                }
                else
                {
                    rewardCanvas.SetActive(false);
                    boxImageDebug.gameObject.SetActive(true);
                    nowClickCount += trapTile.clickedTime;
                    boxOpened = false;
                }

                break;
            case TileType.TreasureTile:
                TreasureTile treasureTile = (TreasureTile)nowTileButton.tileClass;
                if (treasureTile.boxOpened)
                {
                    OpenedTileRewardOpen(treasureTile.treasureType);
                    boxImageDebug.gameObject.SetActive(false);
                    boxOpened = true;
                }
                else
                {
                    rewardCanvas.SetActive(false);
                    boxImageDebug.gameObject.SetActive(true);
                    boxOpened = false;
                }
                nowClickCount += treasureTile.clickedTime;
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

        if(nowTileButton == null)
        {
            Debug.Log("타일버튼이 널이야");
        }

        switch (nowTileButton.tileClass.tileType)
        {
            case TileType.TrapTile:
                TrapTile trapTile = (TrapTile)nowTileButton.tileClass;
                trapTile.clickedTime++;
                break;
            case TileType.TreasureTile:
                TreasureTile treasureTile = (TreasureTile)nowTileButton.tileClass;
                treasureTile.clickedTime++;
                break;
            default:
                break;
        }
        Debug.Log(nowClickCount);
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
                TrapOpen();
                break;
            case TileType.TreasureTile:
                TreasureTile treasureTile = (TreasureTile)nowTileButton.tileClass;
                treasureTile.boxOpened = true;
                RewardOpen();
                break;
            default:
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

    void TrapOpen()
    {
        //여기는 트랩 세팅 해줄 필요 없음. 트랩은 단일 캔버스임.
        rewardCanvas.SetActive(true);
    }
    void RewardOpen()
    {
        string bundleFileName;
        bool getStory = DocumentCheck(out bundleFileName);
        if (getStory)
        {

        }
        else
        {

        }
        rewardCanvas.SetActive(true);
    }

    void OpenedTileRewardOpen(TreasureType type)
    {

    }

    bool DocumentCheck(out string bundleFileName)
    {
        bundleFileName = null;

        for(int i = 0; i < documentConditionList.Count; i++)
        {
            DocumentCondition condition = documentConditionList[i];
            bool conditioning = true;
            for (int j = 0; j < condition.regionGainConditionList.Count; j++)
            {
                if (exploreManager.nowRegionIngame.regionProperty.regionFileName == condition.regionGainConditionList[j])
                {
                    conditioning = true;
                    break;
                }
                else
                {
                    conditioning = false;
                }
            }
            if (conditioning == false)
            {
                continue;
            }
            for (int j = 0; j < condition.questGainConditionList.Count; j++)
            {
                if (!saveData.solvedQuestBundleName.Contains(condition.questGainConditionList[j]))
                {
                    conditioning = false;
                    break;
                }
            }
            if(conditioning == false)
            {
                continue;
            }
            for (int j = 0; j < condition.documentGainConditionList.Count; j++)
            {
                bool contains = false;
                for(int k = 0; k < saveData.owningDocumentList.Count; k++)
                {
                    if (saveData.owningDocumentList[k].name == condition.documentGainConditionList[j])
                    {
                        contains = true;
                        break;
                    }
                }
                if(contains == false)
                {
                    conditioning = false;
                    break;
                }
                
            }
            if (conditioning == false)
            {
                continue;
            }
            for (int j = 0; j < condition.storyGainConditionList.Count; j++)
            {
                if (!saveData.readStoryList.Contains(condition.storyGainConditionList[j]))
                {
                    conditioning = false;
                    break;
                }
            }
            if (conditioning == false)
            {
                continue;
            }
            else
            {
                bundleFileName = condition.fileName;
                return true;
            }
            


        }
        
        return false;
    }

    public void OnBackButton()
    {
        boxImageDebug.gameObject.SetActive(false);
        rewardCanvas.SetActive(false);
        nowClickCount = 0;
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
