using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour
{
    GameManager gameManager;
    SceneManager sceneManager;
    [SerializeField]
    GameObject otherToolTab;
    [SerializeField]
    GameObject measureToolTab;

    [SerializeField]
    Text coinText;

    private void Start()
    {
        sceneManager = SceneManager.inst;
        gameManager = GameManager.singleTon;
        otherToolTab.SetActive(false);
        measureToolTab.SetActive(true);
    }
    public void ToNextSceneButton()
    {
        //gameManager.saveData.nowTime = 0;
        //gameManager.TimeChange(7200);
        //gameManager.ForceSaveButtonActive("RoomCounterScene");
        sceneManager.LoadScene("ExploreScene");
    }


    public void ChangeCoinText()
    {
        coinText.text = gameManager.saveData.coin.ToString();
    }
    public void OtherToolTabButton()
    {
        otherToolTab.SetActive(true);
        measureToolTab.SetActive(false);
    }
    public void ToolTabButton()
    {
        otherToolTab.SetActive(false);
        measureToolTab.SetActive(true);
    }

    
}
