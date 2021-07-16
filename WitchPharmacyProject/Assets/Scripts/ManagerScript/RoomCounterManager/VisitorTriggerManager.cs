using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisitorTriggerManager : MonoBehaviour
{
    [SerializeField]
    CounterManager counterManager;
    [SerializeField]
    CounterDialogManager counterDialogManager;
    GameManager gameManager;
    SaveDataClass saveData;
    // Start is called before the first frame update
    bool nowCheckingTrigger;


    
    void Start()
    {
        nowCheckingTrigger = false;
        gameManager = GameManager.singleTon;
        saveData = gameManager.saveData;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (nowCheckingTrigger)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                nowCheckingTrigger = false;
                counterManager.CounterStart();
            }
            else if (Input.GetKeyDown(KeyCode.B) || Input.GetMouseButtonDown(0))
            {
                nowCheckingTrigger = false;
                counterManager.CounterStart(counterDialogManager.specialVisitorDialogBundle.characterName);
            }
           
        }
    }

    public void TriggerCheck()
    {
        if (counterDialogManager.ConditionCheck())
        {
            nowCheckingTrigger = true;
        }
        else
        {
            nowCheckingTrigger = false;
            counterManager.CounterStart();
        }

    }
}
