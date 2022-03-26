﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OneScene
{
    public int day;
    public string sceneName;
    public string sceneParameter;
    public string saveTimeString;


    
    public OneScene()
    {
        day = -1;
        sceneName = null;
        sceneParameter = null;
        saveTimeString = null;
    }
}
