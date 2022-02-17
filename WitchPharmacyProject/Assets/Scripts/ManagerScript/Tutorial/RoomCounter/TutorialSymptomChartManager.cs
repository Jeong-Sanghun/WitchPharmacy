using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialSymptomChartManager : MonoBehaviour
{
    [SerializeField]
    TutorialRoomCounterManager tutorialManager;
    [SerializeField]
    GameObject symptomChartObject;
    [SerializeField]
    Toggle waterToggle;

    public void SymptomChartButton(bool turnOn)
    {
        if (turnOn)
        {
            Debug.Log("눌림");
            if (tutorialManager.isGlowing[(int)ActionKeyword.CounterSymptomChartGlow] == false)
            {
                return;
            }
            symptomChartObject.SetActive(true);
            
        }
        else
        {
            if (tutorialManager.isGlowing[(int)ActionKeyword.CounterChartExitButtonGlow] == false)
            {
                return;
            }
            symptomChartObject.SetActive(false);
        }
    }

    public void WaterPlusToggle()
    {
        if (tutorialManager.isGlowing[(int)ActionKeyword.WaterPlusGlow] == false)
        {
            waterToggle.isOn = false;
        }
    }

}
