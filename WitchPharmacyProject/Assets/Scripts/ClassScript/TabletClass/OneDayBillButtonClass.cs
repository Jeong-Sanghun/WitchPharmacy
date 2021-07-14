using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OneDayBillButtonClass
{
    public int index;
    public Text wholeGainText;
    public Text wholeSpentText;
    public bool propertySetted;

    public GameObject propertyCanvasParent;
    public Transform propertyScrollContent;
    public GameObject propertyGainLineObject;
    public GameObject propertySpentLineObject;

    public OneDayBillWrapper wrapper;

    public List<OneBillLineClass> gainBillList;
    public List<OneBillLineClass> spentBillList;


    public OneDayBillButtonClass(int _index, Text wholeGain, Text wholeSpent,OneDayBillWrapper _wrapper)
    {
        propertySetted = false;
        index = _index;
        wholeGainText = wholeGain;
        wholeSpentText = wholeSpent;
        gainBillList = new List<OneBillLineClass>();
        spentBillList = new List<OneBillLineClass>();
        wrapper = _wrapper;
    }

    public void SetProperty(GameObject parent, Transform scrollContent, GameObject gainLine, GameObject spentLine, GameObject linePrefab, UILanguagePack languagePack)
    {
        propertySetted = true;
        propertyCanvasParent = parent;
        propertyScrollContent = scrollContent;
        propertyGainLineObject = gainLine;
        propertySpentLineObject = spentLine;
        InitializeWholeProperty(linePrefab, languagePack);
    }

    public void InitializeWholeProperty(GameObject linePrefab, UILanguagePack languagePack)
    {
        for(int i = 0; i < wrapper.billList.Count; i++)
        {
            OneBillClass bill = wrapper.billList[i];
            UpdatePropertyLine(bill.reason, bill.isPlus, bill.changedCoin, linePrefab, languagePack);
        }
    }

    public void UpdatePropertyLine(BillReason reason, bool isPlus, int coin, GameObject linePrefab, UILanguagePack languagePack)
    {
        OneBillLineClass billLine = null;
        bool billLineAdded = false;
        if (isPlus)
        {
            for (int i = 0; i < gainBillList.Count; i++)
            {
                if(reason == gainBillList[i].reason)
                {
                    billLine = gainBillList[i];
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < spentBillList.Count; i++)
            {
                if(reason == spentBillList[i].reason)
                {
                    billLine = spentBillList[i];
                    break;
                }
            }
        }
        if(billLine == null)
        {
            billLineAdded = true;
            GameObject line = GameObject.Instantiate(linePrefab, propertyScrollContent);
            line.SetActive(true);
            Text reasonText = line.transform.GetChild(1).GetComponent<Text>();
            Text coinText = line.transform.GetChild(2).GetComponent<Text>();
            reasonText.text = languagePack.reasonArray[(int)reason];
            coinText.text = coin.ToString();
            billLine = new OneBillLineClass(reason, isPlus, coin,reasonText, coinText,line);
            if (isPlus)
            {
                gainBillList.Add(billLine);
            }
            else
            {
                spentBillList.Add(billLine);
            }
        }
        else
        {
            int nowCoin = billLine.coin + coin;
            billLine.coin += coin;
            billLine.oneLineCoinText.text = nowCoin.ToString();
        }
        if(gainBillList.Count == 0)
        {
            propertyGainLineObject.SetActive(false);
        }
        if(spentBillList.Count == 0)
        {
            propertySpentLineObject.SetActive(false);
        }

        if (billLineAdded)
        {
            Vector3 gainStartPos = new Vector3(0,-70,0);
            Vector3 spentStartPos = Vector3.zero;
            int nowButtonCount = 0;
            bool gainZero = false;
            bool spentZero = false;
            if(gainBillList.Count == 0)
            {
                spentStartPos = new Vector3(0, -70, 0);
                gainZero = true;
                if (spentBillList.Count == 0)
                {
                    return;
                }
            }
            if (spentBillList.Count == 0)
            {
                spentZero = true;
            }

            if (!gainZero)
            {
                propertyGainLineObject.SetActive(true);
                propertyGainLineObject.GetComponent<RectTransform>().anchoredPosition = gainStartPos;
                nowButtonCount++;
                for (int i = 0; i < gainBillList.Count; i++)
                {
                    gainBillList[i].lineObject.GetComponent<RectTransform>().anchoredPosition = gainStartPos + (new Vector3(0, -120, 0)) * (nowButtonCount);
                    
                    nowButtonCount++;
                }
            }
            else
            {
                propertyGainLineObject.SetActive(false);
            }

            if (!spentZero)
            {
                propertySpentLineObject.SetActive(true);
                if(!gainZero)
                    spentStartPos = gainStartPos + (new Vector3(0, -120, 0)) * (nowButtonCount + 1);
                propertySpentLineObject.GetComponent<RectTransform>().anchoredPosition = spentStartPos;
                nowButtonCount++;
                int count = 0;
                for (int i = 0; i < spentBillList.Count; i++)
                {
                    spentBillList[i].lineObject.GetComponent<RectTransform>().anchoredPosition = spentStartPos + (new Vector3(0, -120, 0)) * (count + 1);
                    count++;
                    nowButtonCount++;
                }
            }
            else
            {
                propertySpentLineObject.SetActive(false);
            }
            propertyScrollContent.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 120 * nowButtonCount);
        }
        
    }
}
