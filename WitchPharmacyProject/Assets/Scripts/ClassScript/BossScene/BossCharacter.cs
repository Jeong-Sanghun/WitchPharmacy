using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCharacter
{
    BossData bossData;
    public GameObject bossObject;
    public SpriteRenderer bossSpriteRenderer;
    public List<BossSymptom> bossSymptomList;


    public BossCharacter(GameObject obj,BossData data)
    {
        bossData = data;
        bossSymptomList = new List<BossSymptom>();
        bossObject = obj;
        bossSpriteRenderer = bossObject.GetComponent<SpriteRenderer>();
    }

    public void SetSprite()
    {
        CharacterIndexToName tool = new CharacterIndexToName();
        tool.GetSprite(bossData.characterEnum, CharacterFeeling.nothing);
        bossSpriteRenderer.sprite = 
    }
}
