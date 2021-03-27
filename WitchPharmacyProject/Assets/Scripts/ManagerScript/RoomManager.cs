using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public CounterManager counterManager;
    GameManager gameManager;
    SaveDataClass saveData;
    MedicineDictionary medicineDictionary;

    [SerializeField]
    GameObject medicineScrollPrefab;
    [SerializeField]
    GameObject medicineButtonPrefab;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.singleTon;
        saveData = gameManager.saveData;
        medicineDictionary = gameManager.medicineDictionary;


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
