using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploreButtonManager : MonoBehaviour
{
    SceneManager sceneManager;
    [SerializeField]
    ExploreManager exploreManager;
    [SerializeField]
    GameObject researchButton;
    [SerializeField]
    GameObject[] regionButtonArray;
    SaveDataClass saveData;

    [SerializeField]
    bool isTutorial;

    public bool isTutorialGlowing;


    // Start is called before the first frame update
    void Start()
    {
        sceneManager = SceneManager.inst;
        exploreManager = ExploreManager.inst;
        saveData = GameManager.singleton.saveData;
        isTutorialGlowing = false;
        if (isTutorial)
        {
            string unlockScene = SceneManager.inst.sceneParameter;
            RegionName unlockRegion =(RegionName)System.Enum.Parse(typeof(RegionName), unlockScene);
            int regionLength = System.Enum.GetValues(typeof(RegionName)).Length;
            for (int i = 0; i < regionLength; i++)
            {
                GameObject onObject = regionButtonArray[i].transform.GetChild(0).gameObject;
                GameObject offObject = regionButtonArray[i].transform.GetChild(1).gameObject;

                if(i == (int)unlockRegion)
                {
                    onObject.SetActive(true);
                    offObject.SetActive(false);
                }
                else
                {
                    onObject.SetActive(false);
                    offObject.SetActive(true);
                }
            }
        }
        else
        {
            UnlockButtonObsolete();
        }

    }

    void UnlockButtonObsolete()
    {
        RegionDataWrapper wrapper = exploreManager.regionDataWrapper;

        if (saveData.forcedRegion != null)
        {
            researchButton.SetActive(false);
            for (int i = 0; i < wrapper.regionDataList.Count; i++)
            {
                Debug.Log(wrapper.regionDataList[i].fileName);
                if (saveData.forcedRegion == wrapper.regionDataList[i].fileName)
                {
                    regionButtonArray[i].SetActive(true);
                }
                else
                {
                    regionButtonArray[i].SetActive(false);
                }
            }
        }
        else
        {
            for (int i = 0; i < wrapper.regionDataList.Count; i++)
            {
                Debug.Log(wrapper.regionDataList[i].fileName);
                if (wrapper.regionDataList[i].unlockDay <= GameManager.singleton.saveData.nowDay)
                {
                    regionButtonArray[i].SetActive(true);
                }
                else
                {
                    regionButtonArray[i].SetActive(false);
                }
            }
        }
    }

    void UnlockButton()
    {

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
        if (isTutorial)
        {
            if (isTutorialGlowing)
            {
                isTutorialGlowing = false;
                SceneManager.inst.LoadNextScene();
                
            }
            
        }
        else
        {

            exploreManager.nowRegion = (RegionName)index;
            sceneManager.LoadScene("RegionScene");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
