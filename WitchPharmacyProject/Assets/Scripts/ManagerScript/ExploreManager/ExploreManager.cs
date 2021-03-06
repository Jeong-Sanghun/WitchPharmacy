using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public enum RegionName
{
    BackStreet, Revav, WitchGrave, Semnos, Narin, RuinCity, Mikcha, TreeOfLife, Research, Store
}

public class ExploreManager : MonoBehaviour
{
    public static ExploreManager inst;
    public RegionDataWrapper regionDataWrapper;
    TabletManager tabletManager;
    GameManager gameManager;
    SaveDataClass saveData;
    [SerializeField]
    bool isTutorial;

    public RegionName nowRegion;
    int timeCount = 1;
    // Start is called before the first frame update
    private void Awake()
    {
        if(isTutorial == true)
        {
            return;
        }
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
        gameManager = GameManager.singleton;
        tabletManager = TabletManager.inst;
        saveData = gameManager.saveData;
        for (int i = 0; i < saveData.researchSaveData.endOtherToolResearchList.Count; i++)
        {
            if (saveData.researchSaveData.endOtherToolResearchList[i].Contains("energyDrink"))
            {
                timeCount = 2;
            }
        }
        StringBuilder path = new StringBuilder("RegionData/");
        path.Append(saveData.nowRegion.ToString());
        regionDataWrapper = gameManager.jsonManager.ResourceDataLoad<RegionDataWrapper>(path.ToString());

    }

    public void TimeChange(float time)
    {
        gameManager.TimeChange(time);
        if (saveData.nowTime > 3600 * 15)
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
            //gameManager.NextDay();
            //SceneManager.inst.LoadScene("StoryScene");
            SceneManager.inst.LoadNextScene();
            Destroy(gameObject);
        }
        else
        {
            SceneManager.inst.LoadScene("ExploreScene");
        }
    }

}
