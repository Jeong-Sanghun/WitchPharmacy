using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreOtherToolManager : MonoBehaviour
{
    public class OtherToolButton
    {
        public int dataIndex;
        public OtherToolData data;
        public RectTransform buttonRect;
        public Button buttonComponent;
        public GameObject lockObject;
        public bool researched;
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

    OtherToolDataWrapper dataWrapper;

    List<OtherToolButton> wholeButtonList;


    int nowButtonIndex = -1;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.singleton;
        saveData = gameManager.saveData;
        dataWrapper = gameManager.jsonManager.ResourceDataLoad<OtherToolDataWrapper>("OtherToolDataWrapper");
        wholeButtonList = new List<OtherToolButton>();
        int nowButtonNumber = 0;
        for (int i = 0; i < dataWrapper.otherToolDataList.Count; i++)
        {
            OtherToolData data = dataWrapper.otherToolDataList[i];

            if (saveData.owningOtherToolList.Contains(data.fileName))
            {
                continue;
            }
            OtherToolButton buttonClass = new OtherToolButton();
            buttonClass.researched = false;
            prefabTitleText.text = data.ingameName;
            prefabToolTipText.text = data.toolTip;
            prefabImage.sprite = data.LoadImage();
            prefabCostText.text = data.cost.ToString();
            bool locked = true;
            if(data.needResearch == true)
            {
                for (int j = 0; j < saveData.researchSaveData.endOtherToolResearchList.Count; j++)
                {
                    if (saveData.researchSaveData.endOtherToolResearchList[j].Contains(data.fileName))
                    {
                        locked = false;
                        buttonClass.researched = true;
                        break;
                    }
                }
            }
            else
            {
                locked = false;
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

            buttonClass.lockObject = inst.transform.GetChild(4).gameObject;
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
        if (saveData.coin >= wholeButtonList[nowButtonIndex].data.cost)
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
        if (nowButtonIndex == -1)
        {
            return;
        }
        RemoveButton(nowButtonIndex);
        saveData.owningOtherToolList.Add(wholeButtonList[nowButtonIndex].data.fileName);
        saveData.coin -= wholeButtonList[nowButtonIndex].data.cost;
        storeManager.ChangeCoinText();
        TabletManager.inst.UpdateBill(BillReason.otherToolBuy, false, wholeButtonList[nowButtonIndex].data.cost);

        for (int i = 0; i < wholeButtonList.Count; i++)
        {
            if (wholeButtonList[i].dataIndex > 4)
            {
                if (wholeButtonList[i].dataIndex - 4 == wholeButtonList[nowButtonIndex].dataIndex)
                {
                    if (wholeButtonList[i].researched == true)
                    {
                        wholeButtonList[i].buttonComponent.interactable = true;
                        wholeButtonList[i].lockObject.SetActive(false);
                    }
                }
            }
        }
        PopupDown();
    }

    void RemoveButton(int index)
    {
        wholeButtonList[index].isActive = false;
        int buttonNumber = 0;
        for (int i = 0; i < wholeButtonList.Count; i++)
        {
            if (wholeButtonList[i].isActive == false)
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
