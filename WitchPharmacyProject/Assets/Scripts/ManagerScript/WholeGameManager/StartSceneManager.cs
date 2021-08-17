using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class StartSceneManager : MonoBehaviour
{
    SceneManager sceneManager;
    JsonManager jsonManager;
    //int nowSaveIndex;
    SaveDataTimeWrapper saveDataTimeWrapper;
    SaveDataTime[] saveDataTimeArray;
    UILanguagePack languagePack;
    [SerializeField]
    Text[] buttonTextArray;

    [SerializeField]
    GameObject loadButtonCanvas;
    

    // Start is called before the first frame update
    void Start()
    {
        jsonManager = new JsonManager();
        sceneManager = SceneManager.inst;
        //nowSaveIndex = -1;
        saveDataTimeWrapper = jsonManager.LoadSaveDataTime();
        saveDataTimeArray = saveDataTimeWrapper.saveDataTimeList;
        languagePack = jsonManager.ResourceDataLoadBeforeGame<UILanguagePack>("LanguagePack", saveDataTimeWrapper.nowLanguageDirectory);
        for(int i = 0; i < saveDataTimeArray.Length; i++)
        {
            SetButtonText(i);
        }
    }
    void SetButtonText(int index)
    {
        StringBuilder builder;
        if (index == 0)
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
            builder.Append((saveDataTimeArray[index].day + 1).ToString());
            builder.Append(languagePack.slotDay);
            builder.Append(" ");
            builder.Append(languagePack.saveTimeArray[(int)saveDataTimeArray[index].saveTime]);
        }
        buttonTextArray[index].text = builder.ToString();
    }

    public void OpenLoadButton(bool active)
    {
        loadButtonCanvas.SetActive(active);
    }

    public void LoadButton(int index)
    {
        sceneManager.nowSaveIndex = index;
        sceneManager.VeryFirstStartLoad();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
