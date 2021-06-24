using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//이거 안쓸수도 있음.
[System.Serializable]
public class ConversationDialogBundleWrapper
{
    public List<ConversationDialogBundle> conversationDialogBundleList;

    public ConversationDialogBundleWrapper()
    {
        conversationDialogBundleList = new List<ConversationDialogBundle>();

        conversationDialogBundleList.Add(new ConversationDialogBundle());
        conversationDialogBundleList[0].bundleName = "bundle";

        conversationDialogBundleList.Add(new ConversationDialogBundle());
    }
}
