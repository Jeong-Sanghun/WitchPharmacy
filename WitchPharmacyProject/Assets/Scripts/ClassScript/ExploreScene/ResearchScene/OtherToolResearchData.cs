using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OtherToolResearchData :ResearchData
{
    //히히 빈 클래스 ㅋㅋ

    public OtherToolResearchData()
    {
        fileName = "improvement";
        ingameName = "연구 설비 개선 연구";
        explain = "이것만 있으면 연구를 더 빠르게 진행할 수 있어..!";
        researchEndTime = 5;
        neededResearchList = new List<string>();
    }

    public override Sprite LoadImage()
    {
        if (image != null)
        {
            return image;
        }

        image = Resources.Load<Sprite>("OtherToolImage/" + fileName);
        return image;
    }

}
