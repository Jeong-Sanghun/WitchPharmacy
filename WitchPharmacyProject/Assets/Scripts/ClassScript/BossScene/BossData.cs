using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class BossData
{
    public string fileName;
    public string characterName;
    public string nextStory;
    public int health;
    public int timeLimit;
    public string conversationText;
    [System.NonSerialized]
    public CharacterName characterEnum;
    [System.NonSerialized]
    public string[] conversationTextList;

    public BossData()
    {
        fileName = "Ian";
        characterName = "Ian";
        nextStory = "fifthStory";
        health = 5;
        timeLimit = 600;
        conversationText = "아몰랑/나아퍼/많이아퍼/도와줘/나고쳐줘/나는 이안이야";
    }

    public void SetUp()
    {
        
        conversationTextList = conversationText.Split('/');
        characterEnum = (CharacterName)Enum.Parse(typeof(CharacterName), characterName);
    }
}
