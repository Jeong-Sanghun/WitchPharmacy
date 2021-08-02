using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MedicineResearchDataWrapper
{
    public List<MedicineResearchData> medicineResearchDataList;

    public MedicineResearchDataWrapper()
    {
        medicineResearchDataList = new List<MedicineResearchData>();
        medicineResearchDataList.Add(new MedicineResearchData());
        medicineResearchDataList.Add(new MedicineResearchData());
        medicineResearchDataList.Add(new MedicineResearchData());
        medicineResearchDataList.Add(new MedicineResearchData());
        medicineResearchDataList.Add(new MedicineResearchData());
        medicineResearchDataList.Add(new MedicineResearchData());
        medicineResearchDataList.Add(new MedicineResearchData());
        medicineResearchDataList.Add(new MedicineResearchData());
        medicineResearchDataList.Add(new MedicineResearchData());
    }
}
