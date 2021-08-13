using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class SpecialVisitorClass : VisitorClass
{
    public SpriteRenderer spriteRenderer;
    public SpecialVisitorCondition condition;
    string characterName;
    CharacterIndexToName loader;
    public SpecialVisitorClass(GameObject parent,GameObject prefab,SpecialVisitorCondition cond)
    {

        visitorType = VisitorType.Special;
        condition = cond;
        diseaseList = new List<RandomVisitorDisease>();
        symptomObjectList = new List<SymptomObject>();
        finalSymptomObjectList = new List<SymptomObject>();
        symptomAmountArray = cond.symptomNumberArray;
        characterName = cond.characterName;
        GameObject part = GameObject.Instantiate(prefab, parent.transform);
        visitorObject = part;
        spriteRenderer = part.GetComponent<SpriteRenderer>();
        loader = new CharacterIndexToName();
        part.SetActive(true);
        SetDiseaseList();
        StartSymptomSpriteUpdate();

    }

    public SpecialVisitorClass(GameObject parent, GameObject prefab, string characterName, string characterFeeling)
    {
        GameObject part = prefab;
        visitorObject = part;
        this.characterName = characterName;
        spriteRenderer = part.GetComponent<SpriteRenderer>();
        loader = new CharacterIndexToName();
        SetObjectImage(characterName, characterFeeling);
        prefab.SetActive(true);
    }


    public void SetObjectImage(string characterName,string feeling)
    {
        //string path = "CharacterSprite/";
        //StringBuilder builder = new StringBuilder(path);
        //builder.Append(characterName);
        //builder.Append("/");
        //builder.Append(feeling);
        spriteRenderer.sprite = loader.GetSprite(characterName, feeling);
    }
}
