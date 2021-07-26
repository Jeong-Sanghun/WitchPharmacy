using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreMeasureToolManager : MonoBehaviour
{
    public class MeasureToolButton
    {
        public int dataIndex;
        public MeasureToolData data;
        public RectTransform buttonRect;
        public Button buttonComponent;
        public bool isActive;
    }
    GameManager gameManager;
    SaveDataClass saveData;
    [SerializeField]
    StoreManager storeManager;
    [SerializeField]
    GameObject buttonPrefab;
    [SerializeField]
    Text prefabTitleText;
    [SerializeField]
    Text prefabToolTipText;
    [SerializeField]
    Text prefabCostText;
    [SerializeField]
    GameObject prefabLockObject;
    [SerializeField]
    Image prefabImage;
    [SerializeField]
    Transform buttonContent;
    [SerializeField]
    GameObject popUpObject;
    [SerializeField]
    GameObject noCoinPopUpObject;

    MeasureToolDataWrapper dataWrapper;

    List<MeasureToolButton> wholeButtonList;

    
    int nowButtonIndex = -1;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.singleTon;
        saveData = gameManager.saveData;
        dataWrapper = gameManager.jsonManager.ResourceDataLoad<MeasureToolDataWrapper>("MeasureToolDataWrapper");
        wholeButtonList = new List<MeasureToolButton>();
        int nowButtonNumber = 0;
        for(int i = 0; i < dataWrapper.measureToolDataList.Count; i++)
        {
            MeasureToolData data = dataWrapper.measureToolDataList[i];
            if (saveData.owningMeasureToolList.Contains(i))
            {
                continue;
            }
            
            prefabTitleText.text = data.ingameName;
            prefabToolTipText.text = data.toolTip;
            prefabImage.sprite = data.LoadImage();
            prefabCostText.text = data.cost.ToString();
            bool locked = true;
            for(int j = 0; j < saveData.researchSaveData.endMeasureToolResearchList.Count; j++)
            {
                if (saveData.researchSaveData.endMeasureToolResearchList[j].Contains(data.fileName))
                {
                    locked = false;
                    break;
                }
            }
            if (i > 4)
            {
                if (!saveData.owningMeasureToolList.Contains(i - 4))
                {
                    locked = true;
                }
            }

            if (!locked)
            {
                prefabLockObject.SetActive(false);
            }
            else
            {
                prefabLockObject.SetActive(true);
            }

            GameObject inst = Instantiate(buttonPrefab, buttonContent);
            inst.SetActive(true);
            Button comp = inst.GetComponent<Button>();
            RectTransform rect = inst.GetComponent<RectTransform>();
            MeasureToolButton buttonClass = new MeasureToolButton();
            buttonClass.dataIndex = i;
            buttonClass.buttonComponent = comp;
            buttonClass.buttonRect = rect;
            buttonClass.data = data;
            buttonClass.isActive = true;
            if (locked)
            {
                comp.interactable = false;
            }
            wholeButtonList.Add(buttonClass);
            rect.anchoredPosition = new Vector2(0, -90 - nowButtonNumber * 180);
            int dele = nowButtonNumber;
            comp.onClick.AddListener(() => PopupOn(dele));
            nowButtonNumber++;
            

        }
        buttonContent.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 180 * nowButtonNumber);
        storeManager.ChangeCoinText();
    }

    void PopupOn(int index)
    {
        nowButtonIndex = index;
        if(saveData.coin >= wholeButtonList[nowButtonIndex].data.cost)
        {
            popUpObject.SetActive(true);
        }
        else
        {
            noCoinPopUpObject.SetActive(true);
        }
        
    }

    public void NotEnoughCoinButton()
    {
        noCoinPopUpObject.SetActive(false);

    }

    public void PopupDown()
    {
        nowButtonIndex = -1;
        popUpObject.SetActive(false);
    }

    public void BuyButton()
    {
        if(nowButtonIndex == -1)
        {
            return;
        }
        RemoveButton(nowButtonIndex);
        saveData.owningMeasureToolList.Add(wholeButtonList[nowButtonIndex].dataIndex);
        saveData.coin -= wholeButtonList[nowButtonIndex].data.cost;
        storeManager.ChangeCoinText();
        TabletManager.inst.UpdateBill(BillReason.measureToolBuy, false, wholeButtonList[nowButtonIndex].data.cost);

        PopupDown();
    }

    void RemoveButton(int index)
    {
        wholeButtonList[index].isActive = false;
        int buttonNumber = 0;
        for(int i = 0; i < wholeButtonList.Count; i++)
        {
            if(wholeButtonList[i].isActive == false)
            {
                wholeButtonList[i].buttonRect.gameObject.SetActive(false);
                continue;
            }
            wholeButtonList[i].buttonRect.anchoredPosition = new Vector2(0, -90 - buttonNumber * 180);
            buttonNumber++;
        }
        buttonContent.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 180 * buttonNumber);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
