using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCharacter
{
    public VisitorType visitorType;
    public GameObject visitorObject;
    public int[] symptomAmountArray;
    protected static List<MedicineClass> ownedMedicineList;
    protected static RandomVisitorDiseaseBundle diseaseBundle;
    public List<RandomVisitorDisease> diseaseList;
    protected List<SymptomObject> symptomObjectList;
    protected List<SymptomObject> finalSymptomObjectList;

    public static void SetStaticData(List<MedicineClass> ownedMedicineList,
    RandomVisitorDiseaseBundle bundle)
    {
        BossCharacter.ownedMedicineList = ownedMedicineList;
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



}
