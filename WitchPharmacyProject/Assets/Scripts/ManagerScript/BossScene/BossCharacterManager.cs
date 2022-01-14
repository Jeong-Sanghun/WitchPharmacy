using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCharacterManager : MonoBehaviour
{
    BossData bossData;
    BossCharacter bossCharacter;
    List<MedicineClass> medicineDataList;

    [SerializeField]
    BossMedicineManager bossMedicineManager;
    [SerializeField]
    BossCenterManager bossCenterManager;
    [SerializeField]
    GameObject bossObject;
    [SerializeField]
    GameObject symptomObjectPrefab;
    [SerializeField]
    GameObject[] symptomPositionObjectArray;

    // Start is called before the first frame update
    void Start()
    {
        bossData = bossCenterManager.bossData;
        bossCharacter = new BossCharacter(bossObject,bossData);
        for(int i = 0; i < bossCharacter.bossSymptomArray.Length; i++)
        {
            GameObject obj = Instantiate(symptomObjectPrefab, symptomPositionObjectArray[i].transform);
            obj.transform.localPosition = Vector3.zero;
            bossCharacter.bossSymptomArray[i]= new BossSymptom(obj);
        }
        medicineDataList = GameManager.singleton.medicineDataWrapper.medicineDataList;
    }

    public void OnMedicineDelivery(CookedMedicine medicine, GameObject symptomObject)
    {
        BossSymptom nowSymptom = null;
        for(int i = 0; i < bossCharacter.bossSymptomArray.Length; i++)
        {
            if(bossCharacter.bossSymptomArray[i] == null)
            {
                continue;
            }
            if(symptomObject == bossCharacter.bossSymptomArray[i].symptomObjectParent)
            {
                nowSymptom = bossCharacter.bossSymptomArray[i];
                break;
            }
        }
        if(nowSymptom == null)
        {
            return;
        }
        bool wrongMedicine = false;
        int[] medicineIndexArray = medicine.medicineArray;
        int[] medicineSymptomArray = new int[5];
        int[] finalSymptomArray = new int[5];
        int[] visitorSymptomArray = nowSymptom.symptomAmountArray;

        List<Symptom> badSymptomList = new List<Symptom>();
        for (int i = 0; i < 5; i++)
        {
            finalSymptomArray[i] = 0;
            medicineSymptomArray[i] = 0;
        }
        for (int i = 0; i < medicine.medicineCount; i++)
        {
            Debug.Log(medicineIndexArray[i]);
            MedicineClass med = medicineDataList[medicineIndexArray[i]];
            //if (med.firstSymptom == Symptom.none)
            //{
            //    continue;
            //}
            medicineSymptomArray[(int)med.GetFirstSymptom()] += med.firstNumber;
            medicineSymptomArray[(int)med.GetSecondSymptom()] += med.secondNumber;
        }
        for (int i = 0; i < 5; i++)
        {
            finalSymptomArray[i] = medicineSymptomArray[i] + visitorSymptomArray[i];
            if (finalSymptomArray[i] != 0)
            {
                wrongMedicine = true;
                badSymptomList.Add((Symptom)i);
            }
        }
        if (wrongMedicine)
        {
            return;
        }
        else
        {
            int index = bossCharacter.TakeIndexOfSymptom(nowSymptom);
            bossCharacter.CorrectMedicine(nowSymptom);
            GameObject obj = Instantiate(symptomObjectPrefab, symptomPositionObjectArray[index].transform);
            obj.transform.localPosition = Vector3.zero;
            bossCharacter.bossSymptomArray[index] = new BossSymptom(obj);
            
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
