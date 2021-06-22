using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionManager : MonoBehaviour
{
    SceneManager sceneManager;
    // Start is called before the first frame update
    void Start()
    {
        sceneManager = SceneManager.inst;
    }

    public void OnBackButton()
    {
        sceneManager.LoadScene("ExploreScene");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
