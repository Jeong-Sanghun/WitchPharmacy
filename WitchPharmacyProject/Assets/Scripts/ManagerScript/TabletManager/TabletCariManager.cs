using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TabletType
{
    Main,Bill,Save,DocumentFolder,Document,Dogam,Option
}
public class TabletCariManager : MonoBehaviour
{
    [SerializeField]
    MainCariDialogWrapper mainCariDialogWrapper;
    MainCariDialogCondition mainCariDialogCondition;
    TabletType nowTabletType;

    [SerializeField]
    TabletBillManager tabletBillManager;

    GameManager gameManager;
    SaveDataClass saveData;

    [SerializeField]
    GameObject minimizeParent;
    [SerializeField]
    Text minimizeTalkText;

    [SerializeField]
    GameObject maximizeParent;
    [SerializeField]
    GameObject[] maximizeButtonObjectArray;
    [SerializeField]
    Text[] maximizeButtonTextArray;
    [SerializeField]
    public Text maximizeTalkText;
    [SerializeField]
    GameObject buttonBundle;

    int nowMainDialogIndex;
    //BillCariDialog nowBillCariDialog;
    CariDialog nowCariDialog;

    bool nowInHome;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.singleton;
        saveData = gameManager.saveData;
        nowInHome = true;

        JsonManager jsonManager = new JsonManager();
        mainCariDialogCondition = jsonManager.ResourceDataLoad<MainCariDialogCondition>("TabletCariDialog/Main/MainCariDialogCondition");
        OnNextDay();
    }

    public void MaximizeButton()
    {
        if (nowTabletType != TabletType.Main)
        {
            return;
        }
        MaximizeTalk();
        SetDialogByType();
    }

    public void MinimizeButton()
    {
        if (nowTabletType != TabletType.Main)
        {
            return;
        }
        MinimizeTalk();
    }

    public void MaximizeTalk()
    {
        minimizeParent.SetActive(false);
        maximizeParent.SetActive(true);
    }

    public void MinimizeTalk()
    {
        minimizeParent.SetActive(true);
        maximizeParent.SetActive(false);
    }

    public void NextDialogButton()
    {
        nowMainDialogIndex++;
        if(nowMainDialogIndex == mainCariDialogWrapper.mainCariDialogArray.Length)
        {
            MinimizeTalk();
            nowMainDialogIndex = 0;
        }
        else
        {
            SetMainDialog();
        }
        
        
    }

    public void ChangeTabletType(TabletType type)
    {
        MinimizeTalk();
        nowTabletType = type;
        SetDialogByType();

    }

    void SetDialogByType()
    {
        switch (nowTabletType)
        {
            case TabletType.Bill:
                nowCariDialog = tabletBillManager.GetNowCariTalk();
                ChangeCariDialog();
                break;
            case TabletType.Main:
                SetMainDialog();
                break;
            default:
                RouteButtonActiveFalse();
                break;
        }
    }

    void SetMainDialog()
    {
        MainCariDialog mainDialog = mainCariDialogWrapper.mainCariDialogArray[nowMainDialogIndex];
        maximizeTalkText.text = mainDialog.dialog;
        RouteButtonActiveFalse();
        for (int i = 0; i < mainDialog.buttonTextList.Count; i++)
        {
            buttonBundle.SetActive(true);
            maximizeButtonObjectArray[i].SetActive(true);
            maximizeButtonTextArray[i].text = mainDialog.buttonTextList[i];
        }
    }

    void RouteButtonActiveFalse()
    {
        for (int i = 0; i < maximizeButtonObjectArray.Length; i++)
        {
            maximizeButtonObjectArray[i].SetActive(false);
        }

    }

    //빌 버튼에서 호출.
    public void OnBillOpenButton()
    {
        
        nowCariDialog = tabletBillManager.GetNowCariTalk();
        ChangeCariDialog();
    }

    public void OnNextDay()
    {
        JsonManager jsonManager = new JsonManager();
        nowMainDialogIndex = 0;
        mainCariDialogWrapper = null;
        if (mainCariDialogCondition.appearingDayArray.Length == 1)
        {
            mainCariDialogWrapper = jsonManager.ResourceDataLoad<MainCariDialogWrapper>("TabletCariDialog/Main/" + mainCariDialogCondition.appearingDayArray[0]);
        }
        for (int i = 0; i < mainCariDialogCondition.appearingDayArray.Length-1; i++)
        {
            int nowNum = mainCariDialogCondition.appearingDayArray[i];
            int nextNum= mainCariDialogCondition.appearingDayArray[i+1];

            if (saveData.nowDay >= nowNum && saveData.coin < nextNum)
            {
                mainCariDialogWrapper = jsonManager.ResourceDataLoad<MainCariDialogWrapper>("TabletCariDialog/Main/" + mainCariDialogCondition.appearingDayArray[nowNum]);
                break;
            }
        }
        if (mainCariDialogWrapper == null)
        {
            mainCariDialogWrapper = jsonManager.ResourceDataLoad<MainCariDialogWrapper>("TabletCariDialog/Main/" + mainCariDialogCondition.appearingDayArray[mainCariDialogCondition.appearingDayArray.Length-1]);
        }
        mainCariDialogWrapper.Parse();
    }

    void ChangeCariDialog()
    {
        maximizeTalkText.text = nowCariDialog.dialog;
    }

    


    // Update is called once per frame
    
}
