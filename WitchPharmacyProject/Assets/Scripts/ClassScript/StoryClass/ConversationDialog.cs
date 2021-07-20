using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//대사 하나를 위한거. 이걸 Bundle에서 묶어주고 제이슨으로 받을거.


[System.Serializable]
public class ConversationDialog
{
    //이거 인덱스 번호 몇번인지. 사실 크게 의미없음. 혹시몰라서 넣어두는거
    //public int index;
    //왼쪽 캐릭터 인덱스 몇번인지. 이걸 CharacterToIndex
    //int leftCharacterIndex;
    //int rightCharacterIndex;
    public string ingameName;
    public string fx;

    public string leftCharacterName;
    public string middleCharacterName;
    public string rightCharacterName;
    public string backGroundSpriteFileName;
    public bool isCutScene;
    public string cutSceneFileName;

    //왼쪽놈 페이드할지 말지. 으론쪽놈 페이드할지 말지
    public bool leftFade;
    public bool rightFade;
    //왼쪽 기분 오른쪽 기분.
    public string leftCharacterFeeling;
    public string rightCharacterFeeling;

    //내용.
    public string dialog;

    //public bool endOfWrapper;

    public ConversationDialog()
    {
        backGroundSpriteFileName = "test";
        isCutScene = false;
        cutSceneFileName = "test";
        //leftCharacterFeeling = CharacterFeeling.nothing;
        //rightCharacterFeeling = CharacterFeeling.nothing;
    }

    //public int GetLeftIndex()
    //{
    //    return leftCharacterIndex;
    //}

    //public int GetRightIndex()
    //{
    //    return rightCharacterIndex;
    //}

}
