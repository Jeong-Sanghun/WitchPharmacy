﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpecialVisitorDialogBundle
{
    public string bundleName;
    public string characterName;
    public string answerSpecialMedicineName;
    public int[] symptomNumberArray;
    public float appearingProgression;
    public float progressingNumber;
    public List<string> appearingConditionQuestList;
    public ConversationRouter conversationRouter;
    public List<SpecialVisitorDialogWrapper> specialVisitorDialogWrapperList;
    public SpecialVisitorDialogWrapper answerDialogWrapper;
    public SpecialVisitorDialogWrapper wrongDialogWrapper;

    public SpecialVisitorDialogBundle()
    {
        bundleName = "testBundle";
        characterName = "Lily";
        appearingProgression = 0;
        progressingNumber = 10;
        appearingConditionQuestList = new List<string>();
        appearingConditionQuestList.Add("testBundle");
        conversationRouter = new ConversationRouter();
        specialVisitorDialogWrapperList = new List<SpecialVisitorDialogWrapper>();
        for(int i = 0; i < 3; i++)
        {
            specialVisitorDialogWrapperList.Add(new SpecialVisitorDialogWrapper(i));
        }
    }
}