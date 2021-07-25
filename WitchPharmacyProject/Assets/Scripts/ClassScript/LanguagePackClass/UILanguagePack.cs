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
    public string documentGainedRegion;
    public string boxCoinGained;
    public string boxGained;
    public string autoSave;
    public string slot;
    public string slotDay;
    public string slotEmpty;
    public string explain;
    public string neededResearch;
    public string[] saveTimeArray;
    public string[] reasonArray;
    public string[] characterNameArray;

    //billReason enum으로 값을 받아온다.
    //GameObject symptomChartButtonPrefab;
    //GameObject symptomChartPrefab;

    public string Insert(string org,int coin)
    {
        string origin = string.Copy(org);
        for (int i = 0; i < origin.Length; i++)
        {
            if(origin[i] == '$')
            {
                origin = origin.Remove(i, 1);
                origin  = origin.Insert(i, coin.ToString());

                break;
            }
        }
        Debug.Log(origin);
        return origin;
    }

    public string Insert(string org,string insertString)
    {
        string origin = string.Copy(org);
        for (int i = 0; i < origin.Length; i++)
        {
            if (origin[i] == '$')
            {
                origin = origin.Remove(i, 1);
                origin =origin.Insert(i, insertString);

                break;
            }
        }
        Debug.Log(origin);
        return origin;
    }

    //받침있는게 first, 받침없는게 second.
    public string GetCompleteWord(string name, string firstVal, string secondVal)
    {
        //char lastName = name.ElementAt(name.Length - 1);
        char lastName = name[name.Length - 1];
        int index = (lastName - 0xAC00) % 28;
        //한글의 제일 처음과 끝의 범위 밖일경우 에러
        if (lastName < 0xAC00 || lastName > 0xD7A3)
        {
            return secondVal;
        }
        string selectVal = (lastName - 0xAC00) % 28 > 0 ? firstVal : secondVal;
        return selectVal;

    }


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
