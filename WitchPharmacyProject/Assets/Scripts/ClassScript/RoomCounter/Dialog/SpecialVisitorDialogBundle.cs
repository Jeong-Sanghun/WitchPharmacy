using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpecialVisitorDialogBundle
{
    public string bundleName;
    public string characterName;
    public ConversationRouter conversationRouter;
    public List<SpecialVisitorDialogWrapper> specialVisitorDialogWrapperList;
}
