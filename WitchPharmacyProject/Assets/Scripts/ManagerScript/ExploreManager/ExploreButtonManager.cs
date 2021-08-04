using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploreButtonManager : MonoBehaviour
{
    SceneManager sceneManager;
    [SerializeField]
    ExploreManager exploreManager;
    
    // Start is called before the first frame update
    void Start()
    {
        sceneManager = SceneManager.inst;
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
