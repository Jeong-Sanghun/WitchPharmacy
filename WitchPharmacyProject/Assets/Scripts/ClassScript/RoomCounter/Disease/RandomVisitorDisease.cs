using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System;


public enum RandomVisitorFX
{
    None,Shiny, GrayScale, Transparent
}
[System.Serializable]
public class RandomVisitorDisease
{
    public string symptomName;
    public int symptomNumber;
    [System.NonSerialized]
    RandomVisitorFX effect;
    [System.NonSerialized]
    public Symptom symptom;
    public string effectString;
    public string dialog;
    public string firstSpriteName;
    Sprite firstSprite;
    GameObject firstObj;
    GameObject secondObj;


    public string secondSpriteName;
    Sprite secondSprite;
    public string firstSpriteLayer;
    float firstSpriteLayerNumber;
    public string secondSpriteLayer;
    float secondSpriteLayerNumber;

    bool firstParsed=false;
    bool secondParsed=false;
    bool effectParsed = false;

    static GameObject prefab;


    public static void SetStaticData(GameObject diseasePrefab)
    {
        prefab = diseasePrefab;
    }
    public Sprite LoadImage(bool first)
    {
        Sprite nowSprite = null;
        string spriteName;
        if(first == true)
        {
            spriteName = firstSpriteName;
            nowSprite = firstSprite;
        }
        else{
            spriteName = secondSpriteName;
            nowSprite = secondSprite;
        }
        if (nowSprite != null)
        {
            return nowSprite;
        }
        StringBuilder builder = new StringBuilder("RandomCharacter/Disease/");
        builder.Append(spriteName);
        nowSprite = Resources.Load<Sprite>(builder.ToString());
        if (first == true)
        {
            firstSprite =nowSprite;
        }
        else
        {
           secondSprite= nowSprite ;
        }
        return nowSprite;
    }

    //public GameObject LoadObject(bool first)
    //{
    //    GameObject nowObject = null;
    //    string objectName;
    //    if (first == true)
    //    {
    //        objectName = firstSpriteName;
    //        nowObject = firstObj;
    //    }
    //    else
    //    {
    //        objectName = secondSpriteName;
    //        nowObject = secondObj;
    //    }
    //    if (nowObject != null)
    //    {
    //        return nowObject;
    //    }
    //    StringBuilder builder = new StringBuilder("RandomCharacter/Disease/");
    //    builder.Append(objectName);
    //    nowObject = Resources.Load<GameObject>(builder.ToString());
    //    return nowObject;
    //}

    public GameObject LoadObject(bool first,Transform parent)
    {
        GameObject nowObject = null;
        string objectName;
        if (first == true)
        {
            objectName = firstSpriteName;
            nowObject = firstObj;
        }
        else
        {
            objectName = secondSpriteName;
            nowObject = secondObj;
        }
        if (nowObject != null)
        {
            return nowObject;
        }
        nowObject = GameObject.Instantiate(prefab, parent);
        if (first == true)
        {

            nowObject.transform.GetChild(0).GetComponent<Image>().sprite = LoadImage(true) ;
        }
        else
        {

            nowObject.transform.GetChild(0).GetComponent<Image>().sprite = LoadImage(false);
        }
        nowObject.name = objectName;
        return nowObject;
    }

    public RandomVisitorFX GetEffect()
    {
        if (effectParsed)
        {
            return effect;
        }
        effectParsed = true;
        effect =  (RandomVisitorFX)Enum.Parse(typeof(RandomVisitorFX), effectString);
        return effect;
    }

    public float GetFirstLayer()
    {
        if (firstParsed)
        {
            return firstSpriteLayerNumber;
        }
        firstParsed = true;
        float.TryParse(firstSpriteLayer, out firstSpriteLayerNumber);
        return firstSpriteLayerNumber;
    }

    public float GetSecondLayer()
    {
        if (secondParsed)
        {
            return secondSpriteLayerNumber;
        }
        secondParsed = true;
        float.TryParse(secondSpriteLayer, out secondSpriteLayerNumber);
        return secondSpriteLayerNumber;
    }

    public RandomVisitorDisease()
    {
        symptomName = null;
        symptomNumber = -2;
        effect = RandomVisitorFX.None;
        dialog = "대본인데요";
        firstSpriteName = null;
        firstSpriteLayer = "0";
        firstSpriteLayerNumber = -1;

        secondSpriteName = null;
        secondSpriteLayer = "0";
        secondSpriteLayerNumber = -1;
        firstParsed = false;
        secondParsed = false;
        effectParsed = false;
    }
}
