using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterPos
{
    Left, Middle, Right, Null
}
public enum DialogFX
{
    Up, Down, Null
}
public class DialogEffect
{
    public CharacterPos characterPosition;
    public DialogFX effect;
    public DialogEffect()
    {
        characterPosition = CharacterPos.Left;
        effect = DialogFX.Up;
    }
}
