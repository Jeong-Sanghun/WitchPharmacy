using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;

//씨발 이거 타일에서 쓰려고 조부모상속을 해줘야함 ㅡㅡ
//타일매니저가 좃도아닌거여서 다행이지 ㅋㅋ
public class StoreToolManager : TileManager,IStore
{
    const int rQuantityToSell = 10;
    TabletManager tabletManager;
    protected List<StoreToolClass> storeToolDataList;
    protected List<StoreToolClass> appearingStoreToolList;
    protected List<OwningToolClass> owningToolList;
    //Dictionary<int, int> owningMedicineDictionary;
    //List<CookedMedicineData> cookedMedicineDataList;
    //위는 기본적인 매니저들 그리고 데이터들

    //스크롤 뷰에 들어가있는 content들 위아래 길이조정 해줘야함.
    //버튼도 이 아래에 생성할거여서 따로 드래그앤드롭 해줘야함. GetChild로 받아왔는데 순서꼬이면 귀찮아짐.
    [SerializeField]
    protected RectTransform scrollContent;

    [SerializeField]
    protected GameObject toolButtonPrefab;

    //스크롤뷰에 들어가는 약재버튼 하나. 프리팹으로 만들어서 Instantiate해줄거.
    //프리팹들에 들어가는 것들을 다 받아오고, 시작할 때만 설정해주고 Instantiate해주고 그다음 버튼 새로 설정하고 Instantiate해주고 반복.
    [SerializeField]
    protected Text prefabButtonName;
    [SerializeField]
    protected Image prefabButtonIcon;
    [SerializeField]
    protected Text prefabButtonQuantity;
    [SerializeField]
    protected Text prefabButtonCost;
    [SerializeField]
    protected Text prefabButtonToolTip;


    [SerializeField]
    protected GameObject sliderPopupParent;
    [SerializeField]
    protected Text sliderPopupQuantityText;
    [SerializeField]
    protected Slider quantSlider;

    [SerializeField]
    protected GameObject justBuyPopupParent;

    [SerializeField]
    protected GameObject notEnoughCoinPopup;
    [SerializeField]
    protected Text coinText;

    protected List<StoreToolButton> wholeToolButtonList;
    protected int nowButtonIndex;
    protected bool nowPopup;

    protected bool isTileStore;


    //이거 스토어타일 매니저에서 그대로 써올거임.
    protected virtual new void Start()
    {
        gameManager = GameManager.singleTon;
        saveData = gameManager.saveData;
        storeToolDataList = gameManager.storeToolDataWrapper.storeToolDataList;
        owningToolList = saveData.owningToolList;
        appearingStoreToolList = storeToolDataList;
        StoreStart();

        isTileStore = false;
    }
    // Start is called before the first frame update
    protected void StoreStart()
    {


        wholeToolButtonList = new List<StoreToolButton>();
        int buttonIndex = 0;
        for (int i = 0; i < appearingStoreToolList.Count; i++)
        {

            int quantity = rQuantityToSell;
            if (isTileStore)
            {
                StoreTile tile = (StoreTile)nowTileButton.tileClass;
                for(int j = 0; j < tile.selledToolList.Count; j++)
                {
                    if(tile.selledToolList[j].index == appearingStoreToolList[i].GetIndex())
                    {
                        quantity -= tile.selledToolList[j].quantity;
                    }
                }
                
            }
            StoreToolClass tool = appearingStoreToolList[i];
            StringBuilder nameBuilder = new StringBuilder(tool.name);

            prefabButtonIcon.sprite = tool.LoadImage();
            prefabButtonName.text = nameBuilder.ToString();
            prefabButtonToolTip.text = tool.toolTip;
            prefabButtonCost.text = tool.cost.ToString();
            if (!appearingStoreToolList[i].usedOnce)
            {
                prefabButtonQuantity.gameObject.SetActive(false);
                quantity = 1;
            }
            else
            {
                prefabButtonQuantity.gameObject.SetActive(true);
            }
            prefabButtonQuantity.text = quantity.ToString();

            //버튼 세팅 다 해서 instantiate하고 리스트에 넣어줌.
            //이제 이 리스트에서 active하고 위치바꿔주고 그럴거임.
            GameObject buttonObject = Instantiate(toolButtonPrefab, scrollContent.transform);
            buttonObject.SetActive(true);


            Text quantText = buttonObject.transform.GetChild(2).GetComponent<Text>();


            StoreToolButton buttonClass = new StoreToolButton(buttonObject, i, quantity, quantText);
            if (quantity == 0)
            {
                buttonClass.zeroTool = true;
            }
            buttonClass.storeTool = appearingStoreToolList[i];
            wholeToolButtonList.Add(buttonClass);

            int dataIndex = 0;
            for(int j = 0; j < storeToolDataList.Count; j++)
            {
                if(appearingStoreToolList[i] == storeToolDataList[j])
                {
                    dataIndex = j;
                }
            }
            OwningToolClass owningTool = null;
            for(int j = 0; j < owningToolList.Count;j++)
            {
                if(owningToolList[j].index == dataIndex)
                {
                    owningTool = owningToolList[j];
                    break;
                }
            }
            if(owningTool == null)
            {
                owningTool = new OwningToolClass();
                owningTool.index = dataIndex;
                owningTool.quantity = 0;
                owningToolList.Add(owningTool);
            }
            buttonClass.owningTool = owningTool;
            
            
            //이 위까지가 프리팹들 다 설정해서 whoelMedicineButtonList에 buttonClass를 추가하는거임.
            //wholeMedicineButtonList의 index는 바로 아랫줄과 onButtonUp Down Drag함수에서 사용하니 medicineDictionary와 혼동하지 않도록 유의
            //정훈아 미안해 이거 주석 다 못적겠어. 그냥 그러려니 해. 셋업이야.

            int delegateIndex = buttonIndex;
            Button button = buttonClass.buttonObject.GetComponent<Button>();
            button.onClick.AddListener(() => OnButtonDown(delegateIndex));

            buttonIndex++;
            //wholeMedicienButtonList의 index임.

        }

        coinText.text = saveData.coin.ToString();
        ListButton();

    }

    public void OnButtonDown(int index)
    {
        if (nowPopup)
        {
            return;
        }
        nowPopup = true;
        nowButtonIndex = index;
        Debug.Log(nowButtonIndex);
        if (appearingStoreToolList[index].usedOnce)
        {
            sliderPopupParent.SetActive(true);
        }
        else
        {
            justBuyPopupParent.SetActive(true);
        }
        sliderPopupQuantityText.text = "0";
        quantSlider.value = 0;

    }

    public virtual void OnBuyButton()
    {
        nowPopup = false;

        int quant = (int)(wholeToolButtonList[nowButtonIndex].toolQuant * quantSlider.value);
        if (appearingStoreToolList[nowButtonIndex].usedOnce)
        {
            sliderPopupParent.SetActive(false);
        }
        else
        {
            quant = 1;
            justBuyPopupParent.SetActive(false);
        }
        if (quant == 0)
        {
            return;
        }
        //돈이없을 때
        int coinUsed = quant * wholeToolButtonList[nowButtonIndex].storeTool.cost;
        if (saveData.coin - coinUsed < 0)
        {
            //여기서 에러메시지 표출
            notEnoughCoinPopup.SetActive(true);
            OnPopupBackButton();
            nowPopup = true;
            return;
        }
        int existIndex = -1;
        for (int i = 0; i < owningToolList.Count; i++)
        {
            if (wholeToolButtonList[nowButtonIndex].toolIndex == owningToolList[i].index)
            {
                existIndex = i;
                break;
            }
        }
        if (existIndex == -1)
        {
            Debug.Log("여기되나");
            OwningToolClass tool = new OwningToolClass();
            tool.index = wholeToolButtonList[nowButtonIndex].toolIndex;
            tool.quantity = 0;
            owningToolList.Add(tool);
            wholeToolButtonList[nowButtonIndex].owningTool = tool;
            //existIndex = owningToolList.Count - 1;
        }
        else
        {
            wholeToolButtonList[nowButtonIndex].owningTool = owningToolList[existIndex];
        }

        if (isTileStore)
        {
            int existTileIndex = -1;
            StoreTile tile = (StoreTile)nowTileButton.tileClass;
            OwningToolClass selledTool;
            for(int i = 0; i < tile.selledToolList.Count; i++)
            {
                if(wholeToolButtonList[nowButtonIndex].owningTool.index == tile.selledToolList[i].index)
                {
                    existTileIndex = i;
                    break;
                }
            }
            if(existTileIndex == -1)
            {
                selledTool = new OwningToolClass();
                selledTool.index = wholeToolButtonList[nowButtonIndex].owningTool.index;
                selledTool.quantity = quant;
                tile.selledToolList.Add(selledTool);
            }
            else
            {
                tile.selledToolList[existTileIndex].quantity += quant;
            }
            exploreManager.OnBuyTool(wholeToolButtonList[nowButtonIndex].owningTool.index, quant);
        }

        saveData.coin -= coinUsed;
        tabletManager = TabletManager.inst;
        tabletManager.UpdateBill(BillReason.toolBuy, false, coinUsed);

        wholeToolButtonList[nowButtonIndex].toolQuant -= quant;
        if (wholeToolButtonList[nowButtonIndex].toolQuant <= 0)
        {
            wholeToolButtonList[nowButtonIndex].zeroTool = true;
        }
        coinText.text = saveData.coin.ToString();

        wholeToolButtonList[nowButtonIndex].owningTool.quantity += quant;
        wholeToolButtonList[nowButtonIndex].quantityText.text = wholeToolButtonList[nowButtonIndex].toolQuant.ToString();
        if (wholeToolButtonList[nowButtonIndex].zeroTool)
        {
           ListButton();
        }
        nowButtonIndex = -1;
        //gameManager.SaveJson();
    }

    protected void ListButton()
    {
        int buttonQuantity = 0;

        for (int i = 0; i < wholeToolButtonList.Count; i++)
        {

            if (wholeToolButtonList[i].owningTool.quantity >= 1 && !wholeToolButtonList[i].storeTool.usedOnce)
            {
                wholeToolButtonList[i].buttonObject.SetActive(false);
                continue;
            }

            if (!wholeToolButtonList[i].zeroTool)
            {
                wholeToolButtonList[i].buttonObject.SetActive(true);
                wholeToolButtonList[i].buttonRect.anchoredPosition = new Vector2(0, -90 - buttonQuantity * 180);
                buttonQuantity++;
            }
            else
            {
                wholeToolButtonList[i].buttonObject.SetActive(false);
            }

        }


        scrollContent.sizeDelta = new Vector2(0, 180 * buttonQuantity);
    }


    //그 버튼눌러서 1개씩 올라가느넉
    public void OnQuantityChangeButton(bool plus)
    {
        int one;
        if (plus)
        {
            one = 1;
        }
        else
        {
            one = -1;
        }
        float ratio = 1.0f / wholeToolButtonList[nowButtonIndex].toolQuant;
        //float value = (int)(wholeToolButtonList[nowButtonIndex].ToolQuant * quantSlider.value) *ratio;
        //quantSlider.value = value;

        if (quantSlider.value + one * ratio < 0)
        {
            quantSlider.value = 0;
        }
        else if (quantSlider.value + one * ratio > 1)
        {
            quantSlider.value = 1;
        }
        else
        {
            quantSlider.value += one * ratio;
        }

        sliderPopupQuantityText.text = ((int)(wholeToolButtonList[nowButtonIndex].toolQuant * quantSlider.value)).ToString();

    }

    public void OnSliderValueChange()
    {
        sliderPopupQuantityText.text = ((int)(wholeToolButtonList[nowButtonIndex].toolQuant * quantSlider.value)).ToString();
    }



    public void OnNotEnoughCoinPopupButton()
    {
        nowPopup = false;
        notEnoughCoinPopup.SetActive(false);

    }

    public void OnPopupBackButton()
    {
        nowPopup = false;
        if (appearingStoreToolList[nowButtonIndex].usedOnce)
        {
            sliderPopupParent.SetActive(false);
        }
        else
        {
            justBuyPopupParent.SetActive(false);
        }
        nowButtonIndex = -1;

    }


}
