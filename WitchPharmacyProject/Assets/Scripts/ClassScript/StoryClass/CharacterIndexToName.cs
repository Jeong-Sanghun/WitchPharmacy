using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;


//캐릭터 인덱스를 받아서 이름으로 바꿔주는거. 이거 ResourceLoad할떄 씀
public class CharacterIndexToName
{
    //스프라이트 여기서 로딩해옴.
    public string[] characterNameArray = { "Ruelia", "Cari" };
    public Sprite[,] characterSprite;
    //각 스프라이트 뭔지.
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
