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
    GameObject loadButton;
    [SerializeField]
    Text[] buttonTextArray;
    [SerializeField]
    Button saveButton;
    [SerializeField]
    GameObject alertCanvas;

    [SerializeField]
    Text alertTitle;
    [SerializeField]
    Text alertText;
    [SerializeField]
    Text yesText;
    [SerializeField]
    Text noText;

    SaveDataTime[] saveDataTimeArray;
    UILanguagePack languagePack;

    int nowAlertSaveIndex = -1;
    bool isSavingMode = false;
    bool isForceSave = false;
  
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.singleton;
        tabletManager = TabletManager.inst;
        languagePack = gameManager.languagePack;
        saveDataTimeArray = gameManager.saveDataTimeWrapper.saveDataTimeList;
        alertTitle.text = languagePack.alertTitle;
        alertText.text = languagePack.alertText;
        yesText.text = languagePack.yes;
        noText.text = languagePack.no;
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
            builder.Append(SceneManager.inst.sceneWrapper.sceneArray[saveDataTimeArray[index].sceneIndex].saveTimeString);
        }
        buttonTextArray[index].text = builder.ToString();

    }

    public void SaveCanvasActive(bool active)
    {

        saveButton.gameObject.SetActive(false);
        loadButton.SetActive(true);
        saveLoadCanvas.SetActive(active);
    }

    public void OnOpenedForceSave()
    {
        isForceSave = true;
        //saveButton.enabled = true;
        saveButton.gameObject.SetActive(true);
        loadButton.SetActive(false);
        for (int i = 0; i < 4; i++)
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

    public void WholeButtonOff()
    {
        saveButtonCanvas.SetActive(false);
        alertCanvas.SetActive(false);
        saveLoadCanvas.SetActive(false);
    }

    public void DontSaveGoAheadButton()
    {
        tabletManager.OnTabletButton(false);
        SaveOrLoadButtonCanvasGetout();
        SaveCanvasActive(false);
        gameManager.LoadJson(0);
        tabletManager.SetOnSaveDataLoad();
    }

    public void SaveOrLoadButtonDown(int index)
    {

        if (isSavingMode)
        {
            if(saveDataTimeArray[index].day == -1)
            {
                gameManager.SaveJson(index);
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
                    SceneManager.inst.LoadNextScene();
                    //gameManager.sceneManager.LoadScene(gameManager.saveData.nextLoadSceneName);

                }
            }
            else
            {
                nowAlertSaveIndex = index;
                alertCanvas.SetActive(true);
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

    public void AlertYesButton()
    {
        alertCanvas.SetActive(false);
        gameManager.SaveJson(nowAlertSaveIndex);

        for (int i = 0; i < getOutButtons.Length; i++)
        {
            getOutButtons[i].SetActive(true);
        }
        isForceSave = false;
        tabletManager.OnTabletButton(false);
        SaveOrLoadButtonCanvasGetout();
        SaveCanvasActive(false);

        SceneManager.inst.LoadNextScene();

    }

    public void AlertNoButton()
    {
        alertCanvas.SetActive(false);
    }
}
