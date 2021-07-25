using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploreButtonManager : MonoBehaviour
{
    SceneManager sceneManager;

    
    // Start is called before the first frame update
    void Start()
    {
        sceneManager = SceneManager.inst;
    }

    public void ResearchSceneButton()
    {
        sceneManager.LoadScene("ResearchScene");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
