using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour
{
    GameManager gameManager;
    SceneManager sceneManager;
    [SerializeField]
    GameObject medicineTab;
    [SerializeField]
    GameObject toolTab;

    [SerializeField]
    Text coinText;

    private void Start()
    {
        sceneManager = SceneManager.inst;
        gameManager = GameManager.singleTon;
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
    public void MedicineTabButton()
    {
        medicineTab.SetActive(true);
        toolTab.SetActive(false);
    }
    public void ToolTabButton()
    {
        medicineTab.SetActive(false);
        toolTab.SetActive(true);
    }
}
