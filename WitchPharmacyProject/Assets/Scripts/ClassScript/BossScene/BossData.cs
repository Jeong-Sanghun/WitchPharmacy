using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BossData
{
    public string characterFileName;
    public string nextStory;
    public int health;
    public int timeLimit;
    public string conversationText;
    [System.NonSerialized]
    public CharacterName characterEnum;
    [System.NonSerialized]
    public string conversationTextList;



}
