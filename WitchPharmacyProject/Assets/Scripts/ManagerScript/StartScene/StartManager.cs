using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartManager : MonoBehaviour
{

    SceneManager sceneManager;
    GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        sceneManager = SceneManager.inst;
    }

    public void LoadSceneButton(string name)
    {
        sceneManager.LoadScene(name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
