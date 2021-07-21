using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;

public enum CharacterFeeling { nothing, angry }
public enum CharacterName
{
    Ruellia, Cari, Jet, Lily, Iris, Ian
}

public class BackGroundPair
{
    public string fileName;
    public Sprite backGroundSprite;

    public BackGroundPair(string name, Sprite spr)
    {
        fileName = name;
        backGroundSprite = spr;
    }
}

//캐릭터 인덱스를 받아서 이름으로 바꿔주는거. 이거 ResourceLoad할떄 씀
public class CharacterIndexToName
{
    //스프라이트 여기서 로딩해옴.
    //static string[] characterNameArray = { "Ruelia", "Cari", "Jet", "Lily" ,"Iris","Ian"};
    static Sprite[,] characterSprite;
    //각 스프라이트 뭔지.
    const int characterNumber = 6;
    List<BackGroundPair> backGroundList;
    List<BackGroundPair> cutSceneList;

    public CharacterIndexToName()
    {
        characterSprite = new Sprite[characterNumber,2];
        backGroundList = new List<BackGroundPair>();
        cutSceneList = new List<BackGroundPair>();
    }

    //public Sprite GetSprite(int index,CharacterFeeling feeling)
    //{
    //    if (characterSprite[index,(int)feeling] == null)
    //    {
    //        StringBuilder nameBuilder = new StringBuilder("CharacterSprite/");
    //        nameBuilder.Append(characterNameArray[index]);
    //        nameBuilder.Append("/");
    //        nameBuilder.Append(feeling.ToString());
    //        characterSprite[index,(int)feeling] = Resources.Load<Sprite>(nameBuilder.ToString());

    //    }
    //    return characterSprite[index,(int)feeling];
    //}

    public Sprite GetSprite(CharacterName name, CharacterFeeling feeling)
    {
        if (characterSprite[(int)name, (int)feeling] == null)
        {
            StringBuilder nameBuilder = new StringBuilder("CharacterSprite/");
            nameBuilder.Append(name);
            nameBuilder.Append("/");
            nameBuilder.Append(feeling.ToString());
            characterSprite[(int)name, (int)feeling] = Resources.Load<Sprite>(nameBuilder.ToString());

        }
        return characterSprite[(int)name, (int)feeling];
    }

    public Sprite GetSprite(string nameText, string feelingText)
    {
        CharacterName name = (CharacterName)Enum.Parse(typeof(CharacterName), nameText);
        CharacterFeeling feeling = (CharacterFeeling)Enum.Parse(typeof(CharacterFeeling), feelingText);
        if (characterSprite[(int)name, (int)feeling] == null)
        {
            StringBuilder nameBuilder = new StringBuilder("CharacterSprite/");
            nameBuilder.Append(name);
            nameBuilder.Append("/");
            nameBuilder.Append(feeling.ToString());
            characterSprite[(int)name, (int)feeling] = Resources.Load<Sprite>(nameBuilder.ToString());

        }
        return characterSprite[(int)name, (int)feeling];
    }

    public Sprite GetBackGroundSprite(string nameText,bool isCutscene)
    {
        List<BackGroundPair> pairList;
        if (isCutscene)
        {
            pairList = cutSceneList;
        }
        else
        {
            pairList = backGroundList;
        }
        for(int i = 0; i < pairList.Count; i++)
        {
            if(pairList[i].fileName == nameText)
            {
                return pairList[i].backGroundSprite;
            }
        }
        StringBuilder nameBuilder;
        if (!isCutscene)
        {
            nameBuilder = new StringBuilder("BackGround/");
        }
        else
        {
            nameBuilder = new StringBuilder("CutScene/");
        }
        nameBuilder.Append(nameText);
        Sprite spr = Resources.Load<Sprite>(nameBuilder.ToString());
        BackGroundPair pair = new BackGroundPair(nameText, spr);
        pairList.Add(pair);
        return spr;
    }

    public string NameTranslator(string fileName, UILanguagePack languagePack)
    {
        if(fileName == null)
        {
            return null;
        }
        CharacterName name = (CharacterName)Enum.Parse(typeof(CharacterName), fileName);
        return languagePack.characterNameArray[(int)name];
    }

    //public int GetIndex(string name)
    //{
    //    for(int i = 0; i < characterNameArray.Length; i++)
    //    {
    //        if(name == characterNameArray[i])
    //        {
    //            return i;
    //        }
    //    }
    //    return -1;
    //}

    //public string GetName(int index)
    //{
    //    return characterNameArray[index];
    //}
}
