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
            ,BackGround,BackGroundName,BackGroundEffect
    }

    CharacterIndexToName characterIndexToName;
    UILanguagePack languagePack;
    public StoryParser(CharacterIndexToName indexToName, UILanguagePack language)
    {
        characterIndexToName = indexToName;
        languagePack = language;
    }

    public ConversationDialogBundle LoadBundle(string bundleName,string languageDirectory,bool isRegion)
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
            if(i == originText.Length - 1)
            {
                nowDialog.dialog = builder.ToString();
            }
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
                    for(int j = 0; j < 3; j++)
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
            
        }

        return gameData;
        //이 정보를 게임매니저나, 로딩으로 넘겨주는 것이당
    }
    
}
