using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResearchButtonManager : MonoBehaviour
{
    [SerializeField]
    GameObject medicineTab;
    [SerializeField]
    GameObject measureToolTab;
    [SerializeField]
    Transform mainCanvasTransform;


    
    public void MedicineTabButton()
    {
        medicineTab.SetActive(true);
        measureToolTab.SetActive(false);
        for(int i = 0; i < mainCanvasTransform.childCount; i++)
        {
            if(mainCanvasTransform.GetChild(i).gameObject.activeSelf == true)
            {
                mainCanvasTransform.GetChild(i).gameObject.SetActive(false);

            }
        }
    }

    public void MeasureToolTabButton()
    {

        medicineTab.SetActive(false);
        measureToolTab.SetActive(true);
        for (int i = 0; i < mainCanvasTransform.childCount; i++)
        {
            if (mainCanvasTransform.GetChild(i).gameObject.activeSelf == true)
            {
                mainCanvasTransform.GetChild(i).gameObject.SetActive(false);

            }
        }
    }
}
