using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class TabletSaveManager : MonoBehaviour
{
    
    GameManager gameManager;
    TabletManager tabletManager;

    [SerializeField]
    GameObject saveLoadCanvas;
    [SerializeField]
    GameObject saveButtonCanvas;
    [SerializeField]
    GameObject[] getOutButtons;
    [SerializeField]
    Text[] buttonTextArray;

    SaveDataTime[] saveDataTimeArray;
    UILanguagePack languagePack;

    bool isSavingMode = false;
    bool isForceSave = false;
    SaveTime nowSaveTime;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.singleTon;
        tabletManager = TabletManager.inst;
        languagePack = gameManager.languagePack;
        saveDataTimeArray = gameManager.saveDataTimeWrapper.saveDataTimeList;
        for(int i = 0; i < 4; i++)
        {
            SetButtonText(i);
        }
    }

    void SetButtonText(int index)
    {
        StringBuilder builder;
        if(index == 0)
        {
            builder = new StringBuilder(languagePack.autoSave);
        }
        else
        {
            builder = new StringBuilder(languagePack.slot);
            builder.Append(" ");
            builder.Append(index.ToString());
        }
        builder.Append(" : ");
        if (saveDataTimeArray[index].day == -1)
        {
            builder.Append(languagePack.slotEmpty);
        }
        else
        {
            builder.Append((saveDataTimeArray[index].day+1).ToString());
            builder.Append(languagePack.slotDay);
            builder.Append(" ");
            builder.Append(languagePack.saveTimeArray[(int)saveDataTimeArray[index].saveTime]);
        }
        buttonTextArray[index].text = builder.ToString();

    }

    public void SaveCanvasActive(bool active)
    {
        saveLoadCanvas.SetActive(active);
    }

    public void OnOpenedForceSave(SaveTime saveTime)
    {
        isForceSave = true;
        nowSaveTime = saveTime;
        for(int i = 0; i < 4; i++)
        {
            SetButtonText(i);
        }
        for (int i = 0; i < getOutButtons.Length; i++)
        {
            getOutButtons[i].SetActive(false);
        }

    }

    public void SaveOrLoadButtonCanvasActive(bool isSave)
    {
        isSavingMode = isSave;
        saveButtonCanvas.SetActive(true);

    }

    public void SaveOrLoadButtonCanvasGetout()
    {
        saveButtonCanvas.SetActive(false);
    }

    public void SaveOrLoadButtonDown(int index)
    {

        if (isSavingMode)
        {
            gameManager.SaveJson(index,nowSaveTime);
            if (isForceSave)
            {
                for (int i = 0; i < getOutButtons.Length; i++)
                {
                    getOutButtons[i].SetActive(true);
                }
                isForceSave = false;
                tabletManager.OnTabletButton(false);
                SaveOrLoadButtonCanvasGetout();
                SaveCanvasActive(false);
               
               // gameManager.sceneManager.lastSceneName = 
                gameManager.sceneManager.LoadScene(gameManager.saveData.nextLoadSceneName);

            }
        }
        else
        {
            tabletManager.OnTabletButton(false);
            SaveOrLoadButtonCanvasGetout();
            SaveCanvasActive(false);
            gameManager.LoadJson(index);
            tabletManager.SetOnSaveDataLoad();

        }
        for (int i = 0; i < 4; i++)
        {
            SetButtonText(i);
        }

    }
}
