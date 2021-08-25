using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class SpecialVisitorClass : VisitorClass
{
    public SpriteRenderer spriteRenderer;
    public SpecialVisitorCondition condition;
    string characterName;
    List<string> diseaseNameList;
    CharacterIndexToName loader;
    public SpecialVisitorClass(GameObject parent,GameObject prefab,VisitorDialogBundle bundle)
    {

        visitorType = VisitorType.Special;
        diseaseList = new List<RandomVisitorDisease>();
        symptomObjectList = new List<SymptomObject>();
        finalSymptomObjectList = new List<SymptomObject>();
        symptomAmountArray = bundle.symptomNumberArray;
        characterName = bundle.startWrapperList[0].characterName;
        diseaseNameList = bundle.diseaseNameList;
        GameObject part = GameObject.Instantiate(prefab, parent.transform);
        visitorObject = part;
        spriteRenderer = part.GetComponent<SpriteRenderer>();
        loader = new CharacterIndexToName();
        part.SetActive(true);
        SetSpecialDiseaseList();
        StartSymptomSpriteUpdate();

    }
    protected void SetSpecialDiseaseList()
    {
        for (int i = 0; i < symptomAmountArray.Length; i++)
        {
            int amount = symptomAmountArray[i];
            if (amount == 0)
            {
                continue;
            }
            for(int j = 0; j < diseaseNameList.Count; j++)
            {
                bool find = false;
                for(int k = 0; k < diseaseBundle.wrapperList[i].randomVisitorDiseaseArray.Length; k++)
                {
                    if (diseaseNameList[j].Contains(diseaseBundle.wrapperList[i].randomVisitorDiseaseArray[k].symptomName))
                    {
                        find = true;
                        diseaseList.Add(diseaseBundle.wrapperList[i].randomVisitorDiseaseArray[k]);
                        break;
                    }
                }
                if (find)
                {
                    break;
                }
            }



        }
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
