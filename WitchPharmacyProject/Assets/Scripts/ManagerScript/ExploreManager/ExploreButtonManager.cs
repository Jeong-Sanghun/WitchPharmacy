using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploreButtonManager : MonoBehaviour
{
    SceneManager sceneManager;
    [SerializeField]
    ExploreManager exploreManager;
    [SerializeField]
    GameObject[] regionButtonArray;



    // Start is called before the first frame update
    void Start()
    {
        sceneManager = SceneManager.inst;
        exploreManager = ExploreManager.inst;
        UnlockButton();
    }

    void UnlockButton()
    {
        RegionDataWrapper wrapper = exploreManager.regionDataWrapper;

        for(int i = 0; i < wrapper.regionDataList.Count; i++)
        {
            Debug.Log(wrapper.regionDataList[i].fileName);
            if(wrapper.regionDataList[i].unlockDay <= GameManager.singleTon.saveData.nowDay)
            {
                regionButtonArray[i].SetActive(true);
            }
            else
            {
                regionButtonArray[i].SetActive(false);
            }
        }
    }

    public void ResearchSceneButton()
    {
        sceneManager.LoadScene("ResearchScene");
    }

    public void StoreSceneButton()
    {
        sceneManager.LoadScene("StoreScene");
    }

    public void RegionSceneButton(int index)
    {
        exploreManager.nowRegion = (RegionName)index;
        sceneManager.LoadScene("RegionScene");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
