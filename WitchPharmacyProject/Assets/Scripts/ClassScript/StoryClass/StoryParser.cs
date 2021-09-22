using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;
public class StoryParser
{
    enum ParseMode{
        Clamp,Switch,Route,DialogCharacterName, Dialog,DialogEffect, BundleName,
            ReadMode,ClampCharacterName, ClampFeeling,ClampEffect
            ,RouteSwitch,RouteText,RightAfterRoute,CutScene,CutSceneFileName,CutSceneEffect,Null
            ,BackGround,BackGroundName,BackGroundEffect, NextStory, NextStoryName
            ,NextRegion, NextRegionName,RightMedicine,WrongMedicine,SkipVisitor,GiveCoin,CoinNumber
            ,Symptom,SymptomNumber,Disease,DiseaseName,VisitorSet,VisitorSetNumber
            ,ForcedRegion,ForcedRegionName
    }

    CharacterIndexToName characterIndexToName;
    UILanguagePack languagePack;
    public StoryParser(CharacterIndexToName indexToName, UILanguagePack language)
    {
        characterIndexToName = indexToName;
        languagePack = language;
    }

    public ConversationDialogBundle LoadBundle(string bundleName, string languageDirectory, bool isRegion = false, RegionName regionName = RegionName.Library,StoryRegion storyRegion=StoryRegion.Narin)
    {
        string originText;

        ConversationDialogBundle gameData = new ConversationDialogBundle();

        string language = languageDirectory;
        string directory = "JsonData/";

        string appender1 = bundleName;
        StringBuilder builder = new StringBuilder(directory);
        builder.Append(language);
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

        TextAsset jsonString = Resources.Load<TextAsset>(builder.ToString());
        originText = jsonString.text;

        gameData.bundleName = bundleName;

        builder = new StringBuilder();
        ParseMode nowMode = ParseMode.Null;
        ConversationDialogWrapper nowWrapper = null;
        List<ConversationDialogWrapper> nowWrapperList = gameData.dialogWrapperList;
        ConversationDialogWrapper wrapperBeforeRoute = null;
        ConversationRouter nowRouter = null;
        ConversationDialog nowDialog = null;
        bool isRouting = false;
        int routingNumber = 0;
        int leftMiddleRight = 0;
        for (int i = 0; i < originText.Length; i++)
        {

            switch (originText[i])
            {
                case '<':
                    if(nowMode == ParseMode.Dialog)
                    {
                        nowDialog.dialog = builder.ToString();
                    }
                    builder.Clear();
                    nowMode = ParseMode.ReadMode;
                    break;

                case '>':
                    if(nowMode != ParseMode.ReadMode)
                    {
                        builder.Append(originText[i]);
                        break;
                    }
                    string modeStr = builder.ToString();
                    if (modeStr.Contains("bundleName"))
                    {
                        nowMode = ParseMode.BundleName;
                        nowWrapperList = gameData.dialogWrapperList;
                    }
                    else if (modeStr.Contains("switch"))
                    {
                        nowMode = ParseMode.Switch;
                        if(nowWrapper == null || nowWrapper.conversationDialogList.Count != 0)
                        {
                            nowWrapper = new ConversationDialogWrapper();
                            nowWrapperList.Add(nowWrapper);
                        }
                    }
                    else if (modeStr.Contains("route"))
                    {
                        if (nowWrapper != null)
                            nowWrapper.nextWrapperIsRouter = true;
                        nowMode = ParseMode.Route;
                        nowRouter = new ConversationRouter();
                        nowWrapperList = nowRouter.routingWrapperList;
                        gameData.conversationRouterList.Add(nowRouter);
                        wrapperBeforeRoute = new ConversationDialogWrapper();
                        for (int j = 0; j < 3; j++)
                        {
                            wrapperBeforeRoute.characterFeeling[j] = nowWrapper.characterFeeling[j];
                            wrapperBeforeRoute.characterName[j] = nowWrapper.characterName[j];
                            wrapperBeforeRoute.ingameName[j] = nowWrapper.ingameName[j];
                        }
                        isRouting = true;
                    }
                    else if (modeStr.Contains("cutscene"))
                    {
                        nowMode = ParseMode.CutScene;
                        ConversationDialogWrapper wrapper;

                        if (nowWrapper == null)
                        {
                            wrapper = new ConversationDialogWrapper();
                            wrapper.isCutscene = true;
                            nowWrapper = wrapper;
                            nowWrapperList.Add(wrapper);
                        }
                        else if (nowWrapper.conversationDialogList.Count == 0)
                        {
                            nowWrapper.isCutscene = true;
                        }
                        else
                        {
                            wrapper = new ConversationDialogWrapper();
                            for (int j = 0; j < 3; j++)
                            {
                                wrapper.characterFeeling[j] = nowWrapper.characterFeeling[j];
                                wrapper.characterName[j] = nowWrapper.characterName[j];
                                wrapper.ingameName[j] = nowWrapper.ingameName[j];
                            }
                            wrapper.isCutscene = true;
                            nowWrapper = wrapper;
                            nowWrapperList.Add(wrapper);
                        }
                    }
                    else if (modeStr.Contains("background"))
                    {
                        nowMode = ParseMode.BackGround;
                        ConversationDialogWrapper wrapper;

                        if (nowWrapper == null)
                        {
                            wrapper = new ConversationDialogWrapper();
                            nowWrapper = wrapper;
                            nowWrapperList.Add(wrapper);
                        }
                        else if (nowWrapper.conversationDialogList.Count == 0)
                        {
                            nowWrapper.backGroundFileName = "testBackGround1";
                            //기본 백그라운드.
                        }
                        else
                        {
                            wrapper = new ConversationDialogWrapper();
                            for (int j = 0; j < 3; j++)
                            {
                                wrapper.characterFeeling[j] = nowWrapper.characterFeeling[j];
                                wrapper.characterName[j] = nowWrapper.characterName[j];
                                wrapper.ingameName[j] = nowWrapper.ingameName[j];
                            }
                            nowWrapper = wrapper;
                            nowWrapperList.Add(wrapper);
                        }
                    }
                    else if (modeStr.Contains("nextStory"))
                    {
                        nowMode = ParseMode.NextStory;
                        ConversationDialogWrapper wrapper;

                        if (nowWrapper == null)
                        {
                            wrapper = new ConversationDialogWrapper();
                            nowWrapper = wrapper;
                            nowWrapperList.Add(wrapper);
                        }
                        else if (nowWrapper.conversationDialogList.Count == 0)
                        {
                            //의미없는 코드.
                            nowWrapper.nextStory = null;
                        }
                        else
                        {
                            wrapper = new ConversationDialogWrapper();
                            for (int j = 0; j < 3; j++)
                            {
                                wrapper.characterFeeling[j] = nowWrapper.characterFeeling[j];
                                wrapper.characterName[j] = nowWrapper.characterName[j];
                                wrapper.ingameName[j] = nowWrapper.ingameName[j];
                            }
                            nowWrapper = wrapper;
                            nowWrapperList.Add(wrapper);
                        }
                    }
                    else if (modeStr.Contains("forcedRegion"))
                    {
                        nowMode = ParseMode.ForcedRegion;
                    }
                    else if (modeStr.Contains("Region"))
                    {
                        nowMode = ParseMode.NextRegion;
                        ConversationDialogWrapper wrapper;

                        if (nowWrapper == null)
                        {
                            wrapper = new ConversationDialogWrapper();
                            nowWrapper = wrapper;
                            nowWrapperList.Add(wrapper);
                        }
                        else if (nowWrapper.conversationDialogList.Count == 0)
                        {
                            //의미없는 코드.
                            nowWrapper.nextRegion = null;
                        }
                        else
                        {
                            wrapper = new ConversationDialogWrapper();
                            for (int j = 0; j < 3; j++)
                            {
                                wrapper.characterFeeling[j] = nowWrapper.characterFeeling[j];
                                wrapper.characterName[j] = nowWrapper.characterName[j];
                                wrapper.ingameName[j] = nowWrapper.ingameName[j];
                            }
                            nowWrapper = wrapper;
                            nowWrapperList.Add(wrapper);
                        }
                    }

                    builder.Clear();
                    break;

                case '{':

                    switch (nowMode)
                    {
                        case ParseMode.DialogCharacterName:
                            nowMode = ParseMode.DialogEffect;
                            break;
                        case ParseMode.BundleName:
                            break;
                        case ParseMode.Switch:
                            //string name = builder.ToString();
                            //if (nowWrapper.characterName[leftMiddleRight] == null && name.Length>1)
                            //{
                            //    nowWrapper.characterName[leftMiddleRight] = name;
                            //    nowWrapper.ingameName[leftMiddleRight] = characterIndexToName.NameTranslator(name, languagePack);

                            //}
                            //builder.Clear();
                            nowMode = ParseMode.ClampCharacterName;
                            break;
                        case ParseMode.Route:
                            nowMode = ParseMode.RouteText;
                            break;
                        case ParseMode.ClampFeeling:
                            nowMode = ParseMode.ClampEffect;
                            break;
                        case ParseMode.ClampCharacterName:
                            string name = builder.ToString();
                            nowWrapper.characterName[leftMiddleRight] = name;
                            nowWrapper.ingameName[leftMiddleRight] = characterIndexToName.NameTranslator(name, languagePack);
                            nowMode = ParseMode.ClampFeeling;
                            builder.Clear();
                            nowMode = ParseMode.ClampEffect;
                            break;
                        case ParseMode.CutScene:
                            nowMode = ParseMode.CutSceneFileName;
                            break;
                        case ParseMode.BackGround:
                            nowMode = ParseMode.BackGroundName;
                            break;
                        case ParseMode.NextStory:
                            nowMode = ParseMode.NextStoryName;
                            break;
                        case ParseMode.NextRegion:
                            nowMode = ParseMode.NextRegionName;
                            break;
                        case ParseMode.ForcedRegion:
                            nowMode = ParseMode.ForcedRegionName;
                            break;


                        default:
                            builder.Append('{');
                            break;

                    }
                    break;
                case '}':
                    switch (nowMode)
                    {
                        case ParseMode.BundleName:
                            gameData.bundleName = builder.ToString();
                            break;
                        case ParseMode.Switch:
                            nowMode = ParseMode.DialogCharacterName;
                            leftMiddleRight = 0;
                            break;
                        case ParseMode.ClampCharacterName:
                            string name = builder.ToString();
                            if (name.Length < 1)
                            {
                                nowWrapper.characterName[leftMiddleRight] = null;
                                nowWrapper.ingameName[leftMiddleRight] = null;

                            }
                            else
                            {
                                nowWrapper.characterName[leftMiddleRight] = name;
                                nowWrapper.ingameName[leftMiddleRight] = characterIndexToName.NameTranslator(name, languagePack);

                            }
                            leftMiddleRight = 0;
                            nowMode = ParseMode.ClampFeeling;
                            builder.Clear();
                            break;
                        case ParseMode.ClampFeeling:
                            nowMode = ParseMode.DialogCharacterName;
                            leftMiddleRight = 0;
                            break;
                        case ParseMode.ClampEffect:
                            DialogEffect effect = new DialogEffect();
                            string effectStr = builder.ToString();
                            nowWrapper.startEffectList.Add(effect);
                            if(effectStr.Contains("up") || effectStr.Contains("Up"))
                            {
                                effect.effect = DialogFX.Up;
                            }
                            else if(effectStr.Contains("down") || effectStr.Contains("Down"))
                            {
                                effect.effect = DialogFX.Down;
                            }
                            else if(effectStr.Contains("blur") || effectStr.Contains("Blur"))
                            {
                                effect.effect = DialogFX.Blur;
                            }

                            effect.characterPosition = (CharacterPos)leftMiddleRight;

                            nowMode = ParseMode.Switch;
                            break;
                        case ParseMode.RouteText:
                            routingNumber++;
                            nowRouter.routeButtonText.Add(builder.ToString());
                            nowMode = ParseMode.Route;
                            break;
                        case ParseMode.CutSceneFileName:
                            nowMode = ParseMode.Null;
                            nowWrapper.cutSceneFileName = builder.ToString();
                            break;
                        case ParseMode.BackGroundName:
                            nowMode = ParseMode.Null;
                            nowWrapper.backGroundFileName = builder.ToString();
                            break;
                        //여기 파스모드 컷씬 이펙트랑 백그라운드 이펙트는 하나마나 의미없어서..
                        case ParseMode.CutScene:
                        case ParseMode.BackGroundEffect:
                            nowMode = ParseMode.Null;
                            break;
                        case ParseMode.NextStoryName:
                            nowMode = ParseMode.Null;
                            Debug.Log(builder.ToString());
                            nowWrapper.nextStory = builder.ToString();
                            break;
                        case ParseMode.NextRegionName:
                            nowMode = ParseMode.Null;
                            nowWrapper.nextRegion = builder.ToString();
                            break;
                        case ParseMode.ForcedRegionName:
                            nowMode = ParseMode.Null;
                            gameData.forcedRegion = builder.ToString();
                            break;
                    }
                    builder.Clear();
                    break;
                case '(':
                    switch (nowMode)
                    {
                        case ParseMode.Switch:
                            break;
                        case ParseMode.ClampCharacterName:
                            string name = builder.ToString();
                            Debug.Log(name);
                            if (name.Length < 1)
                            {
                                nowWrapper.characterName[leftMiddleRight] = null;
                                nowWrapper.ingameName[leftMiddleRight] = null;

                            }
                            else
                            {
                                nowWrapper.characterName[leftMiddleRight] = name;
                                nowWrapper.ingameName[leftMiddleRight] = characterIndexToName.NameTranslator(name, languagePack);

                            }
                            nowMode = ParseMode.ClampFeeling;
                            builder.Clear();
                            break;
                        case ParseMode.ClampEffect:
                        case ParseMode.ClampFeeling:
                            break;
                        case ParseMode.Route:
                            //좆됐다고 봐야함.
                            nowRouter.routingWrapperIndex.Add(nowRouter.routingWrapperList.Count);
                            ConversationDialogWrapper routeWrapper = new ConversationDialogWrapper();
                            for (int j = 0; j < 3; j++)
                            {
                                routeWrapper.characterFeeling[j] = wrapperBeforeRoute.characterFeeling[j];
                                routeWrapper.characterName[j] = wrapperBeforeRoute.characterName[j];
                                routeWrapper.ingameName[j] = wrapperBeforeRoute.ingameName[j];
                            }
                            nowWrapper = routeWrapper;
                            
                            nowRouter.routingWrapperList.Add(routeWrapper);
                            break;
                        case ParseMode.CutSceneFileName:
                            nowWrapper.cutSceneFileName = builder.ToString();
                            nowMode = ParseMode.CutSceneEffect;
                            builder.Clear();
                            break;
                        case ParseMode.BackGroundName:
                            nowWrapper.backGroundFileName = builder.ToString();
                            nowMode = ParseMode.BackGroundEffect;
                            builder.Clear();
                            break;
                        case ParseMode.Dialog:
                            builder.Append('(');
                            break;
                    }

                    break;
                case ')':
                    switch (nowMode)
                    {
                        case ParseMode.ClampFeeling:
                            string feeling = builder.ToString();
                            nowWrapper.characterFeeling[leftMiddleRight]= feeling;
                            //nowMode = ParseMode.Switch;
                            builder.Clear();
                            break;
                        case ParseMode.Route:
                            builder.Clear();
                            break;
                        case ParseMode.CutSceneEffect:
                            string effect = builder.ToString();
                            if (effect.Contains("blur") || effect.Contains("Blur"))
                            {
                                nowWrapper.cutSceneEffect = CutSceneEffect.Blur;
                            }
                            builder.Clear();
                            break;
                        case ParseMode.BackGroundEffect:
                            effect = builder.ToString();
                            if (effect.Contains("blur") || effect.Contains("Blur"))
                            {
                                nowWrapper.backGroundEffect = CutSceneEffect.Blur;
                            }
                            builder.Clear();
                            break;
                        case ParseMode.Dialog:
                            if (isRouting)
                            {
                                nowDialog.dialog = builder.ToString();
                                routingNumber--;
                                if (routingNumber > 0)
                                {
                                    nowMode = ParseMode.Route;
                                    
                                    //ConversationDialogWrapper wrapper = new ConversationDialogWrapper();
                                    //nowRouter.routingWrapperList.Add(wrapper);
                                    //for (int j = 0; j < 3; j++)
                                    //{
                                    //    wrapper.characterFeeling[j] = wrapperBeforeRoute.characterFeeling[j];
                                    //    wrapper.characterName[j] = wrapperBeforeRoute.characterName[j];
                                    //    wrapper.ingameName[j] = wrapperBeforeRoute.ingameName[j];
                                    //}
                                    //nowWrapper = wrapper;
                                }
                                else
                                {
                                    nowMode = ParseMode.RightAfterRoute;
                                    Debug.Log("여기실행되니");
                                    nowWrapperList = gameData.dialogWrapperList;
                                    ConversationDialogWrapper wrapper = new ConversationDialogWrapper();
                                    for(int j = 0; j < 3; j++)
                                    {
                                        wrapper.characterFeeling[j] = wrapperBeforeRoute.characterFeeling[j];
                                        wrapper.characterName[j] = wrapperBeforeRoute.characterName[j];
                                        wrapper.ingameName[j] = wrapperBeforeRoute.ingameName[j];
                                    }
                                    nowWrapper = wrapper;
                                    nowWrapperList.Add(wrapper);
                                }
                                builder.Clear();
                            }
                            else
                            {
                                builder.Append(')');
                            }
                            
                            break;
                    }


                    break;
                case '[':
                    if(nowMode == ParseMode.Dialog)
                    {
                        nowDialog.dialog = builder.ToString();
                    }
                    builder.Clear();
                    nowMode = ParseMode.DialogCharacterName;
                    ConversationDialog dialog = new ConversationDialog();
                    nowDialog = dialog;
                    nowWrapper.conversationDialogList.Add(dialog);
                    break;
                case ']':
                    nowDialog.ingameName = builder.ToString();
                    for(int j = 0; j < 4; j++)
                    {
                        if(nowDialog.ingameName.Length > 1)
                        {
                            if(nowWrapper.ingameName[j] == null)
                            {
                                nowDialog.fade[j] = false;
                            }
                            else
                            {
                                if (nowWrapper.ingameName[j].Contains(nowDialog.ingameName))
                                {
                                    nowDialog.fade[j] = true;
                                }
                                else
                                {
                                    nowDialog.fade[j] = false;
                                }
                            }

                        }
                        else
                        {
                            nowDialog.fade[j] = false;
                        }

                    }
                    nowMode = ParseMode.Dialog;
                    builder.Clear();
                    break;
                case ',':
                    switch (nowMode)
                    {
                        case ParseMode.ClampCharacterName:
                            string name = builder.ToString();
                            if (name.Length < 1)
                            {
                                nowWrapper.characterName[leftMiddleRight] = null;
                                nowWrapper.ingameName[leftMiddleRight] = null;

                            }
                            else
                            {
                                nowWrapper.characterName[leftMiddleRight] = name;
                                nowWrapper.ingameName[leftMiddleRight] = characterIndexToName.NameTranslator(name, languagePack);

                            }
                            leftMiddleRight++;
                            nowMode = ParseMode.ClampCharacterName;
                            builder.Clear();
                            break;
                        case ParseMode.Switch:
                        case ParseMode.ClampFeeling:
                        case ParseMode.ClampEffect:
                            leftMiddleRight++;
                            nowMode = ParseMode.ClampCharacterName;
                            break;
                        default:
                            builder.Append(originText[i]);
                            break;
                    }
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

    public VisitorDialogBundle LoadBundle(string bundleName, string languageDirectory,VisitorType visitorType,int symptomNumber = 0)
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
                gameData.visitorType = VisitorType.Special;
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
        ParseMode nowMode = ParseMode.Null;
        VisitorDialogWrapper nowWrapper = null;
        List<VisitorDialogWrapper> nowWrapperList = gameData.startWrapperList;
        string beforeName=null;
        string beforeFeeling=null;
        VisitorDialog nowDialog = null;
        int nowSymptomIndex = 0;
        int nowVisitorSetIndex = 0;

        for (int i = 0; i < originText.Length; i++)
        {

            switch (originText[i])
            {
                case '<':
                    if (nowMode == ParseMode.Dialog)
                    {
                        nowDialog.dialog = builder.ToString();
                    }
                    builder.Clear();
                    nowMode = ParseMode.ReadMode;
                    break;

                case '>':
                    if (nowMode != ParseMode.ReadMode)
                    {
                        builder.Append(originText[i]);
                        break;
                    }
                    string modeStr = builder.ToString();
                    if (modeStr.Contains("bundleName"))
                    {
                        nowMode = ParseMode.BundleName;
                        nowWrapperList = gameData.startWrapperList;
                        nowWrapper = new VisitorDialogWrapper();
                        nowWrapperList.Add(nowWrapper);
                    }
                    else if (modeStr.Contains("switch"))
                    {
                        nowMode = ParseMode.Switch;
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
                        nowMode = ParseMode.Null;
                        nowWrapper.forceEnd = true;
                    }
                    else if (modeStr.Contains("giveCoin"))
                    {
                        nowMode = ParseMode.GiveCoin;

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
                        nowMode = ParseMode.RightMedicine;
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
                        nowMode = ParseMode.RightMedicine;
                        nowWrapper = new VisitorDialogWrapper();
                        nowWrapper.characterName = beforeName;
                        nowWrapper.characterFeeling = beforeFeeling;
                        nowWrapperList = gameData.wrongWrapperList;
                        nowWrapperList.Add(nowWrapper);
                    }
                    else if (modeStr.Contains("skip"))
                    {
                        nowMode = ParseMode.SkipVisitor;
                        nowWrapper = new VisitorDialogWrapper();
                        nowWrapper.characterName = beforeName;
                        nowWrapper.characterFeeling = beforeFeeling;
                        nowWrapperList = gameData.skipWrapperList;
                        nowWrapperList.Add(nowWrapper);
                    }
                    else if (modeStr.Contains("symptom"))
                    {
                        nowMode = ParseMode.Symptom;
                    }
                    else if (modeStr.Contains("disease"))
                    {
                        nowMode = ParseMode.Disease;
                    }
                    else if (modeStr.Contains("visitorSet"))
                    {
                        nowMode = ParseMode.VisitorSet;
                    }
                    else if (modeStr.Contains("region"))
                    {
                        nowMode = ParseMode.NextRegion;
                    }
                    builder.Clear();
                    break;

                case '{':

                    switch (nowMode)
                    {
                        case ParseMode.DialogCharacterName:
                            nowMode = ParseMode.DialogEffect;
                            break;
                        case ParseMode.BundleName:
                            break;
                        case ParseMode.Switch:
                            //string name = builder.ToString();
                            //if (nowWrapper.characterName[leftMiddleRight] == null && name.Length>1)
                            //{
                            //    nowWrapper.characterName[leftMiddleRight] = name;
                            //    nowWrapper.ingameName[leftMiddleRight] = characterIndexToName.NameTranslator(name, languagePack);

                            //}
                            //builder.Clear();
                            nowMode = ParseMode.ClampEffect;
                            break;
                        case ParseMode.ClampFeeling:
                            nowMode = ParseMode.ClampEffect;
                            break;
                        case ParseMode.ClampCharacterName:
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
                            nowMode = ParseMode.ClampEffect;
                            break;
                        case ParseMode.GiveCoin:
                            nowMode = ParseMode.CoinNumber;
                            break;
                        case ParseMode.Symptom:
                            nowMode = ParseMode.SymptomNumber;
                            break;
                        case ParseMode.Disease:
                            nowMode = ParseMode.DiseaseName;
                            break;
                        case ParseMode.VisitorSet:
                            nowMode = ParseMode.VisitorSetNumber;
                            break;
                        case ParseMode.NextRegion:
                            nowMode = ParseMode.NextRegionName;
                            break;
                        default:
                            builder.Append('{');
                            break;

                    }
                    break;
                case '}':
                    switch (nowMode)
                    {
                        case ParseMode.BundleName:
                            gameData.bundleName = builder.ToString();
                            break;
                        case ParseMode.Switch:
                            nowMode = ParseMode.DialogCharacterName;
                            break;
                        case ParseMode.ClampCharacterName:
                            string name = builder.ToString();
                            if (name.Length < 1)
                            {
                                nowWrapper.characterName = null;
                            }
                            else
                            {
                                nowWrapper.characterName = name;

                            }
                            nowMode = ParseMode.ClampFeeling;
                            builder.Clear();
                            break;
                        case ParseMode.ClampFeeling:
                            nowMode = ParseMode.DialogCharacterName;
                            break;
                        case ParseMode.ClampEffect:
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
                            nowMode = ParseMode.Switch;
                            //nowMode = ParseMode.DialogCharacterName;
                            break;
                        //여기 파스모드 컷씬 이펙트랑 백그라운드 이펙트는 하나마나 의미없어서..
                        case ParseMode.CutScene:
                        case ParseMode.BackGroundEffect:
                            nowMode = ParseMode.Null;
                            break;
                        case ParseMode.CoinNumber:
                            nowMode = ParseMode.Null;
                            string num = builder.ToString();
                            nowWrapper.coin = int.Parse(num);
                            break;
                        case ParseMode.SymptomNumber:
                            nowMode = ParseMode.Null;
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
                        case ParseMode.DiseaseName:
                            nowMode = ParseMode.Null;
                            string disease = builder.ToString();
                            gameData.diseaseNameList.Add(disease);
                            break;
                        case ParseMode.VisitorSetNumber:
                            nowMode = ParseMode.Null;
                            gameData.oddVisitorSetArray[nowVisitorSetIndex] = int.Parse(builder.ToString());
                            nowVisitorSetIndex = 0;
                            break;
                        case ParseMode.NextRegionName:
                            nowMode = ParseMode.Null;
                            gameData.storyRegion = (StoryRegion)Enum.Parse(typeof(StoryRegion), builder.ToString());
                            break;

                    }
                    builder.Clear();
                    break;
                case ',':
                    switch (nowMode)
                    {
                        case ParseMode.SymptomNumber:
                            string strNum = builder.ToString();
                            if (nowSymptomIndex >= gameData.symptomNumberArray.Length)
                            {
                                break;
                            }
                            int intNum;
                            if (strNum.Length>0)
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
                        case ParseMode.DiseaseName:
                            string disease = builder.ToString();
                            gameData.diseaseNameList.Add(disease);
                            builder.Clear();
                            break;
                        case ParseMode.VisitorSetNumber:
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
                        case ParseMode.Switch:
                            nowMode = ParseMode.ClampFeeling;
                            break;
                        case ParseMode.ClampCharacterName:
                            string name = builder.ToString();
                            if (name.Length < 1)
                            {
                                nowWrapper.characterName = null;

                            }
                            else
                            {
                                nowWrapper.characterName = name;
                            }
                            nowMode = ParseMode.ClampFeeling;
                            builder.Clear();
                            break;
                        case ParseMode.ClampEffect:
                            nowMode = ParseMode.ClampFeeling;
                            break;
                        case ParseMode.ClampFeeling:
                            break;

                        case ParseMode.Dialog:
                            builder.Append('(');
                            break;
                    }

                    break;
                case ')':
                    switch (nowMode)
                    {
                        case ParseMode.ClampFeeling:
                            string feeling = builder.ToString();
                            nowWrapper.characterFeeling = feeling;
                            nowMode = ParseMode.Switch;
                            builder.Clear();
                            break;
                        case ParseMode.Dialog:
                                builder.Append(')');
                            break;
                    }



                    break;
                case '[':
                    if (nowMode == ParseMode.Dialog)
                    {
                        nowDialog.dialog = builder.ToString();
                    }
                    builder.Clear();
                    nowMode = ParseMode.DialogCharacterName;
                    VisitorDialog dialog = new VisitorDialog();
                    nowDialog = dialog;
                    nowWrapper.dialogList.Add(dialog);
                    break;
                case ']':
                    if (builder.ToString().Contains("Ruellia"))
                        nowDialog.ruelliaTalking = true;
                    else
                        nowDialog.ruelliaTalking = false;
                    nowMode = ParseMode.Dialog;
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
