using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BarrierTileManager : TileManager
{
    List<StoreToolClass> storeToolDataList;
    List<OwningToolClass> owningToolList;

    [SerializeField]
    RegionManager regionManager;

    [SerializeField]
    GameObject dirtObject;
    [SerializeField]
    GameObject rockObject;

    [SerializeField]
    GameObject dirtCanvas;
    [SerializeField]
    GameObject rockCanvas;
    [SerializeField]
    GameObject unlockedCanvas;

    [SerializeField]
    Text dirtQuantityText;
    [SerializeField]
    Text rockQuantityText;

    [SerializeField]
    Button dirtButton;
    [SerializeField]
    Button rockButton;

    OwningToolClass nowTool;
    Text nowText;
    Button nowButton;
    GameObject nowCanvas;
    BarrierTile nowTile;



    protected override void Start()
    {
        base.Start();
        owningToolList = saveData.owningToolList;
        storeToolDataList = gameManager.storeToolDataWrapper.storeToolDataList;
    }

    public override void TileOpen(TileButtonClass tile)
    {
        base.TileOpen(tile);
        nowTile = (BarrierTile)tile.tileClass;
        string name;
        if (nowTile.isRock)
        {
            name = "곡괭이";
            nowText = rockQuantityText;
            nowButton = rockButton;
            nowCanvas = rockCanvas;
            dirtObject.SetActive(false);
            rockObject.SetActive(true);

        }
        else
        {
            name = "삽";
            nowText = dirtQuantityText;
            nowButton = dirtButton;
            nowCanvas = dirtCanvas;
            dirtObject.SetActive(true);
            rockObject.SetActive(false);
        }


        if (nowTile.isUnlocked)
        {
            nowCanvas.SetActive(false);
            dirtObject.SetActive(false);
            rockObject.SetActive(false);
            unlockedCanvas.SetActive(true);
        }
        else
        {
            //내가 그 도구를 가지고있는지. 도구 자체가 OwningList에 있는지 없는지.
            bool exist = false;
            for (int i = 0; i < owningToolList.Count; i++)
            {
                if (storeToolDataList[owningToolList[i].index].name == name)
                {
                    nowTool = owningToolList[i];
                    exist = true;
                }
            }
            if (exist)
            {
                nowText.text = nowTool.quantity.ToString();
                if (nowTool.quantity < 1)
                {
                    nowButton.interactable = false;
                }
                else
                {
                    nowButton.interactable = true;
                }
            }
            else
            {
                nowText.text = "0";
                nowButton.interactable = false;
            }
        }
        

        
    }

    public void OnObjectClick()
    {
        if (!nowTile.isUnlocked)
        {
            nowCanvas.SetActive(true);
        }
        
    }

    public void OnBackButton()
    {
        nowCanvas.SetActive(false);
        unlockedCanvas.SetActive(false);
        //aware 안함.
        regionManager.BackToTileMapFromBarrier();
    }

    public void OnPurchaseButton()
    {
        nowTile.isUnlocked = true;
        nowCanvas.SetActive(false);
        nowTool.quantity--;
        unlockedCanvas.SetActive(true);

    }

    public void OnUnlockCanvasBackButton()
    {
        nowCanvas.SetActive(false);
        unlockedCanvas.SetActive(false);
        regionManager.BackToTileMap();
    }




    void Update()
    {
        
    }
}
