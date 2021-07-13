using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabletBillManager : MonoBehaviour
{

    GameManager gameManager;
    SaveDataClass saveData;
    List<OneDayBillWrapper> billWrapperList;
    UILanguagePack languagePack;
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
    


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.singleTon;
        saveData = gameManager.saveData;
    }

    void InitBill()
    {
        billTitle.text = languagePack.billTitle;
        wholeGainTitleText.text = languagePack.billWholeGain;
        wholeSpentTitleText.text = languagePack.billWholeSpend;
        prefabGainTitleText.text = languagePack.billGain;
        prefabSpentTitleText.text = languagePack.billSpend;
        billWrapperList = saveData.billWrapperList;

        int wholeGain = 0;
        int wholeSpent = 0;
        for(int i = 0; i < billWrapperList.Count;i++)
        {
            int dayGain = 0;
            int daySpent = 0;
            OneDayBillWrapper wrapper = billWrapperList[i];
            for(int j = 0; j < wrapper.billList.Count; j++)
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

            prefabGainNumberText.text = dayGain.ToString();
            prefabSpentNumberText.text = daySpent.ToString();
        }


    }

    //빌지 하나 업데이트 시키는거.
    public void UpdateBill()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
