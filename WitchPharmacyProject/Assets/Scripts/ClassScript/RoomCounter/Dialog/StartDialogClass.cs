using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StartDialogClass
{
    public List<StringWrapper> dialogList;

    public StartDialogClass(int index)
    {
        dialogList = new List<StringWrapper>();
        if(index == 0)
        {
            dialogList.Add(new StringWrapper("안졸려 ㅡㅡ"));
            dialogList.Add(new StringWrapper("잠이나 잘까"));
            dialogList.Add(new StringWrapper("장사중에 잠을 자는 사람이 어디있어! 열심히 꺠야지!"));
            dialogList.Add(new StringWrapper("오늘도 화이팅이야!"));
        }
        else
        {
            dialogList.Add(new StringWrapper("어제 약초를 많이 캐러 다녔더니 안배고프네...."));
            dialogList.Add(new StringWrapper("뭐좀 먹을게 없을까?"));
            dialogList.Add(new StringWrapper("불닭맛 사탕..? 이건 안돼!"));
            dialogList.Add(new StringWrapper("배고프지만 좀만 참아야지! 오늘도 화이팅!"));
        }
    }
}
