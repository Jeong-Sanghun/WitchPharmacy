using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//어느 버튼이 눌리면 
[System.Serializable]
public class ConversationRouter
{
    public List<string> routingWrapperName;
    //public List<ConversationDialog> routeDialog;
    public List<string> routeButtonText;
    public int routingTime;

    public ConversationRouter()
    {
        routingWrapperName = new List<string>();
        routingWrapperName.Add("1");
        routingWrapperName.Add("2");
        routingTime = 1;

        routeButtonText = new List<string>();
        routeButtonText.Add("(근데 좀 열받네...)");
        routeButtonText.Add("(근데 우리집엔 창문이 없잖아...?)");

    }
}
