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
    [SerializeField]
    GameObject tabletCanvasParent;

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

    public void OnTabletButton(bool open)
    {
        tabletCanvasParent.SetActive(open);
    }

    //웬동네에서 다불러옴.
    public void UpdateBill(BillReason reason, bool isPlus, int coin)
    {

        tabletBillManager.UpdateBill(reason,isPlus,coin);
    }

    public void SetTodayBill()
    {
        tabletBillManager.SetBill();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
