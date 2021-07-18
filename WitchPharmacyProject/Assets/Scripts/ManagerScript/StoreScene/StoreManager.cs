﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
    GameManager gameManager;
    SceneManager sceneManager;
    [SerializeField]
    GameObject medicineTab;
    [SerializeField]
    GameObject toolTab;

    private void Start()
    {
        sceneManager = SceneManager.inst;
        gameManager = GameManager.singleTon;
    }
    public void ToNextSceneButton()
    {
        gameManager.saveData.nowTime = 0;
        gameManager.TimeChange(7200);
        //gameManager.ForceSaveButtonActive("RoomCounterScene");
        sceneManager.LoadScene("RoomCounterScene");
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
