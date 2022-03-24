using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;

namespace Obsolete
{

    public class StoryParser
    {


        CharacterIndexToName characterIndexToName;
        UILanguagePack languagePack;
        public StoryParser(CharacterIndexToName indexToName, UILanguagePack language)
        {
            characterIndexToName = indexToName;
            languagePack = language;
        }

        public ConversationDialogBundle LoadBundle(string bundleName, string languageDirectory, bool isRegion = false, RegionName regionName = RegionName.Library, StoryRegion storyRegion = StoryRegion.Narin)
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
            StoryEffect nowMode = StoryEffect.Null;
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
                        if (nowMode == StoryEffect.Dialog)
                        {
                            Debug.Log(builder.ToString());
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
                            nowWrapperList = gameData.dialogWrapperList;
                        }
                        else if (modeStr.Contains("switch"))
                        {
                            nowMode = StoryEffect.Switch;
                            if (nowWrapper == null)
                            {
                                nowWrapper = new ConversationDialogWrapper();
                                nowWrapperList.Add(nowWrapper);
                            }
                            else if (nowWrapper.conversationDialogList.Count != 0)
                            {
                                ConversationDialogWrapper wrapper = nowWrapper;
                                nowWrapper = new ConversationDialogWrapper();
                                nowWrapper.CopyExceptCharacter(wrapper);
                                nowWrapperList.Add(nowWrapper);
                            }
                        }
                        else if (modeStr.Contains("route"))
                        {
                            if (nowWrapper != null)
                                nowWrapper.nextWrapperIsRouter = true;
                            nowMode = StoryEffect.Route;
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
                            nowMode = StoryEffect.CutScene;
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
                            nowMode = StoryEffect.BackGround;
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
                                wrapper.Copy(nowWrapper);

                                nowWrapper = wrapper;
                                nowWrapperList.Add(wrapper);
                            }
                        }
                        else if (modeStr.Contains("nextStory"))
                        {
                            nowMode = StoryEffect.NextStory;
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
                                wrapper.Copy(nowWrapper);
                                nowWrapper = wrapper;
                                nowWrapperList.Add(wrapper);
                            }
                        }
                        else if (modeStr.Contains("forcedRegion"))
                        {
                            nowMode = StoryEffect.ForcedRegion;
                        }
                        else if (modeStr.Contains("region"))
                        {
                            nowMode = StoryEffect.NextRegion;
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
                                wrapper.Copy(nowWrapper);
                                nowWrapper = wrapper;
                                nowWrapperList.Add(wrapper);
                            }
                        }
                        else if (modeStr.Contains("sfx"))
                        {
                            nowMode = StoryEffect.Sfx;
                            ConversationDialogWrapper wrapper;
                            wrapper = new ConversationDialogWrapper();
                            if (nowWrapper != null)
                            {
                                wrapper.Copy(nowWrapper);
                            }
                            nowWrapper = wrapper;
                            nowWrapperList.Add(wrapper);
                        }
                        else if (modeStr.Contains("effect"))
                        {
                            nowMode = StoryEffect.Effect;
                            ConversationDialogWrapper wrapper;
                            wrapper = new ConversationDialogWrapper();
                            if (nowWrapper != null)
                            {
                                wrapper.Copy(nowWrapper);
                            }
                            nowWrapper = wrapper;
                            nowWrapperList.Add(wrapper);
                        }
                        else if (modeStr.Contains("bgm"))
                        {
                            nowMode = StoryEffect.Bgm;
                            ConversationDialogWrapper wrapper;
                            wrapper = new ConversationDialogWrapper();
                            if (nowWrapper != null)
                            {
                                wrapper.Copy(nowWrapper);
                            }
                            nowWrapper = wrapper;
                            nowWrapperList.Add(wrapper);
                        }
                        else if (modeStr.Contains("conceal"))
                        {
                            nowMode = StoryEffect.Conceal;
                            ConversationDialogWrapper wrapper;
                            wrapper = new ConversationDialogWrapper();
                            if (nowWrapper != null)
                            {
                                wrapper.Copy(nowWrapper);
                            }
                            nowWrapper = wrapper;
                            nowWrapperList.Add(wrapper);
                        }
                        else if (modeStr.Contains("unConceal"))
                        {
                            nowMode = StoryEffect.UnConceal;
                            ConversationDialogWrapper wrapper;
                            wrapper = new ConversationDialogWrapper();
                            if (nowWrapper != null)
                            {
                                wrapper.Copy(nowWrapper);
                            }
                            nowWrapper = wrapper;
                            nowWrapperList.Add(wrapper);
                        }
                        else if (modeStr.Contains("popup"))
                        {
                            nowMode = StoryEffect.Popup;
                            ConversationDialogWrapper wrapper;
                            wrapper = new ConversationDialogWrapper();
                            if (nowWrapper != null)
                            {
                                wrapper.Copy(nowWrapper);
                            }
                            nowWrapper = wrapper;
                            nowWrapperList.Add(wrapper);
                        }
                        else if (modeStr.Contains("uiUnlock"))
                        {
                            nowMode = StoryEffect.UIUnlock;
                        }

                        //        Sfx,SfxName,Effect,EffectName,Bgm,BgmName,
                        //Conceal,ConcealName,UnConceal,UnConcealName,UIUnlock,UIUnlockName

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
                                //string name = builder.ToString();
                                //if (nowWrapper.characterName[leftMiddleRight] == null && name.Length>1)
                                //{
                                //    nowWrapper.characterName[leftMiddleRight] = name;
                                //    nowWrapper.ingameName[leftMiddleRight] = characterIndexToName.NameTranslator(name, languagePack);

                                //}
                                //builder.Clear();
                                nowMode = StoryEffect.ClampCharacterName;
                                break;
                            case StoryEffect.Route:
                                nowMode = StoryEffect.RouteText;
                                break;
                            case StoryEffect.ClampFeeling:
                                nowMode = StoryEffect.ClampEffect;
                                break;
                            case StoryEffect.ClampCharacterName:
                                string name = builder.ToString();
                                nowWrapper.characterName[leftMiddleRight] = name;
                                nowWrapper.ingameName[leftMiddleRight] = characterIndexToName.NameTranslator(name, languagePack);
                                builder.Clear();
                                nowMode = StoryEffect.ClampEffect;
                                break;
                            case StoryEffect.CutScene:
                                nowMode = StoryEffect.CutSceneFileName;
                                break;
                            case StoryEffect.BackGround:
                                nowMode = StoryEffect.BackGroundName;
                                break;
                            case StoryEffect.NextStory:
                                nowMode = StoryEffect.NextStoryName;
                                break;
                            case StoryEffect.NextRegion:
                                nowMode = StoryEffect.NextRegionName;
                                break;
                            case StoryEffect.ForcedRegion:
                                nowMode = StoryEffect.ForcedRegionName;
                                break;
                            case StoryEffect.Effect:
                                nowMode = StoryEffect.EffectName;
                                break;
                            case StoryEffect.Sfx:
                                nowMode = StoryEffect.SfxName;
                                break;
                            case StoryEffect.Bgm:
                                nowMode = StoryEffect.BgmName;
                                break;

                            case StoryEffect.Conceal:
                                nowMode = StoryEffect.ConcealName;
                                break;
                            case StoryEffect.Popup:
                                nowMode = StoryEffect.PopupName;
                                break;

                            case StoryEffect.UnConceal:
                                nowMode = StoryEffect.UnConcealName;
                                break;
                            case StoryEffect.UIUnlock:
                                nowMode = StoryEffect.UIUnlockName;
                                break;



                            //        Sfx,SfxName,Effect,EffectName,Bgm,BgmName,
                            //Conceal,ConcealName,UnConceal,UnConcealName,UIUnlock,UIUnlockName

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
                                break;
                            case StoryEffect.Switch:
                                nowMode = StoryEffect.DialogCharacterName;
                                leftMiddleRight = 0;
                                break;
                            case StoryEffect.ClampCharacterName:
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
                                nowMode = StoryEffect.ClampFeeling;
                                builder.Clear();
                                break;
                            case StoryEffect.ClampFeeling:
                                nowMode = StoryEffect.DialogCharacterName;
                                leftMiddleRight = 0;
                                break;
                            case StoryEffect.ClampEffect:
                                DialogEffect effect = new DialogEffect();
                                string effectStr = builder.ToString();
                                nowWrapper.startEffectList.Add(effect);
                                if (effectStr.Contains("up") || effectStr.Contains("Up"))
                                {
                                    effect.effect = DialogFX.Up;
                                }
                                else if (effectStr.Contains("down") || effectStr.Contains("Down"))
                                {
                                    effect.effect = DialogFX.Down;
                                }
                                else if (effectStr.Contains("left") || effectStr.Contains("Left"))
                                {
                                    effect.effect = DialogFX.Left;
                                }
                                else if (effectStr.Contains("right") || effectStr.Contains("Right"))
                                {
                                    effect.effect = DialogFX.Right;
                                }
                                else if (effectStr.Contains("blur") || effectStr.Contains("Blur"))
                                {
                                    effect.effect = DialogFX.Blur;
                                }

                                effect.characterPosition = (CharacterPos)leftMiddleRight;

                                nowMode = StoryEffect.Switch;
                                break;
                            case StoryEffect.RouteText:
                                routingNumber++;
                                nowRouter.routeButtonText.Add(builder.ToString());
                                nowMode = StoryEffect.Route;
                                break;
                            case StoryEffect.CutSceneFileName:
                                nowMode = StoryEffect.Null;
                                nowWrapper.cutSceneFileName = builder.ToString();
                                break;
                            case StoryEffect.BackGroundName:
                                nowMode = StoryEffect.Null;
                                nowWrapper.backGroundFileName = builder.ToString();
                                break;
                            //여기 파스모드 컷씬 이펙트랑 백그라운드 이펙트는 하나마나 의미없어서..
                            case StoryEffect.CutScene:
                            case StoryEffect.BackGroundEffect:
                                nowMode = StoryEffect.Null;
                                break;
                            case StoryEffect.NextStoryName:
                                nowMode = StoryEffect.Null;
                                Debug.Log(builder.ToString());
                                nowWrapper.nextStory = builder.ToString();
                                break;
                            case StoryEffect.NextRegionName:
                                nowMode = StoryEffect.Null;
                                nowWrapper.nextRegion = builder.ToString();
                                break;
                            case StoryEffect.ForcedRegionName:
                                nowMode = StoryEffect.Null;
                                gameData.forcedRegion = builder.ToString();
                                break;
                            case StoryEffect.EffectName:
                                nowMode = StoryEffect.Null;
                                nowWrapper.effect = builder.ToString();
                                break;
                            case StoryEffect.Sfx:
                                nowMode = StoryEffect.SfxName;
                                nowWrapper.sfx = builder.ToString();
                                break;
                            case StoryEffect.BgmName:
                                nowMode = StoryEffect.Null;
                                nowWrapper.bgm = builder.ToString();
                                break;
                            case StoryEffect.PopupName:
                                nowMode = StoryEffect.Null;
                                nowWrapper.popUp = builder.ToString();
                                break;
                            case StoryEffect.ConcealName:
                                nowMode = StoryEffect.Null;
                                nowWrapper.concealedCharacter.Add(characterIndexToName.NameTranslator(builder.ToString(), languagePack));
                                break;
                            case StoryEffect.UnConcealName:
                                nowMode = StoryEffect.Null;
                                nowWrapper.concealedCharacter.Remove(characterIndexToName.NameTranslator(builder.ToString(), languagePack));
                                break;
                            case StoryEffect.UIUnlockName:
                                nowMode = StoryEffect.Null;
                                gameData.uiUnlock = builder.ToString();
                                break;
                        }
                        builder.Clear();
                        break;
                    case '(':
                        switch (nowMode)
                        {
                            case StoryEffect.Switch:
                                break;
                            case StoryEffect.ClampCharacterName:
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
                                nowMode = StoryEffect.ClampFeeling;
                                builder.Clear();
                                break;
                            case StoryEffect.ClampEffect:
                            case StoryEffect.ClampFeeling:
                                break;
                            case StoryEffect.Route:
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
                            case StoryEffect.CutSceneFileName:
                                nowWrapper.cutSceneFileName = builder.ToString();
                                nowMode = StoryEffect.CutSceneEffect;
                                builder.Clear();
                                break;
                            case StoryEffect.BackGroundName:
                                nowWrapper.backGroundFileName = builder.ToString();
                                nowMode = StoryEffect.BackGroundEffect;
                                builder.Clear();
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
                                nowWrapper.characterFeeling[leftMiddleRight] = feeling;
                                //nowMode = ParseMode.Switch;
                                builder.Clear();
                                break;
                            case StoryEffect.Route:
                                builder.Clear();
                                break;
                            case StoryEffect.CutSceneEffect:
                                string effect = builder.ToString();
                                if (effect.Contains("blur") || effect.Contains("Blur"))
                                {
                                    nowWrapper.cutSceneEffect = CutSceneEffect.Blur;
                                }
                                builder.Clear();
                                break;
                            case StoryEffect.BackGroundEffect:
                                effect = builder.ToString();
                                if (effect.Contains("blur") || effect.Contains("Blur"))
                                {
                                    nowWrapper.backGroundEffect = CutSceneEffect.Blur;
                                }
                                builder.Clear();
                                break;
                            case StoryEffect.Dialog:
                                if (isRouting)
                                {
                                    nowDialog.dialog = builder.ToString();
                                    routingNumber--;
                                    if (routingNumber > 0)
                                    {
                                        nowMode = StoryEffect.Route;

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
                                        nowMode = StoryEffect.RightAfterRoute;
                                        Debug.Log("여기실행되니");
                                        nowWrapperList = gameData.dialogWrapperList;
                                        ConversationDialogWrapper wrapper = new ConversationDialogWrapper();
                                        for (int j = 0; j < 3; j++)
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
                        if (nowMode == StoryEffect.Dialog)
                        {
                            nowDialog.dialog = builder.ToString();
                        }
                        builder.Clear();
                        nowMode = StoryEffect.DialogCharacterName;
                        ConversationDialog dialog = new ConversationDialog();
                        nowDialog = dialog;
                        nowWrapper.conversationDialogList.Add(dialog);
                        break;
                    case ']':
                        nowDialog.ingameName = builder.ToString();
                        for (int j = 0; j < 4; j++)
                        {
                            if (nowDialog.ingameName.Length > 1)
                            {
                                if (nowWrapper.ingameName[j] == null)
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
                        nowMode = StoryEffect.Dialog;
                        builder.Clear();
                        break;
                    case ',':
                        switch (nowMode)
                        {
                            case StoryEffect.ClampCharacterName:
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
                                nowMode = StoryEffect.ClampCharacterName;
                                builder.Clear();
                                break;
                            case StoryEffect.Switch:
                            case StoryEffect.ClampFeeling:
                            case StoryEffect.ClampEffect:
                                leftMiddleRight++;
                                nowMode = StoryEffect.ClampCharacterName;
                                break;

                            case StoryEffect.ConcealName:
                                nowWrapper.concealedCharacter.Add(builder.ToString());
                                builder.Clear();
                                break;
                            case StoryEffect.UnConcealName:
                                nowWrapper.concealedCharacter.Remove(builder.ToString());
                                builder.Clear();
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
                if (i == originText.Length - 1 && nowMode == StoryEffect.Dialog)
                {

                    nowDialog.dialog = builder.ToString();
                }


            }

            return gameData;
            //이 정보를 게임매니저나, 로딩으로 넘겨주는 것이당
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
            StoryEffect nowMode = StoryEffect.Null;
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
                            nowMode = StoryEffect.Null;
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
                            nowMode = StoryEffect.RightMedicine;
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
                            nowMode = StoryEffect.RightMedicine;
                            nowWrapper = new VisitorDialogWrapper();
                            nowWrapper.characterName = beforeName;
                            nowWrapper.characterFeeling = beforeFeeling;
                            nowWrapperList = gameData.wrongWrapperList;
                            nowWrapperList.Add(nowWrapper);
                        }
                        else if (modeStr.Contains("skip"))
                        {
                            nowMode = StoryEffect.SkipVisitor;
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
                            //여기 파스모드 컷씬 이펙트랑 백그라운드 이펙트는 하나마나 의미없어서..
                            case StoryEffect.CutScene:
                            case StoryEffect.BackGroundEffect:
                                nowMode = StoryEffect.Null;
                                break;
                            case StoryEffect.CoinNumber:
                                nowMode = StoryEffect.Null;
                                string num = builder.ToString();
                                nowWrapper.coin = int.Parse(num);
                                break;
                            case StoryEffect.SymptomNumber:
                                nowMode = StoryEffect.Null;
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
                                nowMode = StoryEffect.Null;
                                string disease = builder.ToString();
                                gameData.diseaseNameList.Add(disease);
                                break;
                            case StoryEffect.VisitorSetNumber:
                                nowMode = StoryEffect.Null;
                                gameData.oddVisitorSetArray[nowVisitorSetIndex] = int.Parse(builder.ToString());
                                nowVisitorSetIndex = 0;
                                break;
                            case StoryEffect.NextRegionName:
                                nowMode = StoryEffect.Null;
                                gameData.storyRegion = (StoryRegion)Enum.Parse(typeof(StoryRegion), builder.ToString());
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

}