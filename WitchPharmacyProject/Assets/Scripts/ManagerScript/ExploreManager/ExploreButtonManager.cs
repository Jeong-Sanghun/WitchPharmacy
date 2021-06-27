using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExploreButtonManager : MonoBehaviour
{
    ExploreManager exploreManager;
    GameManager gameManager;
    SaveDataClass saveData;
    [SerializeField]
    Button[] regionOpenButtonArray;
    // Start is called before the first frame update
    void Start()
    {
        exploreManager = ExploreManager.inst;
        gameManager = GameManager.singleTon;
        saveData = gameManager.saveData;

        for(int i = 0; i < regionOpenButtonArray.Length; i++)
        {
            regionOpenButtonArray[i].interactable = false;
        }

        for(int i = 0; i < saveData.unlockedRegionIndex.Count; i++)
        {
            regionOpenButtonArray[saveData.unlockedRegionIndex[i]].interactable = true;
        }
    }



    public void OnButtonLoad(int index)
    {
        exploreManager = ExploreManager.inst;
        exploreManager.OnRegionLoad(index);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
