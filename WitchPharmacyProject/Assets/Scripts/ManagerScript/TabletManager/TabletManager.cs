using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabletManager : MonoBehaviour
{
    static public TabletManager inst;

    GameManager gameManager;
    SaveDataClass saveData;
    [SerializeField]
    TabletBillManager tabletBillManager;

    // Start is called before the first frame update
    void Awake()
    {
        if(inst == null)
        {
            DontDestroyOnLoad(gameObject);
            inst = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        gameManager = GameManager.singleTon;
        saveData = gameManager.saveData;
    }

    //웬동네에서 다불러옴.
    public void UpdateBill(BillReason reason, bool isPlus, int coin)
    {
        if (saveData.billWrapperList.Count <= saveData.nowDay)
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
            buyBill.isPlus = false;
            saveData.billWrapperList[saveData.nowDay].billList.Add(buyBill);
        }
        else
        {
            buyBill.changedCoin += coin;
        }
        tabletBillManager.UpdateBill();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
