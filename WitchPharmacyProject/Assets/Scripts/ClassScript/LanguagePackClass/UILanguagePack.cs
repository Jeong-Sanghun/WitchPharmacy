using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UILanguagePack
{
    public string billTitle;
    public string billWholeGain;
    public string billWholeSpend;
    public string billGain;
    public string billSpend;
    public string billDayth;
    public string[] reasonArray;
    //billReason enum으로 값을 받아온다.
    //GameObject symptomChartButtonPrefab;
    //GameObject symptomChartPrefab;

    public UILanguagePack()
    {
        billWholeGain = "총 수입";
        billWholeSpend = "총 판매";
        billGain = "수입";
        billSpend = "지출";
        reasonArray = new string[3];

        reasonArray[0] = "약재 판매";
        reasonArray[1] = "약재 구입";
        reasonArray[2] = "유지비";

    }

}
