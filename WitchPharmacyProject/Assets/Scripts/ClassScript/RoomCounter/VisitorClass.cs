using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Coffee.UIExtensions;
using System.Text;

public class VisitorClass
{
    public VisitorType visitorType;
    public GameObject visitorObject;
    public int[] symptomAmountArray;
    protected static List<MedicineClass> ownedMedicineList;
    protected static RandomVisitorDiseaseBundle diseaseBundle;
    public List<RandomVisitorDisease> diseaseList;
    protected List<SymptomObject> symptomObjectList;
    protected List<SymptomObject> finalSymptomObjectList;
    protected StoryRegion nowRegion;
    protected static int[] bodyPartsNum = { 2, 2, 2, 2 };
    protected static int[] partsNum = { 2, 2, 2 };
    protected int[] partsIndex;
    protected int bodyPartsIndex;
    protected GameObject headPart;
    protected GameObject[] facePart;
    protected GameObject happyFace;
    protected GameObject angryFace;

    public static void SetStaticData(List<MedicineClass> ownedMedicineList,
    RandomVisitorDiseaseBundle bundle)
    {
        VisitorClass.ownedMedicineList = ownedMedicineList;
        diseaseBundle = bundle;
    }

    protected void SetDiseaseList()
    {
        for (int i = 0; i < symptomAmountArray.Length; i++)
        {
            int amount = symptomAmountArray[i];
            if (amount == 0)
            {
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
            int index = diseaseIndexList[Random.Range(0, diseaseIndexList.Count)];
            diseaseList.Add(diseaseBundle.wrapperList[i].randomVisitorDiseaseArray[index]);

        }
    }

    public virtual void StartSymptomSpriteUpdate()
    {
        //partsWrapperArray[2].partsArray[0] 이게 헤드임.
        for (int i = 0; i < diseaseList.Count; i++)
        {

            if (diseaseList[i].firstSpriteName != null)
            {
                GameObject obj = GameObject.Instantiate(diseaseList[i].LoadObject(true), visitorObject.transform).transform.GetChild(0).gameObject;
                UIDissolve dissolve = obj.GetComponent<UIDissolve>();
                dissolve.effectFactor = 0;
                SymptomObject symptomObject = new SymptomObject();
                symptomObject.obj = obj;
                symptomObject.dissolve = true;

                symptomObject.amount = diseaseList[i].symptomNumber;
                symptomObject.dissolveComponent = dissolve;
                symptomObjectList.Add(symptomObject);

                    obj.transform.parent.SetParent(visitorObject.transform);
                    obj.transform.localPosition = new Vector3(0, 0, diseaseList[i].GetFirstLayer());

            }
            if (diseaseList[i].secondSpriteName != null)
            {
                GameObject obj = GameObject.Instantiate(diseaseList[i].LoadObject(false), visitorObject.transform).transform.GetChild(0).gameObject;
                UIDissolve dissolve = obj.GetComponent<UIDissolve>();
                dissolve.effectFactor = 0;
                SymptomObject symptomObject = new SymptomObject();
                symptomObject.obj = obj;
                symptomObject.dissolve = true;
                symptomObject.dissolveComponent = dissolve;
                
                symptomObject.amount = diseaseList[i].symptomNumber;
                symptomObjectList.Add(symptomObject);

                    obj.transform.parent.SetParent(visitorObject.transform);
                    obj.transform.localPosition = new Vector3(0, 0, diseaseList[i].GetSecondLayer());


            }

        }
    }



    public virtual void FinalSymptomSpriteUpdate(int[] finalSymptomArray)
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
                GameObject obj = GameObject.Instantiate(finalDiseaseList[i].LoadObject(true), visitorObject.transform).transform.GetChild(0).gameObject;
                UIDissolve dissolve = obj.GetComponent<UIDissolve>();
                dissolve.effectFactor = 1;
                SymptomObject symptomObject = new SymptomObject();
                symptomObject.obj = obj;
                symptomObject.dissolve = true;
                symptomObject.dissolveComponent = dissolve;
                finalSymptomObjectList.Add(symptomObject);

                    obj.transform.parent.SetParent(visitorObject.transform);
                    obj.transform.localPosition = new Vector3(0, 0, finalDiseaseList[i].GetFirstLayer());


            }
            if (finalDiseaseList[i].secondSpriteName != null)
            {
                GameObject obj = GameObject.Instantiate(finalDiseaseList[i].LoadObject(false), visitorObject.transform).transform.GetChild(0).gameObject;
                UIDissolve dissolve = obj.GetComponent<UIDissolve>();
                dissolve.effectFactor = 1;
                SymptomObject symptomObject = new SymptomObject();
                symptomObject.obj = obj;
                symptomObject.dissolveComponent = dissolve;
                symptomObject.dissolve = true;
                finalSymptomObjectList.Add(symptomObject);

                    obj.transform.parent.SetParent(visitorObject.transform);
                    obj.transform.localPosition = new Vector3(0, 0, finalDiseaseList[i].GetSecondLayer());
            }

        }
    }

    public IEnumerator FinalDissolve()
    {
        yield return null;
        float timer = 0;

        while (timer < 1)
        {
            timer += Time.deltaTime / 3f;
            for (int i = 0; i < symptomObjectList.Count; i++)
            {
                if (symptomObjectList[i].dissolve == false)
                {
                    continue;
                }
                symptomObjectList[i].dissolveComponent.effectFactor = timer;
            }
            for (int i = 0; i < finalSymptomObjectList.Count; i++)
            {
                if (finalSymptomObjectList[i].dissolve == false)
                {
                    continue;
                }
                finalSymptomObjectList[i].dissolveComponent.effectFactor = 1 - timer;
            }
            yield return null;
        }
    }
    public virtual void FaceShifter(bool isAngry)
    {
        if (isAngry)
        {
            angryFace.SetActive(true);
            happyFace.SetActive(false);
        }
        else
        {
            happyFace.SetActive(true);
            angryFace.SetActive(false);
        }
    }

    protected void RandomPartsGenerator(GameObject parent, StoryRegion region)
    {
        string path = "RandomCharacter/Whole/";
        string bodyPath = "RandomCharacter/" + region.ToString() + "/body/";
        int[] partsIndex;
        GameObjectWrapper[] partsWrapperArray;
        Transform visitorParent = parent.transform;
        GameObject visitor = new GameObject();
        visitor.transform.SetParent(visitorParent);
        visitorObject = visitor;

        visitorObject.transform.localPosition = Vector3.zero;

        //먼저 래퍼 7개를 만들고.
        partsIndex = new int[4];
        //head face hair body
        partsWrapperArray = new GameObjectWrapper[4];
        //RandomVisitorFX effect = RandomVisitorFX.None;
        //for(int i = 0; i < diseaseList.Count; i++)
        //{
        //    if(diseaseList[i].GetEffect() != RandomVisitorFX.None)
        //    {
        //        effect = diseaseList[i].GetEffect();
        //    }
        //    if(effect == RandomVisitorFX.GrayScale && diseaseList[i].GetEffect() == RandomVisitorFX.Shiny)
        //    {
        //        effect = diseaseList[i].GetEffect();
        //    }

        //}
        partsIndex[3] = Random.Range(0, bodyPartsNum[(int)region]);
        partsWrapperArray[3] = new GameObjectWrapper();
        partsWrapperArray[3].partsArray = Resources.LoadAll<GameObject>(bodyPath + bodyPartsIndex.ToString());
        for (int i = 0; i < partsIndex.Length - 1; i++)
        {
            partsWrapperArray[i] = new GameObjectWrapper();
            partsIndex[i] = Random.Range(0, partsNum[i]);
            StringBuilder builder = new StringBuilder(path);
            //if(effect == RandomVisitorFX.GrayScale)
            //{
            //    builder.Append("gray");
            //}
            //else if (effect == RandomVisitorFX.Shiny)
            //{
            //    builder.Append("shiny");
            //}
            //else if (effect == RandomVisitorFX.Transparent)
            //{
            //    if(i != 1)
            //    {
            //        continue;
            //    }
            //}
            switch (i)
            {
                case 0:
                    builder.Append("head/");
                    break;
                case 1:
                    builder.Append("face/");
                    break;
                case 2:
                    builder.Append("hair/");
                    break;
                default:
                    break;

            }
            builder.Append(partsIndex[i]);
            partsWrapperArray[i].partsArray = Resources.LoadAll<GameObject>(builder.ToString());
        }

        headPart = null;
        facePart = new GameObject[2];
        for (int i = 0; i < partsWrapperArray.Length; i++)
        {
            if (partsWrapperArray[i].partsArray == null)
            {
                continue;
            }
            for (int j = 0; j < partsWrapperArray[i].partsArray.Length; j++)
            {
                //head face hair body
                GameObject part = GameObject.Instantiate(partsWrapperArray[i].partsArray[j], visitor.transform);
                if (i == 1)
                {
                    facePart[j] = part;
                    if (j == 1)
                    {
                        angryFace = part.transform.GetChild(0).gameObject;
                        angryFace.SetActive(false);
                    }
                    else
                    {
                        happyFace = part.transform.GetChild(0).gameObject;
                    }
                }
                if (i == 0)
                {
                    headPart = part;
                }
                if (i == 2 && j == 1)
                {
                    part.transform.localPosition = new Vector3(0, 0, 1.5f);
                }
                else if(i==2 && j == 2)
                {
                    part.transform.localPosition = new Vector3(0, 0, 1.6f);
                }
                else if (i == 3)
                {
                    part.transform.localPosition = new Vector3(0, 0, 1);
                }
                else
                {
                    part.transform.localPosition = new Vector3(0, 0, 1 - 0.1f * (i + 1) - 0.01f * (j + 1));
                }
            }
        }

    }
}
