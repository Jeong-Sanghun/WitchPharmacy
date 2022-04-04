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
        Screen.SetResolution(2560, 1440, true);
        jsonManager = new JsonManager();
        sceneManager = SceneManager.inst;
        //nowSaveIndex = -1;
        saveDataTimeWrapper = jsonManager.LoadSaveDataTime();
        saveDataTimeArray = saveDataTimeWrapper.saveDataTimeList;
        languagePack = jsonManager.ResourceDataLoadBeforeGame<UILanguagePack>("LanguagePack", saveDataTimeWrapper.nowLanguageDirectory);
        sceneManager.SetSceneWrapper(saveDataTimeWrapper.nowLanguageDirectory);
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
            if(saveDataTimeArray[index].sceneIndex-1 >=0)
            {
                builder.Append(SceneManager.inst.sceneWrapper.sceneArray[saveDataTimeArray[index].sceneIndex-1].saveTimeString);
            }
            
            
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

    //public void BossButton()
    //{
    //    sceneManager.nowSaveIndex = -1;
    //    sceneManager.BossSceneLoad();
    //}


}
