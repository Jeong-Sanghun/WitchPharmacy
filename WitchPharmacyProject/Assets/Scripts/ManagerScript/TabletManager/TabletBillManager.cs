using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabletBillManager : MonoBehaviour
{

    GameManager gameManager;
    SaveDataClass saveData;
    List<OneDayBillWrapper> billWrapperList;
    List<OneDayBillButtonClass> wholeBillButtonClass;
    UILanguagePack languagePack;
    [SerializeField]
    GameObject billCanvasParent;
    [SerializeField]
    Text billTitle;
    [SerializeField]
    Text wholeGainTitleText;
    [SerializeField]
    Text wholeSpentTitleText;

    [SerializeField]
    Text wholeGainNumberText;
    [SerializeField]
    Text wholeSpentNumberText;

    [SerializeField]
    GameObject oneDayBillPrefab;
    [SerializeField]
    Transform scrollContent;
    [SerializeField]
    Text prefabDayText;
    [SerializeField]
    Text prefabGainTitleText;
    [SerializeField]
    Text prefabGainNumberText;
    [SerializeField]
    Text prefabSpentTitleText;
    [SerializeField]
    Text prefabSpentNumberText;
    [SerializeField]
    GameObject propertyParentPrefab;
    [SerializeField]
    Text propertyPrefabDayText;
    [SerializeField]
    Transform propertyParentTransform;
    [SerializeField]
    GameObject propertyOneLinePrefab;
    [SerializeField]
    Text prefabOneLineReasonText;
    [SerializeField]
    Text prefabOneLineCoinText;
    [SerializeField]
    Text prefabOneLineGainText;
    [SerializeField]
    Text prefabOneLineSpentText;
    


    GameObject nowActivePropertyObject;


    int wholeGain = 0;
    int wholeSpent = 0;
    int nowButtonIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        InitBill();

    }

    void InitBill()
    {
        wholeGain = 0;
        wholeSpent = 0;
        nowButtonIndex = 0;
        gameManager = GameManager.singleton;
        saveData = gameManager.saveData;
        languagePack = gameManager.languagePack;
        wholeBillButtonClass = new List<OneDayBillButtonClass>();
        billTitle.text = languagePack.billTitle;
        prefabOneLineSpentText.text = languagePack.billSpend;
        prefabOneLineGainText.text = languagePack.billGain;
        wholeGainTitleText.text = languagePack.billWholeGain;
        wholeSpentTitleText.text = languagePack.billWholeSpend;
        prefabGainTitleText.text = languagePack.billGain;
        prefabSpentTitleText.text = languagePack.billSpend;
        billWrapperList = saveData.billWrapperList;
        wholeGainNumberText.text = "0";
        wholeSpentNumberText.text = "0";
        
        oneDayBillPrefab.SetActive(false);

        while(billWrapperList.Count < saveData.nowDay + 1)
        {
            billWrapperList.Add(new OneDayBillWrapper());
        }

        for(int i = 0; i < billWrapperList.Count;i++)
        {
            MakeNewButton(billWrapperList[i]);
        }

    }
    //날짜 지나서 새로운거 사면 이거 호출.
    void MakeNewButton(OneDayBillWrapper wrapper)
    {
        int dayGain = 0;
        int daySpent = 0;
        //OneDayBillWrapper wrapper = billWrapperList[i];
        for (int j = 0; j < wrapper.billList.Count; j++)
        {
            OneBillClass bill = wrapper.billList[j];
            if (bill.isPlus)
            {
                dayGain += bill.changedCoin;
                wholeGain += bill.changedCoin;
            }
            else
            {
                wholeSpent += bill.changedCoin;
                daySpent += bill.changedCoin;
            }
        }
        prefabDayText.text = (nowButtonIndex+1).ToString() + languagePack.billDayth;
        //prefabGainNumberText.text = dayGain.ToString();
        //prefabSpentNumberText.text = daySpent.ToString();
        GameObject inst = Instantiate(oneDayBillPrefab, scrollContent);
        inst.SetActive(true);
        //존나조심;
        Button dayButton = inst.transform.GetChild(2).GetComponent<Button>();
        //Button spentButton = inst.transform.GetChild(3).GetComponent<Button>();
        int delegateIndex = nowButtonIndex;
        dayButton.onClick.AddListener(() => DayButtonDown(delegateIndex));
        //spentButton.onClick.AddListener(() => DayGainButtonDown(delegateIndex));
        Text gainText = inst.transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<Text>();
        gainText.text = dayGain.ToString();
        //gainText.text = "게인 택스트";

        Text spentText = inst.transform.GetChild(3).GetChild(0).GetChild(0).GetComponent<Text>();
        spentText.text = daySpent.ToString();
        //spentText.text = "스펜트 택스트";
        OneDayBillButtonClass billButton = new OneDayBillButtonClass(nowButtonIndex,inst, gainText, spentText,wrapper);
        wholeBillButtonClass.Add(billButton);
        inst.GetComponent<RectTransform>().anchoredPosition = new Vector3(-255f, -560 + (-400) * nowButtonIndex, 0);

        wholeGainNumberText.text = wholeGain.ToString();
        wholeSpentNumberText.text = wholeSpent.ToString();

        nowButtonIndex++;
        scrollContent.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 420 * nowButtonIndex + 420);
    }


    //빌지 하나 업데이트 되면 이거 
    void UpdateBillButton(int index)
    {
        OneDayBillWrapper wrapper = wholeBillButtonClass[index].wrapper;
        OneDayBillButtonClass buttonClass = wholeBillButtonClass[index];
        int dayGain = 0;
        int daySpent = 0;
        wholeGain = 0;
        wholeSpent = 0;
        for (int j = 0; j < wrapper.billList.Count; j++)
        {
            OneBillClass bill = wrapper.billList[j];
            if (bill.isPlus)
            {
                dayGain += bill.changedCoin;
                wholeGain += bill.changedCoin;
            }
            else
            {
                daySpent += bill.changedCoin;
                wholeSpent += bill.changedCoin;
            }
        }
        for(int i = 0; i < billWrapperList.Count; i++)
        {
            if(wrapper == billWrapperList[i])
            {
                continue;
            }
            for(int j = 0; j < billWrapperList[i].billList.Count; j++)
            {
                OneBillClass bill = billWrapperList[i].billList[j];
                if (bill.isPlus)
                {
                    wholeGain += bill.changedCoin;
                }
                else
                {
                    wholeSpent += bill.changedCoin;
                }
            }
        }
        wholeGainNumberText.text = wholeGain.ToString();
        wholeSpentNumberText.text = wholeSpent.ToString();
        buttonClass.wholeGainText.text = dayGain.ToString();
        buttonClass.wholeSpentText.text = daySpent.ToString();
        
    }

    public void NewLoadedBill()
    {
        if(wholeBillButtonClass!= null)
        {
            for (int i = 0; i < wholeBillButtonClass.Count; i++)
            {
                Destroy(wholeBillButtonClass[i].oneDayButton);
                Destroy(wholeBillButtonClass[i].propertyCanvasParent);
            }
        }
        InitBill();
        

    }

    public void SetBill()
    {
        while (saveData.billWrapperList.Count < saveData.nowDay + 1)
        {
            OneDayBillWrapper wrapper = new OneDayBillWrapper();
            saveData.billWrapperList.Add(wrapper);
        }
        while (wholeBillButtonClass.Count < saveData.nowDay + 1)
        {
            MakeNewButton(billWrapperList[nowButtonIndex]);
        }
    }

    //외부에서 불러옴. 타블렛매니저 통해서 다들어옴. 무언가 살 때 이루어짐.
    public void UpdateBill(BillReason reason, bool isPlus, int coin)
    {
        TabletManager.inst.ButtonHighlightActive(TabletComponent.Bill, true);
        while (saveData.billWrapperList.Count < saveData.nowDay + 1)
        {
            OneDayBillWrapper wrapper = new OneDayBillWrapper();
            saveData.billWrapperList.Add(wrapper);
        }
        OneBillClass buyBill = null;
        for (int i = 0; i < saveData.billWrapperList[saveData.nowDay].billList.Count; i++)
        {
            OneBillClass bill = saveData.billWrapperList[saveData.nowDay].billList[i];
            if (bill.reason == reason)
            {
                buyBill = bill;
                break;
            }
        }
        if (buyBill == null)
        {
            buyBill = new OneBillClass();
            buyBill.changedCoin = coin;
            buyBill.reason = reason;
            buyBill.isPlus = isPlus;
            saveData.billWrapperList[saveData.nowDay].billList.Add(buyBill);
        }
        else
        {
            buyBill.changedCoin += coin;
        }
        bool newButton = false;
        while(wholeBillButtonClass.Count < saveData.nowDay + 1)
        {
            newButton = true;
            MakeNewButton(billWrapperList[nowButtonIndex]);
        }
        if (!newButton)
        {
            UpdateBillButton(saveData.nowDay);
            if (wholeBillButtonClass[saveData.nowDay].propertySetted)
            {
                wholeBillButtonClass[saveData.nowDay].UpdatePropertyLine(reason, isPlus, coin, propertyOneLinePrefab, languagePack);
            }
        }
    }

    public void BillOpenCloseButton(bool open)
    {
        if (open)
        {
            TabletManager.inst.ButtonHighlightActive(TabletComponent.Bill, false);
        }
        billCanvasParent.SetActive(open);
    }

    public void OnPropertyBackButton()
    {
        nowActivePropertyObject.SetActive(false);
    }

    public void WholeButtonOff()
    {
        if(nowActivePropertyObject!=null)
            nowActivePropertyObject.SetActive(false);

        billCanvasParent.SetActive(false);
    }

    void DayButtonDown(int index)
    {
        GameObject propertyParent = wholeBillButtonClass[index].propertyCanvasParent;
        if (!wholeBillButtonClass[index].propertySetted)
        {
            propertyParent = MakeProperty(index);
        }
        
        nowActivePropertyObject = propertyParent;
        propertyParent.SetActive(true);
    }
   

    GameObject MakeProperty(int index)
    {
        OneDayBillButtonClass billButtonClass = wholeBillButtonClass[index];

        propertyPrefabDayText.text = (index+1).ToString() + languagePack.billDayth;
        GameObject propertyInst = Instantiate(propertyParentPrefab, propertyParentTransform);
        propertyInst.SetActive(false);
        billButtonClass.propertyCanvasParent = propertyInst;
        Transform content = propertyInst.transform.GetChild(1).GetChild(0).GetChild(0);
        GameObject gainLine = content.GetChild(0).gameObject;
        GameObject spentLine = content.GetChild(1).gameObject;
        billButtonClass.SetProperty(propertyInst, content, gainLine, spentLine,propertyOneLinePrefab,languagePack);
        return propertyInst;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
