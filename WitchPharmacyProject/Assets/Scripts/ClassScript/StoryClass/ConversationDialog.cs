using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//대사 하나를 위한거. 이걸 Bundle에서 묶어주고 제이슨으로 받을거.
public enum CharacterFeeling { nothing, angry }

[System.Serializable]
public class ConversationDialog
{
    public int index;
    public int leftCharacterIndex;
    public int rightCharacterIndex;
    public bool leftFade;
    public bool rightFade;
    public CharacterFeeling leftCharacterFeeling;
    public CharacterFeeling rightCharacterFeeling;
    public string dialog;

    public ConversationDialog()
    {
        leftCharacterFeeling = CharacterFeeling.nothing;
        rightCharacterFeeling = CharacterFeeling.nothing;
    }

}
