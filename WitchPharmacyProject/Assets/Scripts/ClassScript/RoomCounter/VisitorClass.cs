using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Coffee.UIExtensions;

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
                symptomObject.disease = diseaseList[i].sympotmString;
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
                symptomObject.disease = diseaseList[i].sympotmString;
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
                    if (symptomObjectList[j].disease.Contains(((Symptom)i).ToString()))
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

}
