using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//한 뭉탱이의 다이얼로그
[System.Serializable]
public class ConversationDialogBundle
{

    //분기를 위해서 이렇게 넣어둠
    //분기를 하기 위하여 조건을 더 넣어야 하는데 그거는 기획이 나오면 할것. 일단은 틀만 잡아두기 위하여 이렇게 넣어둠.
    //어떤 식으로 하면 어떻게 분기를 하고 이런 조건을 넣어야하잖아.
    public string bundleName;
    public List<ConversationDialogWrapper> dialogWrapperList;
    public ConversationRouter conversationRouter;
    public string nextBundleName;


    public ConversationDialogBundle()
    {
        bundleName = "testBundle";

        dialogWrapperList = new List<ConversationDialogWrapper>();

        ConversationDialogWrapper wrapper = new ConversationDialogWrapper(0);
        dialogWrapperList.Add(wrapper);
        wrapper = new ConversationDialogWrapper(1);
        dialogWrapperList.Add(wrapper);
        wrapper = new ConversationDialogWrapper(2);
        dialogWrapperList.Add(wrapper);

        conversationRouter = new ConversationRouter();

        nextBundleName = "testBundle";

    }
}
