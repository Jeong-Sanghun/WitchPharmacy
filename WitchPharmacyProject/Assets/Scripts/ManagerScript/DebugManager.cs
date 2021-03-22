using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugManager : MonoBehaviour
{   

    public GameManager GameManagerScirpt;
    public GameObject DebugText;
    public string debugString;
    public GameObject debug_GameObject;

    public void DebugFunction(){
        StartCoroutine(GameManagerScirpt.SceneManagerScirpt.LoadTextOneByOne(debugString,DebugText.GetComponent<Text>()));
        StartCoroutine(GameManagerScirpt.SceneManagerScirpt.MoveModule_Accel(debug_GameObject,new Vector3(7f,-1.74f,-1f),2f));
    }

    // Start is called before the first frame update
    void Start()
    {
        //상훈이의 주석처리
       // DebugFunction();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
