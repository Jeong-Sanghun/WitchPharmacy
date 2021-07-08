using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpecialVisitorDialogBundle
{
    public string bundleName;
    public string characterName;
    public string answerMedicineName;
    public ConversationRouter conversationRouter;
    public List<SpecialVisitorDialogWrapper> specialVisitorDialogWrapperList;
    public SpecialVisitorDialogWrapper answerDialogWrapper;
    public SpecialVisitorDialogWrapper wrongDialogWrapper;

    public SpecialVisitorDialogBundle()
    {
        bundleName = "testBundle";
        characterName = "Lily";
        conversationRouter = new ConversationRouter();
        specialVisitorDialogWrapperList = new List<SpecialVisitorDialogWrapper>();
        for(int i = 0; i < 3; i++)
        {
            specialVisitorDialogWrapperList.Add(new SpecialVisitorDialogWrapper(i));
        }
    }
}
