using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using Coffee.UIExtensions;

public class OddVisitorClass : VisitorClass
{
    public SpecialVisitorCondition condition;
    string characterName;
    List<string> diseaseNameList;
    CharacterIndexToName loader;
    int[] visitorSetArray;

    public GameObjectWrapper[] partsWrapperArray;


    bool childSetParented;
    public OddVisitorClass(GameObject parent, GameObject prefab, VisitorDialogBundle bundle)
    {

        visitorType = VisitorType.Odd;
        diseaseList = new List<RandomVisitorDisease>();
        symptomObjectList = new List<SymptomObject>();
        finalSymptomObjectList = new List<SymptomObject>();
        symptomAmountArray = bundle.symptomNumberArray;
        characterName = bundle.startWrapperList[0].characterName;
        diseaseNameList = bundle.diseaseNameList;
        visitorSetArray = bundle.oddVisitorSetArray;
        loader = new CharacterIndexToName();
        RandomPartsGenerator(parent,bundle.storyRegion);
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
            for (int j = 0; j < diseaseNameList.Count; j++)
            {
                bool find = false;
                for (int k = 0; k < diseaseBundle.wrapperList[i].randomVisitorDiseaseArray.Length; k++)
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

    ////랜덤캐릭터 만드는 함수
    //void RandomPartsGenerator(GameObject parent, StoryRegion region)
    //{
    //    string path = "RandomCharacter/" + region.ToString() + "/";
    //    Transform visitorParent = parent.transform;
    //    GameObject visitor = new GameObject();
    //    visitor.transform.SetParent(visitorParent);
    //    visitorObject = visitor;

    //    visitorObject.transform.localPosition = Vector3.zero;

    //    //먼저 래퍼 7개를 만들고.
    //    partsIndex = new int[5];
    //    partsWrapperArray = new GameObjectWrapper[partsIndex.Length];
    //    RandomVisitorFX effect = RandomVisitorFX.None;
    //    for (int i = 0; i < diseaseList.Count; i++)
    //    {
    //        if (diseaseList[i].GetEffect() != RandomVisitorFX.None)
    //        {
    //            effect = diseaseList[i].GetEffect();
    //        }
    //        if (effect == RandomVisitorFX.GrayScale && diseaseList[i].GetEffect() == RandomVisitorFX.Shiny)
    //        {
    //            effect = diseaseList[i].GetEffect();
    //        }

    //    }

        
    //    for(int i = 0; i < partsIndex.Length; i++)
    //    {
    //        partsIndex[i] = visitorSetArray[i];
    //        Debug.Log(partsIndex[i]);

    //    }

    //    for (int i = 0; i < partsIndex.Length; i++)
    //    {
    //        partsWrapperArray[i] = new GameObjectWrapper();
            
    //        StringBuilder builder = new StringBuilder(path);
    //        if (effect == RandomVisitorFX.GrayScale)
    //        {
    //            builder.Append("gray");
    //        }
    //        else if (effect == RandomVisitorFX.Shiny)
    //        {
    //            builder.Append("shiny");
    //        }
    //        else if (effect == RandomVisitorFX.Transparent)
    //        {
    //            if (i != 1)
    //            {
    //                continue;
    //            }
    //        }
    //        switch (i)
    //        {
    //            case 0:
    //                builder.Append("body/");
    //                break;
    //            case 1:
    //                builder.Append("clothes/");
    //                break;
    //            case 2:
    //                builder.Append("head/");
    //                break;
    //            case 3:
    //                builder.Append("face/");
    //                break;
    //            case 4:
    //                builder.Append("hair/");
    //                break;
    //            default:
    //                break;

    //        }
    //        builder.Append(partsIndex[i]);
    //        partsWrapperArray[i].partsArray = Resources.LoadAll<GameObject>(builder.ToString());
    //    }

    //    headPart = null;
    //    facePart = new GameObject[2];
    //    for (int i = 0; i < partsWrapperArray.Length; i++)
    //    {
    //        if (partsWrapperArray[i].partsArray == null)
    //        {
    //            continue;
    //        }
    //        for (int j = 0; j < partsWrapperArray[i].partsArray.Length; j++)
    //        {
    //            GameObject part = GameObject.Instantiate(partsWrapperArray[i].partsArray[j], visitor.transform);
    //            if (i == 3)
    //            {
    //                facePart[j] = part;
    //            }
    //            if (i == 2)
    //            {
    //                headPart = part;
    //            }
    //            if (i == 4 && j == 1)
    //            {
    //                part.transform.localPosition = new Vector3(0, 0, 1.5f);
    //            }
    //            else
    //            {
    //                part.transform.localPosition = new Vector3(0, 0, 1 - 0.1f * (i + 1) - 0.01f * (j + 1));
    //            }
    //        }
    //    }

    //}


    public override void StartSymptomSpriteUpdate()
    {
        childSetParented = false;
        //partsWrapperArray[2].partsArray[0] 이게 헤드임.
        for (int i = 0; i < diseaseList.Count; i++)
        {

            if (diseaseList[i].firstSpriteName != null)
            {
                GameObject obj = diseaseList[i].LoadObject(true, visitorObject.transform).transform.GetChild(0).gameObject;
                UIDissolve dissolve = obj.GetComponent<UIDissolve>();
                dissolve.effectFactor = 0;
                SymptomObject symptomObject = new SymptomObject();
                symptomObject.obj = obj;
                symptomObject.dissolve = true;
                //symptomObject.disease = diseaseList[i].sympotmString;
                symptomObject.amount = diseaseList[i].symptomNumber;
                symptomObject.dissolveComponent = dissolve;
                symptomObjectList.Add(symptomObject);
                if (diseaseList[i].firstSpriteName.Contains("Skin") && headPart != null)
                {
                    if (childSetParented == false)
                    {
                        for (int j = 0; j < facePart.Length; j++)
                        {
                            facePart[j].transform.GetChild(0).SetParent(headPart.transform.GetChild(0));
                        }
                    }
                    childSetParented = true;
                    obj.transform.SetParent(headPart.transform.GetChild(0));
                    obj.transform.SetAsLastSibling();
                    obj.transform.localScale = Vector3.one;
                    obj.transform.localPosition = Vector3.zero;


                }
                else
                {
                    obj.transform.parent.SetParent(visitorObject.transform);
                    obj.transform.localPosition = new Vector3(0, 0, diseaseList[i].GetFirstLayer());

                }


            }
            if (diseaseList[i].secondSpriteName != null)
            {
                GameObject obj = diseaseList[i].LoadObject(false, visitorObject.transform).transform.GetChild(0).gameObject;
                UIDissolve dissolve = obj.GetComponent<UIDissolve>();
                dissolve.effectFactor = 0;
                SymptomObject symptomObject = new SymptomObject();
                symptomObject.obj = obj;
                symptomObject.dissolve = true;
                symptomObject.dissolveComponent = dissolve;
                //symptomObject.disease = diseaseList[i].sympotmString;
                symptomObject.amount = diseaseList[i].symptomNumber;
                symptomObjectList.Add(symptomObject);
                if (diseaseList[i].secondSpriteName.Contains("Skin") && headPart != null)
                {
                    for (int j = 0; j < facePart.Length; j++)
                    {
                        facePart[j].transform.GetChild(0).SetParent(headPart.transform.GetChild(0));
                    }
                    obj.transform.SetParent(headPart.transform.GetChild(0).transform);
                    obj.transform.SetAsLastSibling();
                    obj.transform.localScale = Vector3.one;
                    obj.transform.localPosition = Vector3.zero;
                }
                else
                {
                    obj.transform.parent.SetParent(visitorObject.transform);
                    obj.transform.localPosition = new Vector3(0, 0, diseaseList[i].GetSecondLayer());

                }

            }

        }
    }

    public override void FinalSymptomSpriteUpdate(int[] finalSymptomArray)
    {
        List<RandomVisitorDisease> finalDiseaseList = new List<RandomVisitorDisease>();
        //for(int i = 0; i < symptomObjectList.Count; i++)
        //{
        //    symptomObjectList[i].SetActive(false);
        //}
        for (int i = 0; i < finalSymptomArray.Length; i++)
        {
            int amount = finalSymptomArray[i];
            if (amount == 0)
            {
                continue;
            }
            if (amount < -2)
            {
                amount = -2;
            }
            else if (amount > 2)
            {
                amount = 2;
            }

            if (amount == symptomAmountArray[i])
            {
                for (int j = 0; j < symptomObjectList.Count; j++)
                {
                    if (symptomObjectList[j].symptom == (Symptom)i)
                    {
                        symptomObjectList[j].dissolve = false;
                    }
                }
                continue;
            }

            List<int> diseaseIndexList = new List<int>();
            for (int j = 0; j < diseaseBundle.wrapperList[i].randomVisitorDiseaseArray.Length; j++)
            {
                if (amount == diseaseBundle.wrapperList[i].randomVisitorDiseaseArray[j].symptomNumber)
                {

                    diseaseIndexList.Add(j);
                }
            }
            if (diseaseIndexList.Count == 0)
            {
                return;
            }
            int index = diseaseIndexList[Random.Range(0, diseaseIndexList.Count)];
            finalDiseaseList.Add(diseaseBundle.wrapperList[i].randomVisitorDiseaseArray[index]);
        }


        for (int i = 0; i < finalDiseaseList.Count; i++)
        {

            if (finalDiseaseList[i].firstSpriteName != null)
            {
                GameObject obj = finalDiseaseList[i].LoadObject(true, visitorObject.transform).transform.GetChild(0).gameObject;
                UIDissolve dissolve = obj.GetComponent<UIDissolve>();
                dissolve.effectFactor = 1;
                SymptomObject symptomObject = new SymptomObject();
                symptomObject.obj = obj;
                symptomObject.dissolve = true;
                symptomObject.dissolveComponent = dissolve;
                finalSymptomObjectList.Add(symptomObject);
                if (finalDiseaseList[i].firstSpriteName.Contains("Skin") && headPart != null)
                {
                    if (childSetParented == false)
                    {
                        for (int j = 0; j < facePart.Length; j++)
                        {

                            facePart[j].transform.GetChild(0).SetParent(headPart.transform.GetChild(0));
                        }
                    }
                    childSetParented = true;
                    obj.transform.SetParent(headPart.transform.GetChild(0));
                    obj.transform.SetAsLastSibling();
                    obj.transform.localScale = Vector3.one;
                    obj.transform.localPosition = Vector3.zero;


                }
                else
                {
                    obj.transform.parent.SetParent(visitorObject.transform);
                    obj.transform.localPosition = new Vector3(0, 0, finalDiseaseList[i].GetFirstLayer());

                }


            }
            if (finalDiseaseList[i].secondSpriteName != null)
            {
                GameObject obj = finalDiseaseList[i].LoadObject(false, visitorObject.transform).transform.GetChild(0).gameObject;
                UIDissolve dissolve = obj.GetComponent<UIDissolve>();
                dissolve.effectFactor = 1;
                SymptomObject symptomObject = new SymptomObject();
                symptomObject.obj = obj;
                symptomObject.dissolveComponent = dissolve;
                symptomObject.dissolve = true;
                finalSymptomObjectList.Add(symptomObject);
                if (finalDiseaseList[i].secondSpriteName.Contains("Skin") && headPart != null)
                {
                    for (int j = 0; j < facePart.Length; j++)
                    {
                        facePart[j].transform.GetChild(0).SetParent(headPart.transform.GetChild(0));
                    }
                    obj.transform.SetParent(headPart.transform.GetChild(0).transform);
                    obj.transform.SetAsLastSibling();
                    obj.transform.localScale = Vector3.one;
                    obj.transform.localPosition = Vector3.zero;
                }
                else
                {
                    obj.transform.parent.SetParent(visitorObject.transform);
                    obj.transform.localPosition = new Vector3(0, 0, finalDiseaseList[i].GetSecondLayer());

                }

            }

        }
    }
}
