using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResearchData
{
    public string fileName;
    public string ingameName;

    public string explain;

    public int researchEndTime;

    public List<string> neededResearchList;

    protected Sprite image;

    public virtual Sprite LoadImage()
    {
        return null;
    }

    public ResearchData()
    {
        fileName = "water";
        ingameName = "측정도구(물)";
        explain = "도구설명";
        researchEndTime = 10;
        neededResearchList = new List<string>();


    }


}
