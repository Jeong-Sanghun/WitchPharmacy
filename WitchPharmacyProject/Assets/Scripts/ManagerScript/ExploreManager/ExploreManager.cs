using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RegionName
{
    Library, Forest, Market, AntiqueShop, Research, Store
}

public class ExploreManager : MonoBehaviour
{
    public static ExploreManager inst;
    TabletManager tabletManager;
    GameManager gameManager;
    SaveDataClass saveData;

    public RegionName nowRegion;
    int timeCount = 1;
    // Start is called before the first frame update
    private void Awake()
    {
        if (inst == null)
        {
            inst = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        gameManager = GameManager.singleTon;
        tabletManager = TabletManager.inst;
        saveData = gameManager.saveData;
        for(int  i = 0; i < saveData.researchSaveData.endOtherToolResearchList.Count; i++)
        {
            if (saveData.researchSaveData.endOtherToolResearchList[i].Contains("energyDrink"))
            {
                timeCount = 2;
            }
        }
        
    }

    public void TimeChange(float time)
    {
        gameManager.TimeChange(time);
        if(saveData.nowTime > 3600 * 15)
        {
            gameManager.NextDay();
            SceneManager.inst.LoadScene("StoryScene");
            Destroy(gameObject);
        }
    }

    public void NextTime()
    {
        timeCount--;
        if (timeCount == 0)
        {
            gameManager.NextDay();
            SceneManager.inst.LoadScene("StoryScene");
            Destroy(gameObject);
        }
        else
        {
            SceneManager.inst.LoadScene("ExploreScene");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
