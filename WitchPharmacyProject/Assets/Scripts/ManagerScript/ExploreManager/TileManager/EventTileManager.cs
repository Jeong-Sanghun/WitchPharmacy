using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Text;

//함정, 트레저
public class EventTileManager : TileManager
{
    SceneManager sceneManager;
    TabletManager tabletManager;
    List<StoreToolClass> storeToolDataList;
    List<OwningToolClass> owningToolList;
    [SerializeField]
    GameObject rewardCanvas;
    [SerializeField]
    Sprite coinImage;

    [SerializeField]
    Image rewardImage;
    [SerializeField]
    Text rewardText;
    
    //GameObject nowRewardCanvas;
    List<DocumentCondition> documentConditionList;
    //[SerializeField]
    //GameObject trapCanvas;

    //디버그용. 나중엔 이미지들어갈거임.
    [SerializeField]
    Image boxImageDebug;

    UILanguagePack languagePack;

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
        languagePack = gameManager.languagePack;
        tabletManager = TabletManager.inst;
        storeToolDataList = gameManager.storeToolDataWrapper.storeToolDataList;
        owningToolList = saveData.owningToolList;
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
                FirstTimeRewardOpen();
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
        Debug.Log("씨발뭐지?");
        rewardCanvas.SetActive(true);
    }
    void FirstTimeRewardOpen()
    {
        DocumentCondition condition;
        bool getStory = DocumentCheck(out condition);
        TreasureTile treasureTile = (TreasureTile)nowTileButton.tileClass;
        Debug.Log(nowTileButton.tileClass.tileType);
        if (getStory)
        {
            treasureTile.treasureType = TreasureType.Story;
            treasureTile.storyFileName = condition.fileName;
            rewardImage.sprite = condition.LoadSprite();
            StringBuilder builder = new StringBuilder(condition.ingameName);
            if (saveData.nowLanguageDirectory.Contains("Korean"))
            {
                string josa = languagePack.GetCompleteWord(builder.ToString(), "을", "를");
                builder.Append(josa);
            }
            rewardText.text = languagePack.Insert(languagePack.boxGained, builder.ToString());
            OwningDocumentClass doc = new OwningDocumentClass();
            doc.name = condition.fileName;
            doc.gainedRegion = exploreManager.GetRegionIngame().regionProperty.regionFileName;
            doc.gainedDay = gameManager.nowDay;
            doc.gainedTime = gameManager.nowTime;
            saveData.owningDocumentList.Add(doc);
            tabletManager.UpdateDocument(doc);
            exploreManager.OnDocumentGain(condition.fileName);
        }
        else
        {
            if (Random.Range(0, 2) == 0)
            {
                //돈
                treasureTile.treasureType = TreasureType.Coin;
                rewardImage.sprite = coinImage;
                int coin = Random.Range(20, 40);
                treasureTile.gainedThing = coin;
                rewardText.text = languagePack.Insert(languagePack.boxCoinGained, coin.ToString());
                tabletManager.UpdateBill(BillReason.exploreBoxGain, true, coin);
                saveData.coin += coin;
                exploreManager.OnCoinGain(coin);
            }
            else
            {
                string findingString = null;
                int gainedNumber = Random.Range(2, 6);
                //도구
                if (Random.Range(0, 2) == 0)
                {
                    treasureTile.treasureType = TreasureType.Pickaxe;


                    findingString = "pickaxe";
                }
                else
                {
                    treasureTile.treasureType = TreasureType.Shovel;
                    findingString = "shovel";
                }
                treasureTile.gainedThing = gainedNumber;
                int index = -1;
                for (int i = 0; i < storeToolDataList.Count; i++)
                {
                    if (storeToolDataList[i].fileName == findingString)
                    {
                        index = i;
                        break;
                    }
                }
                if (index == -1)
                {
                    return;
                }
                Debug.Log(index + "스토어인덱스");
                rewardImage.sprite = storeToolDataList[index].LoadImage();
                exploreManager.OnBuyTool(index, gainedNumber);
                StringBuilder builder = null;
                if (saveData.nowLanguageDirectory.Contains("Korean"))
                {
                    //곡괭이 2개를 획득했다.
                    builder = new StringBuilder(storeToolDataList[index].name);
                    builder.Append(gainedNumber.ToString());
                    string josa = "개를";
                    builder.Append(josa);
                }
                else if (saveData.nowLanguageDirectory.Contains("English"))
                {
                    //you got 2 pickaxes
                    builder = new StringBuilder(gainedNumber.ToString());
                    builder.Append(" ");
                    builder.Append(storeToolDataList[index].name);
                    if(gainedNumber>1)
                        builder.Append("s");
                }
                rewardText.text = languagePack.Insert(languagePack.boxGained, builder.ToString());
                OwningToolClass tool = null;
                for (int i = 0; i < saveData.owningToolList.Count; i++)
                {
                    if(saveData.owningToolList[i].index == storeToolDataList[index].GetIndex())
                    {
                        tool = saveData.owningToolList[i];
                        break;
                    }
                }
                if (tool == null)
                {
                    tool = new OwningToolClass();
                    tool.index = storeToolDataList[index].GetIndex();
                    tool.quantity = gainedNumber;
                    saveData.owningToolList.Add(tool);
                }
                else
                {
                    tool.quantity += gainedNumber;
                }

            }
        }
        rewardCanvas.SetActive(true);
    }

    void OpenedTileRewardOpen(TreasureType type)
    {
        rewardCanvas.SetActive(true);
        TreasureTile treasureTile = (TreasureTile)nowTileButton.tileClass;
        switch (type){
            case TreasureType.Coin:
                break;
            case TreasureType.Story:
                DocumentCondition condition = null;
                for(int i = 0; i < gameManager.documentConditionWrapper.documentConditionList.Count; i++)
                {
                    if(treasureTile.storyFileName == gameManager.documentConditionWrapper.documentConditionList[i].fileName)
                    {
                        condition = gameManager.documentConditionWrapper.documentConditionList[i];
                        break;
                    }
                }
                if(condition == null)
                {
                    Debug.LogError("좃됐다 컨디션이 안찾아진다");
                    return;
                }
                rewardImage.sprite = condition.LoadSprite();
                StringBuilder builder = new StringBuilder(condition.ingameName);
                if (saveData.nowLanguageDirectory.Contains("Korean"))
                {
                    string josa = languagePack.GetCompleteWord(builder.ToString(), "을", "를");
                    builder.Append(josa);
                }
                rewardText.text = languagePack.Insert(languagePack.boxGained, builder.ToString());

                break;
            case TreasureType.Pickaxe:
                int index = -1;
                for (int i = 0; i < storeToolDataList.Count; i++)
                {
                    if (storeToolDataList[i].fileName == "pickaxe")
                    {
                        index = i;
                        break;
                    }
                }
                if (index == -1)
                {
                    return;
                }
                rewardImage.sprite = storeToolDataList[index].LoadImage();

                builder = null;
                if (saveData.nowLanguageDirectory.Contains("Korean"))
                {
                    //곡괭이 2개를 획득했다.
                    builder = new StringBuilder(storeToolDataList[index].name);
                    builder.Append(treasureTile.gainedThing.ToString());
                    string josa = "개를";
                    builder.Append(josa);
                }
                else if (saveData.nowLanguageDirectory.Contains("English"))
                {
                    //you got 2 pickaxes
                    builder = new StringBuilder(treasureTile.gainedThing.ToString());
                    builder.Append(" ");
                    builder.Append(storeToolDataList[index].name);
                    if (treasureTile.gainedThing > 1)
                        builder.Append("s");
                }
                rewardText.text = languagePack.Insert(languagePack.boxGained, builder.ToString());

                break;
            case TreasureType.Shovel:
                index = -1;
                for (int i = 0; i < storeToolDataList.Count; i++)
                {
                    if (storeToolDataList[i].fileName == "shovel")
                    {
                        index = i;
                        break;
                    }
                }
                if (index == -1)
                {
                    return;
                }
                rewardImage.sprite = storeToolDataList[index].LoadImage();

                builder = null;
                if (saveData.nowLanguageDirectory.Contains("Korean"))
                {
                    //곡괭이 2개를 획득했다.
                    builder = new StringBuilder(storeToolDataList[index].name);
                    builder.Append(" ");
                    builder.Append(treasureTile.gainedThing.ToString());
                    string josa = "개를";
                    builder.Append(josa);
                }
                else if (saveData.nowLanguageDirectory.Contains("English"))
                {
                    //you got 2 pickaxes
                    builder = new StringBuilder(treasureTile.gainedThing.ToString());
                    builder.Append(" ");
                    builder.Append(storeToolDataList[index].name);
                    if (treasureTile.gainedThing > 1)
                        builder.Append("s");
                }
                rewardText.text = languagePack.Insert(languagePack.boxGained, builder.ToString());
                break;
            default:
                break;

        }
    }

    bool DocumentCheck(out DocumentCondition bundle)
    {
        bundle = null;

        for(int i = 0; i < documentConditionList.Count; i++)
        {
            DocumentCondition condition = documentConditionList[i];
            bool conditioning = true;
            for(int j = 0; j < saveData.owningDocumentList.Count; j++)
            {
                if(saveData.owningDocumentList[j].name == condition.fileName)
                {
                    conditioning = false;
                    break;
                }
            }
            if (conditioning == false)
            {
                continue;
            }
            for (int j = 0; j < condition.regionGainConditionList.Count; j++)
            {
                if (exploreManager.GetRegionIngame().regionProperty.regionFileName
                    == condition.regionGainConditionList[j])
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
                Debug.Log("이거 왜 안나옴?");
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
                bundle = condition;
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
