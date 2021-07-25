using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploreManager : MonoBehaviour
{
    public static ExploreManager inst;
    TabletManager tabletManager;
    GameManager gameManager;
    SaveDataClass saveData;
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
