using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResearchButtonManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] tabArray;
    [SerializeField]
    Transform mainCanvasTransform;

    void Start()
    {
        tabArray[0].SetActive(true);
        tabArray[1].SetActive(false);
        tabArray[2].SetActive(false);
        tabArray[3].SetActive(false);
    }


    public void TabButton(int index)
    {
        tabArray[index].SetActive(true);
        for(int i = 0; i < tabArray.Length; i++)
        {
            if(i == index)
            {
                continue;
            }
            if(tabArray[i].activeSelf == true)
            {
                tabArray[i].SetActive(false);
            }
        }
        for(int i = 0; i < mainCanvasTransform.childCount; i++)
        {
            if(mainCanvasTransform.GetChild(i).gameObject.activeSelf == true)
            {
                mainCanvasTransform.GetChild(i).gameObject.SetActive(false);

            }
        }
    }


}
