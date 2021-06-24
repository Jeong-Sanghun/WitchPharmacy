using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;

public class StoreToolManager : MonoBehaviour,IStore
{
    const int rQuantityToSell = 10;
    GameManager gameManager;
    SaveDataClass saveData;
    List<StoreToolClass> storeToolDataList;
    List<OwningToolClass> owningToolList;
    //Dictionary<int, int> owningMedicineDictionary;
    //List<CookedMedicineData> cookedMedicineDataList;
    //위는 기본적인 매니저들 그리고 데이터들

    //스크롤 뷰에 들어가있는 content들 위아래 길이조정 해줘야함.
    //버튼도 이 아래에 생성할거여서 따로 드래그앤드롭 해줘야함. GetChild로 받아왔는데 순서꼬이면 귀찮아짐.
    [SerializeField]
    RectTransform scrollContent;

    [SerializeField]
    GameObject toolButtonPrefab;

    //스크롤뷰에 들어가는 약재버튼 하나. 프리팹으로 만들어서 Instantiate해줄거.
    //프리팹들에 들어가는 것들을 다 받아오고, 시작할 때만 설정해주고 Instantiate해주고 그다음 버튼 새로 설정하고 Instantiate해주고 반복.
    [SerializeField]
    Text prefabButtonName;
    [SerializeField]
    Image prefabButtonIcon;
    [SerializeField]
    Text prefabButtonQuantity;
    [SerializeField]
    Text prefabButtonCost;
    [SerializeField]
    Text prefabButtonToolTip;


    [SerializeField]
    GameObject sliderPopupParent;
    [SerializeField]
    Text sliderPopupQuantityText;
    [SerializeField]
    Slider quantSlider;

    [SerializeField]
    GameObject justBuyPopupParent;

    [SerializeField]
    GameObject notEnoughCoinPopup;
    [SerializeField]
    Text coinText;

    List<StoreToolButton> wholeToolButtonList;
    int nowButtonIndex;
    bool nowPopup;


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.singleTon;
        saveData = gameManager.saveData;
        storeToolDataList = gameManager.storeToolDataWrapper.storeToolDataList;
        owningToolList = saveData.owningToolList;
        wholeToolButtonList = new List<StoreToolButton>();
        int buttonIndex = 0;
        for (int i = 0; i < storeToolDataList.Count; i++)
        {

            int quantity = rQuantityToSell;
            StoreToolClass tool = storeToolDataList[i];
            StringBuilder nameBuilder = new StringBuilder(tool.name);
            tool.LoadImage();
            prefabButtonIcon.sprite = tool.toolImage;
            prefabButtonName.text = nameBuilder.ToString();
            prefabButtonToolTip.text = tool.toolTip;
            prefabButtonCost.text = tool.cost.ToString();
            if (!storeToolDataList[i].usedOnce)
            {
                prefabButtonQuantity.gameObject.SetActive(false);
                quantity = 1;
            }
            prefabButtonQuantity.text = quantity.ToString();

            //버튼 세팅 다 해서 instantiate하고 리스트에 넣어줌.
            //이제 이 리스트에서 active하고 위치바꿔주고 그럴거임.
            GameObject buttonObject = Instantiate(toolButtonPrefab, scrollContent.transform);
            buttonObject.SetActive(true);


            Text quantText = buttonObject.transform.GetChild(2).GetComponent<Text>();


            StoreToolButton buttonClass = new StoreToolButton(buttonObject, i, quantity, quantText);
            buttonClass.storeTool = storeToolDataList[i];
            wholeToolButtonList.Add(buttonClass);
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
        
        if (storeToolDataList[index].usedOnce)
        {
            sliderPopupParent.SetActive(true);
        }
        else
        {
            justBuyPopupParent.SetActive(true);
        }
        sliderPopupQuantityText.text = "0";
        quantSlider.value = 0;
        nowButtonIndex = index;
    }

    public void OnBuyButton()
    {
        nowPopup = false;
        int quant = (int)(wholeToolButtonList[nowButtonIndex].toolQuant * quantSlider.value);
        if (storeToolDataList[nowButtonIndex].usedOnce)
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
        else
        {
            if (saveData.coin - quant * wholeToolButtonList[nowButtonIndex].storeTool.cost < 0)
            {
                //여기서 에러메시지 표출
                notEnoughCoinPopup.SetActive(true);
                OnPopupBackButton();
                nowPopup = true;
                return;
            }
            int existIndex = -1;
            for(int i = 0; i < saveData.owningToolList.Count; i++)
            {
                if(wholeToolButtonList[nowButtonIndex].toolIndex == owningToolList[i].index)
                {
                    existIndex = i;
                    break;
                }
            }
            if(existIndex == -1)
            {
                OwningToolClass tool = new OwningToolClass();
                tool.index = wholeToolButtonList[nowButtonIndex].toolIndex;
                tool.quantity = 0;
                owningToolList.Add(tool);
                wholeToolButtonList[nowButtonIndex].owningTool = tool;
            }
            else
            {
                wholeToolButtonList[nowButtonIndex].owningTool = owningToolList[existIndex];
            }


            saveData.coin -= quant * wholeToolButtonList[nowButtonIndex].storeTool.cost;
            wholeToolButtonList[nowButtonIndex].toolQuant -= quant;
            if (wholeToolButtonList[nowButtonIndex].toolQuant <= 0)
            {
                wholeToolButtonList[nowButtonIndex].zeroTool = true;
            }
            coinText.text = saveData.coin.ToString();

            wholeToolButtonList[nowButtonIndex].owningTool.quantity += quant;
            wholeToolButtonList[nowButtonIndex].quantityText.text = wholeToolButtonList[nowButtonIndex].toolQuant.ToString();
        }
        if (wholeToolButtonList[nowButtonIndex].zeroTool)
        {
           ListButton();
        }
    }

    void ListButton()
    {
        int buttonQuantity = 0;

        for (int i = 0; i < wholeToolButtonList.Count; i++)
        {
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
        if (storeToolDataList[nowButtonIndex].usedOnce)
        {
            sliderPopupParent.SetActive(false);
        }
        else
        {
            justBuyPopupParent.SetActive(false);
        }
        nowButtonIndex = -1;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
