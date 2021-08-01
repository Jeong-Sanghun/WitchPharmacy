using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResearchButtonClass
{
    public GameObject menuButtonObj;
    public Button menuButtonComponent;
    public Button researchButtonComponent;
    public Text researchButtonText;
    public ResearchData data;
    public GameObject canvas;
    public Text researchProgressText;
    public Image filledImage;
    public bool locked;

    public ResearchButtonClass()
    {
        locked = true;
    }
}
