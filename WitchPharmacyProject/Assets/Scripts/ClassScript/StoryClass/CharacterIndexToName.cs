using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class CharacterIndexToName
{
    public string[] characterNameArray = { "Ruelia", "Cari" };
    public Sprite[,] characterSprite;
    const int characterNumber = 2;

    public CharacterIndexToName()
    {
        characterSprite = new Sprite[characterNumber,2];
    }

    public Sprite GetSprite(int index,CharacterFeeling feeling)
    {
        if (characterSprite[index,(int)feeling] == null)
        {
            StringBuilder nameBuilder = new StringBuilder("CharacterSprite/");
            nameBuilder.Append(characterNameArray[index]);
            nameBuilder.Append("/");
            nameBuilder.Append(feeling.ToString());
            characterSprite[index,(int)feeling] = Resources.Load<Sprite>(nameBuilder.ToString());

        }
        return characterSprite[index,(int)feeling];
    }

    public string GetName(int index)
    {
        return characterNameArray[index];
    }
}
