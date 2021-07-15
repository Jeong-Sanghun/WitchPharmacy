using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class SpecialStoreTileManager : TileManager
{
    const int rQuantityToSell = 1;
    protected List<SpecialMedicineClass> specialMedicineDataList;
    protected List<SpecialMedicineClass> appearingSpecialMedicineList;
    protected List<OwningMedicineClass> owningSpecialMedicineList;
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


    //[SerializeField]
    //protected GameObject sliderPopupParent;
    //[SerializeField]
    //protected Text sliderPopupQuantityText;
    //[SerializeField]
    //protected Slider quantSlider;

    [SerializeField]
    protected GameObject justBuyPopupParent;

    [SerializeField]
    protected GameObject notEnoughCoinPopup;
    [SerializeField]
    protected Text coinText;

    protected List<MedicineButton> wholeMedicineButtonList;
    protected int nowButtonIndex;
    protected bool nowPopup;

    //이거 스토어타일 매니저에서 그대로 써올거임.
    protected virtual new void Start()
    {
        exploreManager = ExploreManager.inst;
        gameManager = GameManager.singleTon;
        saveData = gameManager.saveData;
        specialMedicineDataList = gameManager.specialMedicineDataWrapper.specialMedicineDataList;
        owningSpecialMedicineList = saveData.owningSpecialMedicineList;
        //StoreStart();
        //isTileStore = false;
    }
    // Start is called before the first frame update
    protected void StoreStart()
    {


        wholeMedicineButtonList = new List<MedicineButton>();
        int buttonIndex = 0;
        for (int i = 0; i < specialMedicineDataList.Count; i++)
        {

            int quantity = rQuantityToSell;
            SpecialStoreTile tile = (SpecialStoreTile)nowTileButton.tileClass;
            for (int j = 0; j < tile.selledSpecialMedicineList.Count; j++)
            {
                if (tile.selledSpecialMedicineList[j].medicineIndex
                    == appearingSpecialMedicineList[i].GetIndex())
                {
                    quantity -= tile.selledSpecialMedicineList[j].medicineQuantity;
                }
            }
            //if(quantity == 0)
            //{
            //    continue;
            //}
            SpecialMedicineClass medicine = appearingSpecialMedicineList[i];
            StringBuilder nameBuilder = new StringBuilder(medicine.firstName);
            nameBuilder.Append(" ");
            nameBuilder.Append(medicine.secondName);

            prefabButtonIcon.sprite = medicine.LoadImage();
            prefabButtonName.text = nameBuilder.ToString();
            prefabButtonToolTip.text = medicine.toolTip;
            prefabButtonCost.text = medicine.cost.ToString();
            prefabButtonQuantity.gameObject.SetActive(true);
            prefabButtonQuantity.text = quantity.ToString();

            //버튼 세팅 다 해서 instantiate하고 리스트에 넣어줌.
            //이제 이 리스트에서 active하고 위치바꿔주고 그럴거임.
            GameObject buttonObject = Instantiate(toolButtonPrefab, scrollContent.transform);
            buttonObject.SetActive(true);


            Text quantText = buttonObject.transform.GetChild(2).GetComponent<Text>();


            MedicineButton buttonClass = new MedicineButton
                (buttonObject,buttonIndex,quantity,new MedicineClass(medicine),null,null,quantText,null);
            if (quantity == 0)
            {
                buttonClass.zeroMedicine = true;
            }
            //buttonClass.storeTool = appearingSpecialMedicineList[i];
            wholeMedicineButtonList.Add(buttonClass);

            int dataIndex = 0;
            for (int j = 0; j < specialMedicineDataList.Count; j++)
            {
                if (appearingSpecialMedicineList[i] == specialMedicineDataList[j])
                {
                    dataIndex = j;
                    break;
                }
            }
            OwningMedicineClass owningSpecialMedicine = null;
            for (int j = 0; j < owningSpecialMedicineList.Count; j++)
            {
                if (owningSpecialMedicineList[j].medicineIndex == dataIndex)
                {
                    owningSpecialMedicine = owningSpecialMedicineList[j];
                    break;
                }
            }
            if (owningSpecialMedicine == null)
            {
                owningSpecialMedicine = new OwningMedicineClass();
                owningSpecialMedicine.medicineIndex = dataIndex;
                owningSpecialMedicine.medicineQuantity = 0;
                owningSpecialMedicineList.Add(owningSpecialMedicine);
            }
            buttonClass.owningMedicine = owningSpecialMedicine;


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
        //if (appearingSpecialMedicineList[index].usedOnce)
        //{
        //    sliderPopupParent.SetActive(true);
        //}
        //else
        //{
        //    justBuyPopupParent.SetActive(true);
        //}
        justBuyPopupParent.SetActive(true);

    }

    public virtual void OnBuyButton()
    {
        nowPopup = false;

        int quant = 1;
        //if (appearingSpecialMedicineList[nowButtonIndex].usedOnce)
        //{
        //    sliderPopupParent.SetActive(false);
        //}
        //else
        //{
        //    quant = 1;
        //    justBuyPopupParent.SetActive(false);
        //}
        justBuyPopupParent.SetActive(false);
        //돈이없을 때
        if (saveData.coin - quant * wholeMedicineButtonList[nowButtonIndex].medicineClass.cost < 0)
        {
            //여기서 에러메시지 표출
            notEnoughCoinPopup.SetActive(true);
            OnPopupBackButton();
            nowPopup = true;
            return;
        }
        //int existIndex = -1;
        //for (int i = 0; i < owningSpecialMedicineList.Count; i++)
        //{
        //    if (wholeMedicineButtonList[nowButtonIndex].medicineIndex == owningSpecialMedicineList[i].medicineIndex)
        //    {
        //        existIndex = i;
        //        break;
        //    }
        //}
        //if (existIndex == -1)
        //{
        //    Debug.Log("여기되나");
        //    OwningMedicineClass tool = new OwningMedicineClass();
        //    tool.medicineIndex = wholeMedicineButtonList[nowButtonIndex].medicineIndex;
        //    tool.medicineQuantity = 0;
        //    owningSpecialMedicineList.Add(tool);
        //    wholeMedicineButtonList[nowButtonIndex].owningMedicine = tool;
        //    //existIndex = owningSpecialMedicineList.Count - 1;
        //}
        //else
        //{
        //    wholeMedicineButtonList[nowButtonIndex].owningMedicine = owningSpecialMedicineList[existIndex];
        //}

        int existTileIndex = -1;
        SpecialStoreTile tile = (SpecialStoreTile)nowTileButton.tileClass;
        OwningMedicineClass selledTool;
        for (int i = 0; i < tile.selledSpecialMedicineList.Count; i++)
        {
            if (wholeMedicineButtonList[nowButtonIndex].owningMedicine.medicineIndex == tile.selledSpecialMedicineList[i].medicineIndex)
            {
                existTileIndex = i;
                break;
            }
        }
        if (existTileIndex == -1)
        {
            selledTool = new OwningMedicineClass();
            selledTool.medicineIndex = wholeMedicineButtonList[nowButtonIndex].owningMedicine.medicineIndex;
            selledTool.medicineQuantity = quant;
            tile.selledSpecialMedicineList.Add(selledTool);
        }
        else
        {
            tile.selledSpecialMedicineList[existTileIndex].medicineQuantity += quant;
        }
        exploreManager.OnBuySpecialMedicine(wholeMedicineButtonList[nowButtonIndex].owningMedicine.medicineIndex, quant);

        saveData.coin -= quant * wholeMedicineButtonList[nowButtonIndex].medicineClass.cost;
         TabletManager.inst.UpdateBill(BillReason.medicineBuy, false, quant * wholeMedicineButtonList[nowButtonIndex].medicineClass.cost);
        wholeMedicineButtonList[nowButtonIndex].medicineQuant -= quant;
        if (wholeMedicineButtonList[nowButtonIndex].medicineQuant <= 0)
        {
            wholeMedicineButtonList[nowButtonIndex].zeroMedicine = true;
        }
        coinText.text = saveData.coin.ToString();

        wholeMedicineButtonList[nowButtonIndex].owningMedicine.medicineQuantity += quant;
        wholeMedicineButtonList[nowButtonIndex].quantityText.text = wholeMedicineButtonList[nowButtonIndex].medicineQuant.ToString();
        if (wholeMedicineButtonList[nowButtonIndex].zeroMedicine)
        {
            ListButton();
        }
        nowButtonIndex = -1;
        gameManager.SaveJson();
    }

    protected void ListButton()
    {
        int buttonQuantity = 0;

        for (int i = 0; i < wholeMedicineButtonList.Count; i++)
        {

            if (!wholeMedicineButtonList[i].zeroMedicine)
            {
                wholeMedicineButtonList[i].buttonObject.SetActive(true);
                wholeMedicineButtonList[i].buttonRect.anchoredPosition = new Vector2(0, -90 - buttonQuantity * 180);
                buttonQuantity++;
            }
            else
            {
                wholeMedicineButtonList[i].buttonObject.SetActive(false);
            }

        }


        scrollContent.sizeDelta = new Vector2(0, 180 * buttonQuantity);
    }

    public override void TileOpen(TileButtonClass tile)
    {
        base.TileOpen(tile);
        appearingSpecialMedicineList = new List<SpecialMedicineClass>();
        for (int i = 0; i < specialMedicineDataList.Count; i++)
        {
            appearingSpecialMedicineList.Add(specialMedicineDataList[i]);
        }
        StoreStart();
    }


    //그 버튼눌러서 1개씩 올라가느넉
    //public void OnQuantityChangeButton(bool plus)
    //{
    //    int one;
    //    if (plus)
    //    {
    //        one = 1;
    //    }
    //    else
    //    {
    //        one = -1;
    //    }
    //    float ratio = 1.0f / wholeMedicineButtonList[nowButtonIndex].toolQuant;
    //    //float value = (int)(wholeMedicineButtonList[nowButtonIndex].ToolQuant * quantSlider.value) *ratio;
    //    //quantSlider.value = value;

    //    if (quantSlider.value + one * ratio < 0)
    //    {
    //        quantSlider.value = 0;
    //    }
    //    else if (quantSlider.value + one * ratio > 1)
    //    {
    //        quantSlider.value = 1;
    //    }
    //    else
    //    {
    //        quantSlider.value += one * ratio;
    //    }

    //    sliderPopupQuantityText.text = ((int)(wholeMedicineButtonList[nowButtonIndex].toolQuant * quantSlider.value)).ToString();

    //}

    //public void OnSliderValueChange()
    //{
    //    sliderPopupQuantityText.text = ((int)(wholeMedicineButtonList[nowButtonIndex].toolQuant * quantSlider.value)).ToString();
    //}



    public void OnNotEnoughCoinPopupButton()
    {
        nowPopup = false;
        notEnoughCoinPopup.SetActive(false);

    }

    public void OnPopupBackButton()
    {
        nowPopup = false;
        //if (appearingSpecialMedicineList[nowButtonIndex].usedOnce)
        //{
        //    sliderPopupParent.SetActive(false);
        //}
        //else
        //{
        //    justBuyPopupParent.SetActive(false);
        //}
        justBuyPopupParent.SetActive(false);
        nowButtonIndex = -1;

    }

}
