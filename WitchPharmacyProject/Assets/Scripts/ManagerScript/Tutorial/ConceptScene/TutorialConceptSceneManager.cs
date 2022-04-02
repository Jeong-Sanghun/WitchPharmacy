using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialConceptSceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnButton()
    {
        SceneManager.inst.LoadNextScene();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
