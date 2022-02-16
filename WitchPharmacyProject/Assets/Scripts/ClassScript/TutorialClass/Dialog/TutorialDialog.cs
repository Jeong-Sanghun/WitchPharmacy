using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum DialogType
{
    Null,Dialog,VisitorDialog
}
[Serializable]
public class TutorialDialog
{
    public string typeString;
    public string characterString;
    public string feelingString;
    public string actionKeyword;
    public string actionKeywordParameter;
    public string dialog;
    public string routeFirst;
    public string routeSecond;
    public string routeThird;
    public string routeFirstJump;
    public string routeSecondJump;
    public string routeThirdJump;
    [NonSerialized]
    public DialogType type;
    [NonSerialized]
    public CharacterName character;
    [NonSerialized]
    public CharacterFeeling feeling;
    [NonSerialized]
    public List<TutorialRoute> routeList;
    [NonSerialized]
    public ActionClass action;



    public TutorialDialog()
    {
        typeString = null;
        characterString = null;
        feelingString = null;
        actionKeyword = null;
        actionKeywordParameter = null;
        type = DialogType.Null;
        character = CharacterName.Null;
        feeling = CharacterFeeling.Null;
        dialog = null;
        routeFirst = null;
        routeSecond = null;
        routeThird = null;
        routeFirstJump = null;
        routeSecondJump = null;
        routeThirdJump = null;
        action = null;
        routeList = null;
      
    }

    public void Parse()
    {
        if(typeString!= null)
        type = (DialogType)Enum.Parse(typeof(DialogType), typeString);
        if(characterString!=null)
        character = (CharacterName)Enum.Parse(typeof(CharacterName), characterString);
        if(feelingString!=null)
        feeling = (CharacterFeeling)Enum.Parse(typeof(CharacterFeeling), feelingString);

        if (actionKeyword != null)
        {
            action = new ActionClass();
            action.action = (ActionKeyword)Enum.Parse(typeof(ActionKeyword), actionKeyword);
            action.parameter = 0;
            if(actionKeywordParameter != null)
            {
                action.parameter = float.Parse(actionKeywordParameter);
            }
        }

        if (routeFirst != null)
        {
            routeList = new List<TutorialRoute>();
            TutorialRoute route = new TutorialRoute();
            route.routeString = routeFirst;
            route.jump = int.Parse(routeFirstJump);
            routeList.Add(route);
        }
        if (routeSecond != null)
        {
            
            TutorialRoute route = new TutorialRoute();
            route.routeString = routeSecond;
            route.jump = int.Parse(routeSecondJump);
            routeList.Add(route);
        }
        if (routeThird != null)
        {
            
            TutorialRoute route = new TutorialRoute();
            route.routeString = routeThird;
            route.jump = int.Parse(routeThirdJump);
            routeList.Add(route);
        }
    }
}
