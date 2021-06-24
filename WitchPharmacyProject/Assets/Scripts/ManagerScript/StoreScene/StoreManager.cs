using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
    
    [SerializeField]
    GameObject medicineTab;
    [SerializeField]
    GameObject toolTab;

    public void MedicineTabButton()
    {
        medicineTab.SetActive(true);
        toolTab.SetActive(false);
    }
    public void ToolTabButton()
    {
        medicineTab.SetActive(false);
        toolTab.SetActive(true);
    }
}
