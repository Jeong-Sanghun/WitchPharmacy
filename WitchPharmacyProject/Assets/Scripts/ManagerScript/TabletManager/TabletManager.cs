using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TabletComponent
{
    Bill,Document, Treeter
}
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
    public TabletCariManager tabletCariManager;
    [SerializeField]
    GameObject tabletCanvasParent;
    [SerializeField]
    GameObject wholeTabletParent;
    [SerializeField]
    GameObject homeButton;
    [SerializeField]
    GameObject[] buttonHighlightObject;
    [SerializeField]
    GameObject tabletHighlightObject;

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
        gameManager = GameManager.singleton;
        saveData = gameManager.saveData;
        wholeTabletParent.SetActive(false);
        SetHighlightButton();

    }

    public void OnTabletButton(bool open)
    {
        tabletCanvasParent.SetActive(open);
        if(open == false)
        {
            bool allFalse = true;
            for(int i = 0; i < buttonHighlightObject.Length; i++)
            {
                if(buttonHighlightObject[i].activeSelf == true)
                {
                    allFalse = false;
                    break;
                }
            }
            if(allFalse == true)
            {
                tabletHighlightObject.SetActive(false);
            }
            else
            {
                tabletHighlightObject.SetActive(true);
            }
        }
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
        tabletCariManager.ChangeTabletType(TabletType.Main);
        tabletBillManager.WholeButtonOff();
        tabletDocumentManager.WholeButtonOff();
        tabletSaveManager.WholeButtonOff();
        tabletTreeterManager.WholeButtonOff();
    }

    public void ForceSaveButtonActive(bool forceSave)
    {
        
        tabletCariManager.SetCariActive(false);
        tabletCanvasParent.SetActive(true);
        tabletSaveManager.SaveCanvasActive(true);
        
        if (forceSave)
        {
            homeButton.SetActive(false);
            tabletSaveManager.OnOpenedForceSave();
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
        tabletCariManager.OnNextDay();
    }

    //tabletSavemanager에서 불러옴
    public void SetOnSaveDataLoad()
    {
        saveData = gameManager.saveData;
        tabletBillManager.NewLoadedBill();
        tabletDocumentManager.InitializeDocument();
        SetHighlightButton();
        tabletTreeterManager.InitializeButtons();
    }

    public void ButtonHighlightActive(TabletComponent component, bool active)
    {
        saveData.highlightedButton[(int)component] = active;
        buttonHighlightObject[(int)component].SetActive(active);
        if(active == true)
        {
            tabletHighlightObject.SetActive(true);
        }
    }

    public void SetHighlightButton()
    {
        bool isOneTrue = false;
        for (int i = 0; i < saveData.highlightedButton.Length; i++)
        {
            if(saveData.highlightedButton[i] == true)
            {
                isOneTrue = true;
            }
            buttonHighlightObject[i].SetActive(saveData.highlightedButton[i]);
        }
        tabletHighlightObject.SetActive(isOneTrue);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
