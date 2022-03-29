using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class StoryDialog 
{
    public string effect;
    public string effectParameter;
    public string talkingCharName;
    public string dialog;

    public string charSlot1;
    public string charEffect1;
    public string charEffectParameter1;

    public string charSlot2;
    public string charEffect2;
    public string charEffectParameter2;

    public string charSlot3;
    public string charEffect3;
    public string charEffectParameter3;

    public string charSlot4;
    public string charEffect4;
    public string charEffectParameter4;

    public string routeFirst;
    public string routeSecond;
    public string routeThird;

    public string routeFirstJump;
    public string routeSecondJump;
    public string routeThirdJump;

    [System.NonSerialized]
    public CharacterName[] enumCharacterArray;

    public StoryDialog()
    {
        enumCharacterArray = new CharacterName[4];
        for(int i = 0; i < enumCharacterArray.Length; i++)
        {
            enumCharacterArray[i] = CharacterName.Null;
        }
    }


}
