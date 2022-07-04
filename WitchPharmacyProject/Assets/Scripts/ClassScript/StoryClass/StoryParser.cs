using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;
using Obsolete;


public class StoryParser
{


    CharacterIndexToName characterIndexToName;
    UILanguagePack languagePack;
    public StoryParser(CharacterIndexToName indexToName, UILanguagePack language)
    {
        characterIndexToName = indexToName;
        languagePack = language;
    }

    public StoryDialogBundle LoadStoryBundle(string bundleName, string languageDirectory, bool isRegion = false, RegionName regionName = RegionName.BackStreet, StoryRegion storyRegion = StoryRegion.Narin)
    {
        StoryDialogBundle gameData;

        string appender1 = bundleName;
        StringBuilder builder = new StringBuilder();
        if (isRegion)
        {
            builder.Append("RegionStoryBundle/");
            builder.Append(storyRegion.ToString());
            builder.Append("/");

            builder.Append(regionName.ToString());
            builder.Append("/");

        }
        else
        {
            builder.Append("StoryBundle/");
        }

        builder.Append(appender1);
        JsonManager json = new JsonManager();
        gameData = json.ResourceDataLoad<StoryDialogBundle>(builder.ToString());
        return gameData;
    }


    public ConversationDialogBundle LoadBundle(string bundleName, string languageDirectory, bool isRegion = false, RegionName regionName = RegionName.BackStreet, StoryRegion storyRegion = StoryRegion.Narin)
    {
        return null;
    }
    public VisitorDialogBundle LoadBundle(string bundleName, string languageDirectory, VisitorType visitorType, int symptomNumber = 0, StoryRegion storyRegion = StoryRegion.NotAllocated)
    {
        string originText;

        VisitorDialogBundle gameData = new VisitorDialogBundle();

        string language = languageDirectory;
        string directory = "JsonData/";

        string appender1 = bundleName;
        StringBuilder builder = new StringBuilder(directory);
        builder.Append(language);
        builder.Append("VisitorStoryBundle/");

        gameData.visitorType = visitorType;
        switch (visitorType)
        {
            case VisitorType.Random:

                builder.Append("Random/");
                builder.Append(storyRegion.ToString());
                builder.Append("/");
                builder.Append(symptomNumber.ToString());
                builder.Append("/");
                break;
            case VisitorType.Odd:
                builder.Append("Odd/");
                break;
            case VisitorType.Special:
                builder.Append("Special/");
                break;
            case VisitorType.RuelliaStart:
                builder.Append("RuelliaStart/");
                break;
            default:
                break;

        }

        builder.Append(appender1);
        Debug.Log(builder.ToString());
        TextAsset jsonString = Resources.Load<TextAsset>(builder.ToString());
        originText = jsonString.text;

        gameData.bundleName = bundleName;

        builder = new StringBuilder();
        StoryEffect nowMode = StoryEffect.Start;
        VisitorDialogWrapper nowWrapper = null;
        List<VisitorDialogWrapper> nowWrapperList = gameData.startWrapperList;
        string beforeName = null;
        string beforeFeeling = null;
        VisitorDialog nowDialog = null;
        int nowSymptomIndex = 0;
        int nowVisitorSetIndex = 0;

        for (int i = 0; i < originText.Length; i++)
        {

            switch (originText[i])
            {
                case '<':
                    if (nowMode == StoryEffect.Dialog)
                    {
                        nowDialog.dialog = builder.ToString();
                    }
                    builder.Clear();
                    nowMode = StoryEffect.ReadMode;
                    break;

                case '>':
                    if (nowMode != StoryEffect.ReadMode)
                    {
                        builder.Append(originText[i]);
                        break;
                    }
                    string modeStr = builder.ToString();
                    if (modeStr.Contains("bundleName"))
                    {
                        nowMode = StoryEffect.BundleName;
                        nowWrapperList = gameData.startWrapperList;
                        nowWrapper = new VisitorDialogWrapper();
                        nowWrapperList.Add(nowWrapper);
                    }
                    else if (modeStr.Contains("switch"))
                    {
                        nowMode = StoryEffect.Switch;
                        if (nowWrapper == null || nowWrapper.dialogList.Count != 0)
                        {
                            nowWrapper = new VisitorDialogWrapper();
                            nowWrapperList.Add(nowWrapper);
                        }
                    }
                    //else if (modeStr.Contains("route"))
                    //{
                    //    nowMode = ParseMode.Route;

                    //    nowWrapper = new VisitorDialogWrapper();
                    //    nowWrapper.characterName = beforeName;
                    //    nowWrapper.characterFeeling = beforeFeeling;
                    //    nowWrapperList = gameData.rightWrapperList;
                    //    nowWrapperList.Add(nowWrapper);
                    //    isRouting = true;
                    //    isRightRoute = true;
                    //}
                    else if (modeStr.Contains("forceEnd"))
                    {
                        nowMode = StoryEffect.End;
                        nowWrapper.forceEnd = true;
                    }
                    else if (modeStr.Contains("giveCoin"))
                    {
                        nowMode = StoryEffect.GiveCoin;

                        if (nowWrapper != null)
                        {
                            beforeName = nowWrapper.characterName;
                            beforeFeeling = nowWrapper.characterFeeling;
                            nowWrapper = new VisitorDialogWrapper();
                            nowWrapper.characterName = beforeName;
                            nowWrapper.characterFeeling = beforeFeeling;
                        }
                        else if (nowWrapper == null || nowWrapper.dialogList.Count != 0)
                        {
                            nowWrapper = new VisitorDialogWrapper();
                        }
                        nowWrapper.giveCoin = true;
                        nowWrapperList.Add(nowWrapper);

                    }
                    else if (modeStr.Contains("right"))
                    {
                        nowMode = StoryEffect.Start;
                        beforeName = nowWrapper.characterName;
                        beforeFeeling = nowWrapper.characterFeeling;
                        nowWrapper = new VisitorDialogWrapper();
                        nowWrapper.characterName = beforeName;
                        nowWrapper.characterFeeling = beforeFeeling;
                        nowWrapperList = gameData.rightWrapperList;
                        nowWrapperList.Add(nowWrapper);
                    }
                    else if (modeStr.Contains("wrong"))
                    {
                        nowMode = StoryEffect.Start;
                        nowWrapper = new VisitorDialogWrapper();
                        nowWrapper.characterName = beforeName;
                        nowWrapper.characterFeeling = beforeFeeling;
                        nowWrapperList = gameData.wrongWrapperList;
                        nowWrapperList.Add(nowWrapper);
                    }
                    else if (modeStr.Contains("skip"))
                    {
                        nowMode = StoryEffect.Start;
                        nowWrapper = new VisitorDialogWrapper();
                        nowWrapper.characterName = beforeName;
                        nowWrapper.characterFeeling = beforeFeeling;
                        nowWrapperList = gameData.skipWrapperList;
                        nowWrapperList.Add(nowWrapper);
                    }
                    else if (modeStr.Contains("symptom"))
                    {
                        nowMode = StoryEffect.Symptom;
                    }
                    else if (modeStr.Contains("disease"))
                    {
                        nowMode = StoryEffect.Disease;
                    }
                    else if (modeStr.Contains("visitorSet"))
                    {
                        nowMode = StoryEffect.VisitorSet;
                    }
                    else if (modeStr.Contains("region"))
                    {
                        nowMode = StoryEffect.NextRegion;
                    }
                    builder.Clear();
                    break;

                case '{':

                    switch (nowMode)
                    {
                        case StoryEffect.DialogCharacterName:
                            nowMode = StoryEffect.DialogEffect;
                            break;
                        case StoryEffect.BundleName:
                            break;
                        case StoryEffect.Switch:

                            nowMode = StoryEffect.ClampCharacterName;
                            break;
                        case StoryEffect.ClampFeeling:
                            nowMode = StoryEffect.ClampEffect;
                            break;
                        case StoryEffect.ClampCharacterName:
                            string name = builder.ToString();
                            if (name.Length < 1)
                            {
                                nowWrapper.characterName = null;
                            }
                            else
                            {
                                nowWrapper.characterName = name;
                            }

                            builder.Clear();
                            nowMode = StoryEffect.ClampEffect;
                            break;
                        case StoryEffect.GiveCoin:
                            nowMode = StoryEffect.CoinNumber;
                            break;
                        case StoryEffect.Symptom:
                            nowMode = StoryEffect.SymptomNumber;
                            break;
                        case StoryEffect.Disease:
                            nowMode = StoryEffect.DiseaseName;
                            break;
                        case StoryEffect.VisitorSet:
                            nowMode = StoryEffect.VisitorSetNumber;
                            break;
                        case StoryEffect.NextRegion:
                            nowMode = StoryEffect.NextRegionName;
                            break;
                        default:
                            builder.Append('{');
                            break;

                    }
                    break;
                case '}':
                    switch (nowMode)
                    {
                        case StoryEffect.BundleName:
                            gameData.bundleName = builder.ToString();
                            nowMode = StoryEffect.Start;
                            break;
                        case StoryEffect.Switch:
                            nowMode = StoryEffect.DialogCharacterName;
                            break;
                        case StoryEffect.ClampCharacterName:
                            string name = builder.ToString();
                            if (name.Length < 1)
                            {
                                nowWrapper.characterName = null;
                            }
                            else
                            {
                                nowWrapper.characterName = name;

                            }
                            nowMode = StoryEffect.ClampFeeling;
                            builder.Clear();
                            break;
                        case StoryEffect.ClampFeeling:
                            nowMode = StoryEffect.DialogCharacterName;
                            break;
                        case StoryEffect.ClampEffect:
                            string effectStr = builder.ToString();
                            if (effectStr.Contains("up") || effectStr.Contains("Up"))
                            {
                                nowWrapper.dialogFX = DialogFX.Up;
                            }
                            else if (effectStr.Contains("down") || effectStr.Contains("Down"))
                            {
                                nowWrapper.dialogFX = DialogFX.Down;
                            }
                            //else if (effectStr.Contains("blur") || effectStr.Contains("Blur"))
                            //{
                            //    nowWrapper.dialogFX = DialogFX.Blur;
                            //}
                            nowMode = StoryEffect.Switch;
                            //nowMode = ParseMode.DialogCharacterName;
                            break;
                        case StoryEffect.CoinNumber:
                            nowMode = StoryEffect.Start;
                            string num = builder.ToString();
                            nowWrapper.coin = int.Parse(num);
                            break;
                        case StoryEffect.SymptomNumber:
                            nowMode = StoryEffect.Start;
                            if (nowSymptomIndex >= gameData.symptomNumberArray.Length)
                            {
                                break;
                            }
                            string strNum = builder.ToString();
                            int intNum;
                            if (strNum.Length > 0)
                            {
                                intNum = int.Parse(strNum);
                            }
                            else
                            {
                                intNum = 0;
                            }
                            gameData.symptomNumberArray[nowSymptomIndex] = intNum;
                            nowSymptomIndex++;
                            break;
                        case StoryEffect.DiseaseName:
                            nowMode = StoryEffect.Start;
                            string disease = builder.ToString();
                            gameData.diseaseNameList.Add(disease);
                            break;
                        case StoryEffect.VisitorSetNumber:
                            nowMode = StoryEffect.Start;
                            gameData.oddVisitorSetArray[nowVisitorSetIndex] = int.Parse(builder.ToString());
                            nowVisitorSetIndex = 0;
                            break;
                        case StoryEffect.NextRegionName:
                            nowMode = StoryEffect.Start;
                            gameData.storyRegion = (StoryRegion)Enum.Parse(typeof(StoryRegion), builder.ToString());
                            break;
                        default:
                            builder.Append('{');
                            break;
                    }
                    builder.Clear();
                    break;
                case ',':
                    switch (nowMode)
                    {
                        case StoryEffect.SymptomNumber:
                            string strNum = builder.ToString();
                            if (nowSymptomIndex >= gameData.symptomNumberArray.Length)
                            {
                                break;
                            }
                            int intNum;
                            if (strNum.Length > 0)
                            {
                                intNum = int.Parse(strNum);
                            }
                            else
                            {
                                intNum = 0;
                            }
                            gameData.symptomNumberArray[nowSymptomIndex] = intNum;
                            nowSymptomIndex++;
                            builder.Clear();
                            break;
                        case StoryEffect.DiseaseName:
                            string disease = builder.ToString();
                            gameData.diseaseNameList.Add(disease);
                            builder.Clear();
                            break;
                        case StoryEffect.VisitorSetNumber:
                            gameData.oddVisitorSetArray[nowVisitorSetIndex] = int.Parse(builder.ToString());
                            nowVisitorSetIndex++;
                            builder.Clear();
                            break;
                        default:
                            builder.Append(',');
                            break;
                    }
                    break;
                case '(':
                    switch (nowMode)
                    {
                        case StoryEffect.Switch:
                            nowMode = StoryEffect.ClampFeeling;
                            break;
                        case StoryEffect.ClampCharacterName:
                            string name = builder.ToString();
                            if (name.Length < 1)
                            {
                                nowWrapper.characterName = null;

                            }
                            else
                            {
                                nowWrapper.characterName = name;
                            }
                            nowMode = StoryEffect.ClampFeeling;
                            builder.Clear();
                            break;
                        case StoryEffect.ClampEffect:
                            nowMode = StoryEffect.ClampFeeling;
                            break;
                        case StoryEffect.ClampFeeling:
                            break;

                        case StoryEffect.Dialog:
                            builder.Append('(');
                            break;
                    }

                    break;
                case ')':
                    switch (nowMode)
                    {
                        case StoryEffect.ClampFeeling:
                            string feeling = builder.ToString();
                            if (feeling.Length > 1)
                            {
                                nowWrapper.characterFeeling = feeling;
                            }

                            nowMode = StoryEffect.ClampFeeling;
                            builder.Clear();
                            break;
                        case StoryEffect.Dialog:
                            builder.Append(')');
                            break;
                    }



                    break;
                case '[':
                    if (nowMode == StoryEffect.Dialog)
                    {
                        nowDialog.dialog = builder.ToString();
                    }
                    builder.Clear();
                    nowMode = StoryEffect.DialogCharacterName;
                    VisitorDialog dialog = new VisitorDialog();
                    nowDialog = dialog;
                    nowWrapper.dialogList.Add(dialog);
                    break;
                case ']':
                    if (builder.ToString().Contains("Ruellia"))
                        nowDialog.ruelliaTalking = true;
                    else
                        nowDialog.ruelliaTalking = false;
                    nowMode = StoryEffect.Dialog;
                    builder.Clear();
                    break;
                case '\n':
                    break;
                default:
                    builder.Append(originText[i]);

                    break;


            }
            if (i == originText.Length - 1)
            {

                nowDialog.dialog = builder.ToString();
            }


        }

        return gameData;
        //이 정보를 게임매니저나, 로딩으로 넘겨주는 것이당
    }

}

