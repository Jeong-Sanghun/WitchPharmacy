using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCharacter
{
    BossData bossData;
    public GameObject bossObject;
    public SpriteRenderer bossSpriteRenderer;
    public BossSymptom[] bossSymptomArray;


    public BossCharacter(GameObject obj,BossData data)
    {
        bossData = data;
        bossSymptomArray = new BossSymptom[3];
        bossObject = obj;
        bossSpriteRenderer = bossObject.GetComponent<SpriteRenderer>();
    }

    public void SetSprite()
    {
        CharacterIndexToName tool = new CharacterIndexToName();
        bossSpriteRenderer.sprite = tool.GetSprite(bossData.characterEnum, CharacterFeeling.nothing);
    }


    public int TakeIndexOfSymptom(BossSymptom symptom)
    {
        for (int i = 0; i < bossSymptomArray.Length; i++)
        {
            if (bossSymptomArray[i] == symptom)
            {
                return i;
            }
        }
        return -1;
    }
    public void CorrectMedicine(BossSymptom symptom)
    {
        int index = TakeIndexOfSymptom(symptom);
        bossSymptomArray[index] = null;
        symptom.symptomObjectParent.SetActive(false);
    }
}
