using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//어느 버튼이 눌리면 어디로 갈 지.
[System.Serializable]
public class ConversationRouter
{
    //public List<int> routingWrapperIndex;
    public List<int> routingWrapperIndex;
    //자 봐봐. 라우팅 안에서도 switch가 일어날 꺼 아니야.
    //그러면 routingWrapper가 여러 개가 있을거라는거지. 그러면
    //0번 라우터 버튼이 어디부터 어디까지 범위인지 알아야돼.
    //routingWrapperIndex[0]이 3이라 치면, 0 1 2까지 표출이라는 거.
    //routingWrapperindex[1]이 5라 치면, 3 4가 표출.
    public List<ConversationDialogWrapper> routingWrapperList;
    //public List<ConversationDialog> routeDialog;
    public List<string> routeButtonText;

    public ConversationRouter()
    {
        routingWrapperList = new List<ConversationDialogWrapper>();
        routeButtonText = new List<string>();
        routingWrapperIndex = new List<int>();

    }
}
