using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//1개의 속성에 따른 증상에 대한 대사 모음집
//물 -2 면 dialogArray[0]에 넣으면 된다는 뜻.
//symptomDialog에 들어가있어서 거기서 저장해줄 것.
//제이슨으로 기획자가 줄거임.
[System.Serializable]
public class DialogBundle //상훈
{
    public string[] dialogArray;
    //이거 4개로 고정할거임

    public DialogBundle()
    {
        dialogArray = new string[4];
        dialogArray[0] = "-2 증상";
        dialogArray[1] = "-1 증상";
        dialogArray[2] = "1 증상";
        dialogArray[3] = "2 증상";

    }
}
