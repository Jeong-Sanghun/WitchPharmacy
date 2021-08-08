﻿using System.Collections;
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
    TabletDocumentManager tabletDocumentManager;
    [SerializeField]
    TabletSaveManager tabletSaveManager;
    [SerializeField]
    TabletTreeterManager tabletTreeterManager;
    [SerializeField]
    GameObject tabletCanvasParent;
    [SerializeField]
    GameObject wholeTabletParent;
    [SerializeField]
    GameObject homeButton;
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
        wholeTabletParent.SetActive(false);
    }

    public void OnTabletButton(bool open)
    {
        tabletCanvasParent.SetActive(open);
    }

    public void TabletOpenButtonActive(bool active)
    {
        wholeTabletParent.SetActive(active);
        homeButton.SetActive(true);
        if (active == false)
        {
            tabletBillManager.WholeButtonOff();
            tabletDocumentManager.WholeButtonOff();
            tabletSaveManager.WholeButtonOff();
        }
    }

    public void OnHomeButton()
    {
        wholeTabletParent.SetActive(true);
        tabletBillManager.WholeButtonOff();
        tabletDocumentManager.WholeButtonOff();
        tabletSaveManager.WholeButtonOff();
        tabletTreeterManager.WholeButtonOff();
    }

    public void ForceSaveButtonActive(bool forceSave,SaveTime saveTime)
    {

        tabletCanvasParent.SetActive(true);
        tabletSaveManager.SaveCanvasActive(true);
        if (forceSave)
        {
            homeButton.SetActive(false);
            tabletSaveManager.OnOpenedForceSave(saveTime);
        }
    }

    //웬동네에서 다불러옴.
    public void UpdateBill(BillReason reason, bool isPlus, int coin)
    {

        tabletBillManager.UpdateBill(reason,isPlus,coin);
    }

    public void UpdateDocument(OwningDocumentClass doc)
    {
        tabletDocumentManager.SetupDocument(doc);
    }


    public void OnNextDay()
    {
        tabletBillManager.SetBill();
        tabletTreeterManager.OnNextDay();
    }

    //tabletSavemanager에서 불러옴
    public void SetOnSaveDataLoad()
    {
        tabletBillManager.NewLoadedBill();
        tabletDocumentManager.InitializeDocument();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
